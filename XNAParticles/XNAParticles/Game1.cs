using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace XNAParticles
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont font;

        private Effect fxParticles;

        private OrbitingCamera cam;

        private FPSCounter fpsCounter;

        private TransparentBox box;
        private Particles particles;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            cam = new OrbitingCamera(GraphicsDevice.Viewport.AspectRatio);
            cam.Offset = Vector3.Backward * 34;

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load shaders
            fxParticles = Content.Load<Effect>("Shaders\\Particles");
            fxParticles.Parameters["Texture"].SetValue(Content.Load<Texture2D>("Textures\\Particle"));

            box = new TransparentBox(GraphicsDevice);

            // load font
            font = Content.Load<SpriteFont>("Fonts\\Debug");

            this.Components.Add(fpsCounter = new FPSCounter(this));


            int pCount = 0;
            // Particles
            particles = new Particles(fxParticles);
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        Color color = Color.Red;
                        if (pCount > 50 && pCount <= 100)
                            color = Color.Orange;
                        else if (pCount > 100 && pCount <= 150)
                            color = Color.Yellow;
                        else if (pCount > 150 && pCount <= 200)
                            color = Color.Green;
                        else if (pCount > 200 && pCount <= 250)
                            color = Color.CornflowerBlue;

                        particles.Add(new Particle(new Vector3(x * 0.5f, y * 0.5f, z * 0.5f), color));
                        pCount++;
                    }
                }
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                cam.Yaw -= 1 * dt;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                cam.Yaw += 1 * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                cam.Pitch -= 1 * dt;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                cam.Pitch += 1 * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                cam.Offset += Vector3.Forward * 5 * dt;
            else if (Keyboard.GetState().IsKeyDown(Keys.X))
                cam.Offset += Vector3.Backward * 5 * dt;

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.G))
                if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                    particles.Gravity += Vector3.Down * 0.1f;
                else
                    particles.Gravity -= Vector3.Down * 0.1f;

            if (keyState.IsKeyDown(Keys.A))
                if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                    particles.Attraction += 0.1f * dt;
                else
                    particles.Attraction -= 0.1f * dt;

            if (keyState.IsKeyDown(Keys.P))
                if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                    particles.Spring += 0.1f * dt;
                else
                    particles.Spring -= 0.1f * dt;

            if (keyState.IsKeyDown(Keys.S))
                if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                    particles.Shear += 0.1f * dt;
                else
                    particles.Shear -= 0.1f * dt;

            if (keyState.IsKeyDown(Keys.D))
                if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                    particles.Damping += 0.1f * dt;
                else
                    particles.Damping -= 0.1f * dt;


            cam.Update(dt);


            // Update particles
            particles.Update(dt);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;

            #region Box

            box.Draw(cam);

            #endregion

            #region Particles

            particles.Draw(cam.Projection, cam.View, cam.SideVector, cam.UpVector);

            #endregion

            #region GUI

            string text =
                "CONTROLS\n" +
                "ARROWS = Control camera rotation\n" +
                "Z/X = Zoom in and out\n\n" +
                "PARAMS" +
                "\nPARTICLE Radius  = " + particles.ParticleRadius +
                "\ng/G - GRAVITY    = " + particles.Gravity +
                "\nd/D - DAMPING    = " + particles.Damping +
                "\np/P - SPRING     = " + particles.Spring +
                "\ns/S - SHEAR      = " + particles.Shear +
                "\na/A - ATTRACTION = " + particles.Attraction;

            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, Vector2.One * 15, Color.White);
            spriteBatch.DrawString(font, "FPS: " + fpsCounter.FPS, new Vector2(GraphicsDevice.Viewport.Width * 0.90f, 15), Color.White);
            spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }
    }
}
