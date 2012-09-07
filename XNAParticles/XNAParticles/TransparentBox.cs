using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAParticles
{
    public sealed class TransparentBox
    {
        private BasicEffect fxBasic;

        private VertexPositionColor[] vertices;
        private short[] indices;

        private VertexBuffer vBuffer;
        private IndexBuffer iBuffer;


        public TransparentBox(GraphicsDevice graphics)
        {
            const int numVertices = 8;
            vertices = new VertexPositionColor[numVertices] {
                new VertexPositionColor(new Vector3(-10, 10, 10), Color.White),
                new VertexPositionColor(new Vector3(10, 10, 10), Color.White),
                new VertexPositionColor(new Vector3(10, -10, 10), Color.White),
                new VertexPositionColor(new Vector3(-10, -10, 10), Color.White),

                new VertexPositionColor(new Vector3(-10, 10, -10), Color.White),
                new VertexPositionColor(new Vector3(10, 10, -10), Color.White),
                new VertexPositionColor(new Vector3(10, -10, -10), Color.White),
                new VertexPositionColor(new Vector3(-10, -10, -10), Color.White),
            };

            indices = new short[24] {
                0, 1,
                1, 2,
                2, 3,
                3, 0,

                4, 5,
                5, 6,
                6, 7,
                7, 4,

                0, 4,
                1, 5,
                2, 6,
                3, 7
            };

            vBuffer = new VertexBuffer(
                graphics,
                VertexPositionColor.VertexDeclaration,
                numVertices,
                BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColor>(vertices);

            iBuffer = new IndexBuffer(
                graphics,
                typeof(short),
                24,
                BufferUsage.WriteOnly);
            iBuffer.SetData<short>(indices);

            fxBasic = new BasicEffect(graphics);
            fxBasic.GraphicsDevice.SetVertexBuffer(vBuffer);
            fxBasic.GraphicsDevice.Indices = iBuffer;
        }

        public void Draw(OrbitingCamera cam)
        {
            fxBasic.Projection = cam.Projection;
            fxBasic.View = cam.View;
            fxBasic.World = Matrix.Identity;
            fxBasic.Techniques[0].Passes[0].Apply();

            fxBasic.GraphicsDevice.SetVertexBuffer(vBuffer);
            fxBasic.GraphicsDevice.Indices = iBuffer;
            fxBasic.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 8, 0, 12);
        }
    }
}
