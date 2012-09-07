using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Particle
{
    private VertexPositionColorTexture[] vertices;
    private short[] indices;

    private Vector3 startPosition;
    private Vector3 startVelocity;

    public Vector3 Position;
    public Vector3 OldPosition;
    public Vector3 Velocity;

    public Particle(Vector3 position, Vector3 velocity, Color color)
    {
        this.Position = this.startPosition = position;
        this.Velocity = this.startVelocity = velocity;

        this.vertices = new VertexPositionColorTexture[4];
        this.vertices[0] = new VertexPositionColorTexture(position, color, new Vector2(0, 0));
        this.vertices[1] = new VertexPositionColorTexture(position, color, new Vector2(0, 1));
        this.vertices[2] = new VertexPositionColorTexture(position, color, new Vector2(1, 1));
        this.vertices[3] = new VertexPositionColorTexture(position, color, new Vector2(1, 0));

        indices = new short[6];
        indices[0] = 0;
        indices[1] = 3;
        indices[2] = 2;
        indices[3] = 2;
        indices[4] = 1;
        indices[5] = 0;
    }

    public void Draw(Effect effect)
    {
        effect.Parameters["Position"].SetValue(Position);
        effect.CurrentTechnique.Passes[0].Apply();
        effect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
            PrimitiveType.TriangleList, vertices, 0, vertices.Length,
            indices, 0, 2);
    }

    public void Reset()
    {
        this.Position = startPosition;
        this.Velocity = startVelocity;
    }
}
