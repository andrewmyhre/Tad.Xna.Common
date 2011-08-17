using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tad.Xna.Common.Entities;

namespace Tad.Xna.Common.Avatars
{
    public abstract class AvatarBase : GameEntity, IAvatar
    {
        public bool CaptureMouse { get; set; }
        protected static float viewAngle = MathHelper.PiOver4;
        public float NearClip { get { return 0.1f; } }
        public float FarClip { get { return 10000.0f; } }
        protected float cullFieldWidth = 80;

        protected BoundingFrustum _viewFrustum;
        public BoundingFrustum ViewFrustum { get { return _viewFrustum; } }
        protected BoundingFrustum _clippingFrustum;
        public BoundingFrustum ClippingFrustum { get { return _clippingFrustum; } }
        
        protected float RotationSpeed = 1/60f;
        protected float ForwardSpeed = 50f / 60f;
        public Matrix ViewMatrix;
        public Matrix ProjectionMatrix;

        protected Vector3 eyesPosition;
        public Vector3 EyesPosition { get { return eyesPosition;} }

        protected MouseState previousMouseState;
        protected KeyboardState previousKeyboardState;
        protected MouseState currentMouseState;
        protected KeyboardState currentKeyboardState;
        protected Vector3 smoothedMouseMovement;
        protected static Vector3 WORLD_X_AXIS = new Vector3(1.0f, 0.0f, 0.0f);
        protected static Vector3 WORLD_Y_AXIS = new Vector3(0.0f, 1.0f, 0.0f);
        protected static Vector3 WORLD_Z_AXIS = new Vector3(0.0f, 0.0f, 1.0f);
        

        public AvatarBase(Game game) : base(game) {
            
            Orientation = Quaternion.Identity;
            RotationSpeed *= 3f;
            ForwardSpeed *= 5f;
            _viewFrustum = new BoundingFrustum(Matrix.Identity);
            _clippingFrustum = new BoundingFrustum(Matrix.Identity);
        }

        public virtual void AcceptInput(KeyboardState keyboardState, GamePadState gamePadState, MouseState mouseState)
        {
            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;

            currentMouseState = mouseState;
            currentKeyboardState = keyboardState;

            if (keyboardState.IsKeyDown(Keys.Insert) && cullFieldWidth < 179)
                cullFieldWidth++;
            if (keyboardState.IsKeyDown(Keys.Delete) && cullFieldWidth > 1)
                cullFieldWidth--;
            if (keyboardState.IsKeyUp(Keys.OemComma) && ForwardSpeed > 0)
                ForwardSpeed -= 1f;
            if (keyboardState.IsKeyUp(Keys.OemPeriod))
                ForwardSpeed += 1f;
        }

        public virtual void SetPosition(Vector3 position)
        {
            this.Position = position;
        }

        public virtual void SetOrientation(Quaternion orientation)
        {
            this._orientation = orientation;
        }

        protected void MouseRotation(MouseState mouseState)
        {
            Rectangle clientBounds = Game.Window.ClientBounds;

            int centerX = clientBounds.Width/2;
            int centerY = clientBounds.Height/2;
            int deltaX = centerX - mouseState.X;
            int deltaY = centerY - mouseState.Y;

            if (CaptureMouse)
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

        public Ray CreateRay()
        {
            //Matrix rotationMatrix = Matrix.CreateFromQuaternion(_orientation);
            return new Ray(Position + (Vector3.Up * 0.5f), Vector3.Down);
        }

        protected Matrix _clippingMatrix;
        public override void Update(GameTime gameTime)
        {
            ViewMatrix = Matrix.Identity;
            ViewMatrix *= Matrix.CreateTranslation(-Position);
            ViewMatrix *= Matrix.CreateFromQuaternion(_orientation);

            Viewport viewport = Game.GraphicsDevice.Viewport;
            float aspectRatio = (float)viewport.Width / (float)viewport.Height;

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(viewAngle, aspectRatio, NearClip, FarClip);
            _clippingMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(cullFieldWidth), aspectRatio, NearClip, FarClip);

            _viewFrustum = new BoundingFrustum(ViewMatrix * ProjectionMatrix);
            _clippingFrustum= new BoundingFrustum(ViewMatrix * _clippingMatrix);

            base.Update(gameTime);
        }
    }
}