using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tad.Xna.Common.Cameras;
using Tad.Xna.Common.Entities;

namespace Tad.Xna.Common
{
    public class TestEntity : GameEntity
    {
        BasicEffect effect;
        private VertexBuffer testVBuffer;
        private IndexBuffer testIBuffer;
        private VertexPositionColor[] vertices;
        private int[] indices;
        public TestEntity(Game game)
            : base(game)
        {
            IsStatic = true;
        }

        public override void Initialize()
        {
            base.Initialize();



            effect = new BasicEffect(Game.GraphicsDevice);
            effect.EnableDefaultLighting();
            effect.LightingEnabled = false;
            effect.AmbientLightColor = new Vector3(255, 255, 255);

            SetUpBuffers();
        }

        protected override void LoadContent()
        {
            base.LoadContent();


        }

        private void SetUpBuffers()
        {
            vertices = new VertexPositionColor[]
                           {
                               new VertexPositionColor(new Vector3(0,0,0), Color.White),
                               new VertexPositionColor(new Vector3(100,200,0), Color.White), 
                               new VertexPositionColor(new Vector3(200,0,0), Color.White), 
                           };
            testVBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            testVBuffer.SetData(vertices);

            indices = new int[] { 0, 1, 2 };

            testIBuffer = new IndexBuffer(Game.GraphicsDevice, typeof(int), 3, BufferUsage.WriteOnly);
            testIBuffer.SetData(indices);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetVertexBuffer(testVBuffer);
            GraphicsDevice.Indices = testIBuffer;

            effect.Projection = Camera.Default.ProjectionMatrix;
            effect.View = Camera.Default.ViewMatrix;
            effect.World = Matrix.Identity;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 3, indices, 0, 1);
            }
        }
    }
}
