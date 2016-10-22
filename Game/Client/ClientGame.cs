using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client
{
    /// <summary>
    /// The entry game class that starts the <see cref="ClientEngine"/>. 
    /// Extends the <see cref="Microsoft.Xna.Framework.Game"/> class.
    /// </summary>
    class ClientGame : Game, IClientInstance
    {
        readonly IClientEngine _clientEngine;

        GraphicsDeviceManager graphics;

        RenderTarget2D drawBuffer;


        Microsoft.Xna.Framework.Rectangle _windowSize;
        bool _stopResizeRecurse;
        bool _isLoaded;


        #region IShanoClient implementation

        public IClientEngine Engine => _clientEngine;
        public event Action GameLoaded;

        #endregion


        public ClientGame(string playerName)
        {
            graphics = new GraphicsDeviceManager(this);
            _clientEngine = new ClientEngine(playerName, graphics, Content);
        }


        /// <summary>
        /// Run at the beginning. 
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = "ShanoRPG";
            GameHelper.SetGame(this);

            //setup MonoGame vars
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            reloadGraphicsEngine();


            Settings.Saved += reloadGraphicsEngine;

            Screen.SetWindowSize(Window.ClientBounds.Size.ToPoint());
        }

        void recreateDrawBuffer()
        {
            var s = Screen.RenderSize;
            var w = (int)(Window.ClientBounds.Width * s);
            var h = (int)(Window.ClientBounds.Height * s);
            drawBuffer = new RenderTarget2D(GraphicsDevice, w, h);
        }

        void reloadGraphicsEngine()
        {
            IsFixedTimeStep = Settings.Current.VSync;
            graphics.SynchronizeWithVerticalRetrace = Settings.Current.VSync;
            graphics.IsFullScreen = false;

            recreateDrawBuffer();
            graphics.ApplyChanges();
        }


        /// <summary>
        /// Changes the back buffer size in response to window resize. 
        /// </summary>
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (_stopResizeRecurse) return;

            _stopResizeRecurse = true;

            var sz = Window.ClientBounds;
            if (sz != _windowSize && sz.Width > 0 && sz.Height > 0)
            {
                _windowSize = sz;

                //drawbuffer
                recreateDrawBuffer();
                _clientEngine.SetWindowSize(sz.Size.ToPoint());

                //actual backbuffer
                graphics.PreferredBackBufferWidth = (int)(sz.Width);
                graphics.PreferredBackBufferHeight = (int)(sz.Height);
                graphics.ApplyChanges();
            }


            _stopResizeRecurse = false;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _clientEngine.LoadContent();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                GameLoaded?.Invoke();
            }

            _clientEngine.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (drawBuffer != null)
            {
                _clientEngine.DefaultRenderTarget = drawBuffer;
                _clientEngine.Draw(gameTime);

                GraphicsDevice.SetRenderTarget(null);
                using (var sb = new SpriteBatch(GraphicsDevice))
                {
                    sb.Begin();
                    var sz = Window.ClientBounds;
                    sb.Draw(drawBuffer,
                        new Rectangle(0, 0, sz.Width, sz.Height),
                        new Rectangle(0, 0, drawBuffer.Width, drawBuffer.Height),
                        Color.White);
                    sb.End();
                }
            }
            else
                _clientEngine.Draw(gameTime);


            base.Draw(gameTime);
        }
    }
}
