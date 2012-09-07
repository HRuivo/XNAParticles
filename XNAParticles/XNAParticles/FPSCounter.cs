using Microsoft.Xna.Framework;

public sealed class FPSCounter : DrawableGameComponent
{
    private int totalFrames = 0;
    private float elapsedTime = 0.0f;

    public int FPS { get; private set; }


    public FPSCounter(Game game) : base(game)
    {
        FPS = 0;
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        elapsedTime += dt;

        if (elapsedTime >= 1.0f)
        {
            FPS = totalFrames;
            totalFrames = 0;
            elapsedTime = 0.0f;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        totalFrames++;
    }
}
