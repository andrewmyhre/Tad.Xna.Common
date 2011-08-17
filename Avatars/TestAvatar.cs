using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tad.Xna.Common.Avatars
{
    public class TestAvatar : AvatarBase
    {
        private BasicEffect effect = null;
        private int[] _boxIndices;
        private VertexPositionColor[] _boxCorners;

        public TestAvatar(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            BoundingBox box = new BoundingBox(new Vector3(-1,-1,-1),new Vector3(1,1,1) );
            Vector3[] boxCornersVertices = box.GetCorners();

            _boxCorners = new VertexPositionColor[boxCornersVertices.Length];
            for (int i = 0; i < 8; i++)
                _boxCorners[i] = new VertexPositionColor(boxCornersVertices[i], Color.White);

            _boxIndices = new int[24];
            _boxIndices[0] = 0;
            _boxIndices[1] = 1;
            _boxIndices[2] = 1;
            _boxIndices[3] = 2;
            _boxIndices[4] = 2;
            _boxIndices[5] = 3;
            _boxIndices[6] = 3;
            _boxIndices[7] = 0;
            _boxIndices[8] = 0;
            _boxIndices[9] = 4;
            _boxIndices[10] = 3;
            _boxIndices[11] = 7;
            _boxIndices[12] = 2;
            _boxIndices[13] = 6;
            _boxIndices[14] = 1;
            _boxIndices[15] = 5;
            _boxIndices[16] = 4;
            _boxIndices[17] = 5;
            _boxIndices[18] = 5;
            _boxIndices[19] = 6;
            _boxIndices[20] = 6;
            _boxIndices[21] = 7;
            _boxIndices[22] = 7;
            _boxIndices[23] = 4;

            effect = new BasicEffect(GraphicsDevice);
            effect.EnableDefaultLighting();
            effect.AmbientLightColor = new Vector3(150, 0, 0);
            effect.PreferPerPixelLighting = true;
            effect.LightingEnabled = true;
            effect.TextureEnabled = false;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!GraphicsDevice.IsDisposed)
            {
                effect.World = Matrix.Identity * Matrix.CreateTranslation(Position);
                effect.View = Cameras.Camera.Default.ViewMatrix;
                effect.Projection = Cameras.Camera.Default.ProjectionMatrix;

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                                                                                       _boxCorners, 0,
                                                                                       _boxCorners.Length,
                                                                                       _boxIndices, 0,
                                                                                       _boxIndices.Length/2);
                }

            }

            base.Draw(gameTime);
        }
    }
}
