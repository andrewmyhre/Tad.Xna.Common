using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tad.Xna.Common.Cameras;

namespace Tad.Xna.Common.Avatars
{
    public class GodAvatar : AvatarBase
    {
        int centerX, centerY;
        private BasicEffect effect;
        private VertexPositionColor[] vertices;
        private int[] indices;

        private int lastWheelValue;

        public GodAvatar(Game game, Vector3 position, string label) : base(game, label) 
        {
            Position = position;
            lastWheelValue = Mouse.GetState().ScrollWheelValue;
            ForwardSpeed *= 50f;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            centerX = GraphicsDevice.Viewport.Width / 2;
            centerY = GraphicsDevice.Viewport.Height / 2;
            Mouse.SetPosition(centerX, centerY);

            vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0,0,2), Color.Red),
                new VertexPositionColor(new Vector3(0,0,-2), Color.Red),
                new VertexPositionColor(new Vector3(-1,0,0), Color.LimeGreen),
                new VertexPositionColor(new Vector3(1,0,0), Color.LimeGreen),
                new VertexPositionColor(new Vector3(0,1,0), Color.Yellow),
                new VertexPositionColor(new Vector3(0,-1,0), Color.Yellow),
            };

            indices = new int[]
            {
                0,1,2,3,4,5
            };

            effect = new BasicEffect(GraphicsDevice);
            effect.VertexColorEnabled = true;
            
        }

        private Matrix _rotationMatrix;
        private Vector3 displacement;
        Vector3 direction = Vector3.Zero;
        public override void AcceptInput(KeyboardState keyboard, GamePadState current, MouseState mouse)
        {
            base.AcceptInput(keyboard,current,mouse);
            float mouseDelta = mouse.ScrollWheelValue - lastWheelValue;

            if (CaptureMouse)
            {
                ForwardSpeed += mouseDelta / 10;
                if (ForwardSpeed < 0f) ForwardSpeed = 0f;

                if (keyboard.IsKeyDown(Keys.Q))
                    smoothedMouseMovement.Z += 1 * RotationSpeed;
                if (keyboard.IsKeyDown(Keys.E))
                    smoothedMouseMovement.Z -= 1 * RotationSpeed;

                MouseRotation(mouse);
            }
            lastWheelValue = mouse.ScrollWheelValue;

            displacement = Vector3.Zero;
            
            if (keyboard.IsKeyDown(Keys.A) || (current.DPad.Left == ButtonState.Pressed))
            {
                // move left
                direction.X += 1;
            }
            if (keyboard.IsKeyDown(Keys.D) || (current.DPad.Right == ButtonState.Pressed))
            {
                // move right
                direction.X -= 1;
            }

            if (keyboard.IsKeyDown(Keys.W) || (current.DPad.Up == ButtonState.Pressed))
            {
                direction.Z += 1;
            }
            if (keyboard.IsKeyDown(Keys.S) || (current.DPad.Down == ButtonState.Pressed))
            {
                direction.Z -= 1;
            }

            

            if (CaptureMouse)
                Mouse.SetPosition(centerX, centerY);
        }

        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Avatar.Current != this) return;
            AcceptInput(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One), Mouse.GetState());
            
            Quaternion adjustedOrientation = new Quaternion(-_orientation.X, -_orientation.Y, -_orientation.Z, _orientation.W);
            _rotationMatrix = Matrix.Identity * Matrix.CreateFromQuaternion(adjustedOrientation);

            displacement = Vector3.Transform(-direction, _rotationMatrix) * ForwardSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += displacement;
            displacement = Vector3.Zero;
            direction = Vector3.Zero;

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            effect.View = Camera.Default.ViewMatrix;
            effect.Projection = Camera.Default.ProjectionMatrix;
            effect.World = Matrix.Identity
                            * Matrix.CreateScale(5)
                            * Matrix.CreateFromQuaternion(this._orientation)
                           *Matrix.CreateTranslation(this.Position);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList,vertices, 0, 6,
                    indices,0,3);
            }
        }
    }
}
