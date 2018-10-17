using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shanism.Client.Test
{
    class StandAloneMSAA : Game
    {
        Texture2D blank;

        SpriteBatch sb;
        RenderTarget2D target;

        public GraphicsDeviceManager Graphics { get; }

        float offset;
        bool useRenderTarget;

        public StandAloneMSAA()
        {
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true,
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            Graphics.ApplyChanges();
        }

        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            Graphics.PreferMultiSampling = true;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
            Graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            sb = new SpriteBatch(GraphicsDevice);

            target = new RenderTarget2D(GraphicsDevice, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight,
                false, SurfaceFormat.Color, DepthFormat.Depth24,
                8, RenderTargetUsage.DiscardContents);

        }

        protected override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.F1))
            {
                Graphics.PreferMultiSampling = true;
                Graphics.ApplyChanges();
            }

            if (kb.IsKeyDown(Keys.F2))
            {
                Graphics.PreferMultiSampling = false;
                Graphics.ApplyChanges();
            }

            if (kb.IsKeyDown(Keys.F5))
                useRenderTarget = true;

            if (kb.IsKeyDown(Keys.F6))
                useRenderTarget = false;

            offset = ((float)gameTime.TotalGameTime.TotalSeconds / 10) % 99.12789370f;

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            if (useRenderTarget)
                GraphicsDevice.SetRenderTarget(target);

            GraphicsDevice.Clear(Color.Black);

            sb.Begin(rasterizerState: new RasterizerState { MultiSampleAntiAlias = true });
            {
                for (float i = -100; i < Window.ClientBounds.Width; i += 4.672513f)
                {
                    sb.Draw(blank,
                        new Vector2(offset + i, 0),
                        null,
                        Color.White,
                        0, Vector2.Zero,
                        new Vector2(2.017823521f, Window.ClientBounds.Height),
                        SpriteEffects.None, 0);
                }
            }
            sb.End();

            if(useRenderTarget)
            {
                GraphicsDevice.SetRenderTarget(null);

                sb.Begin();
                sb.Draw(target, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), Color.White);
                sb.End();
            }

        }
    }
}
