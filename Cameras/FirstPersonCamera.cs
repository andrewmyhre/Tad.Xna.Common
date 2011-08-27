//using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tad.Xna.Common.Avatars;
using Tad.Xna.Common.Entities;

namespace Tad.Xna.Common.Cameras
{
    public class FirstPersonCamera : GameEntity, ICamera
    {
        protected IGameEntity attachedTo;
        SpriteBatch spriteBatch;

        private SpriteFont font;

        // Set field of view of the camera in radians (pi/4 is 45 degrees).
        protected static float viewAngle = MathHelper.PiOver4;

        // Set distance from the camera of the near and far clipping planes.
        public float NearClip { get { return 0.1f; } }
        public float FarClip { get { return 10000.0f; } }
        private Vector3 EyesPosition=new Vector3();
        private BoundingFrustum _viewFrustum;


        public BoundingFrustum ViewFrustum { get { return _viewFrustum; } }
        private BoundingFrustum _clippingFrustum;
        public BoundingFrustum ClippingFrustum { get { return _clippingFrustum; } }

        private Matrix viewMatrix;
        public Matrix ViewMatrix { get { return viewMatrix; } set { viewMatrix = value;} }
        public Matrix ProjectionMatrix { get; set; }

        protected Vector3 smoothedMouseMovement;
        protected static Vector3 WORLD_X_AXIS = new Vector3(1.0f, 0.0f, 0.0f);
        protected static Vector3 WORLD_Y_AXIS = new Vector3(0.0f, 1.0f, 0.0f);
        protected static Vector3 WORLD_Z_AXIS = new Vector3(0.0f, 0.0f, 1.0f);
        protected float RotationSpeed = 1 / 60f;


        public FirstPersonCamera(Game game)
            : base(game)
        {
            
            Position = new Vector3(0, 0, 1);
            _viewFrustum = new BoundingFrustum(Matrix.Identity);
        }

        public void AttachTo(IGameEntity Entity)
        {
            attachedTo = Entity;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Game.Content.Load<SpriteFont>("Default");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (attachedTo == null)
                MouseRotation(Mouse.GetState());

            UpdateCamera();
            
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (attachedTo != null && attachedTo is IAvatar)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, ((IAvatar)attachedTo).Label, 
                    new Vector2(0,0), Color.White);
                spriteBatch.End();
                    
            }
        }

        protected void MouseRotation(MouseState mouseState)
        {
            Rectangle clientBounds = Game.Window.ClientBounds;

            int centerX = clientBounds.Width / 2;
            int centerY = clientBounds.Height / 2;
            int deltaX = centerX - mouseState.X;
            int deltaY = centerY - mouseState.Y;

            Mouse.SetPosition(centerX, centerY);

            smoothedMouseMovement.X = (float)deltaX * RotationSpeed;
            smoothedMouseMovement.Y = (float)deltaY * RotationSpeed;

            float headingRadians = MathHelper.ToRadians(-smoothedMouseMovement.X);
            float pitchRadians = MathHelper.ToRadians(-smoothedMouseMovement.Y);
            float rollRadians = MathHelper.ToRadians(-smoothedMouseMovement.Z);

            Rotate(headingRadians, pitchRadians, rollRadians);

            smoothedMouseMovement.Z = 0;
        }

        protected virtual void Rotate(float headingRadians, float pitchRadians, float rollRadians)
        {
            Quaternion rotation = Quaternion.Identity;

            // Rotate the camera about the world Y axis.
            if (headingRadians != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref WORLD_Y_AXIS, headingRadians, out rotation);
                Quaternion.Concatenate(ref _orientation, ref rotation, out _orientation);
            }

            // Rotate the camera about its local X axis.
            if (pitchRadians != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref WORLD_X_AXIS, pitchRadians, out rotation);
                Quaternion.Concatenate(ref _orientation, ref rotation, out _orientation);
            }

            // Rotate the camera about it's local Z axis
            if (rollRadians != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref WORLD_Z_AXIS, rollRadians, out rotation);
                Quaternion.Concatenate(ref _orientation, ref rotation, out _orientation);
            }
        }

        /// <summary>
        /// Updates the position and direction of the camera relative to the avatar.
        /// </summary>
        private Matrix _clippingMatrix, _clipViewMatrix;
        protected virtual void UpdateCamera()
        {
            // project the camera behind the box
            Vector3 cameraPosition;
            if (attachedTo != null)
            {
                cameraPosition = attachedTo.Position + EyesPosition;
                _orientation = attachedTo.Orientation;
            }
            else
                cameraPosition = Position + EyesPosition;

            viewMatrix = Matrix.Identity;
            viewMatrix *= Matrix.CreateTranslation(-cameraPosition);
            viewMatrix *= Matrix.CreateFromQuaternion(_orientation);

            Viewport viewport = Game.GraphicsDevice.Viewport;
            float aspectRatio = (float)viewport.Width / (float)viewport.Height;

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(viewAngle, aspectRatio, NearClip, FarClip);
            this.Position = cameraPosition;

            // determine culling frustum
            _clipViewMatrix = Matrix.Identity;
            _clipViewMatrix *= Matrix.CreateTranslation(-Position);
            _clipViewMatrix *= Matrix.CreateFromQuaternion(new Quaternion(-_orientation.X, -_orientation.Y, _orientation.Z, _orientation.W));

            _clippingMatrix = Matrix.CreatePerspectiveFieldOfView(viewAngle * 1.6f, aspectRatio, NearClip, FarClip);

            _viewFrustum = new BoundingFrustum(_clipViewMatrix * ProjectionMatrix);
            _clippingFrustum = new BoundingFrustum(_clipViewMatrix * _clippingMatrix);
        }

    }
}
