using Shanism.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client
{
    /// <summary>
    /// The main game class that starts the client engine. 
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

        public IShanoClient Engine { get { return _clientEngine; } }

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
            ExitHelper.SetGame(this);
            Content.RootDirectory = "Content";


            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
            };

            //setup stuff
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Settings.Current.Saved += reloadGraphicsEngine;
            reloadGraphicsEngine();

            Screen.SetWindowSize(Window.ClientBounds.Size.ToPoint());
        }

        void recreateBackBuffer()
        {
            var s = Settings.Current.RenderSize;
            var w = (int)(Window.ClientBounds.Width * s);
            var h = (int)(Window.ClientBounds.Height * s);
            drawBuffer = new RenderTarget2D(GraphicsDevice, w, h);
        }

        void reloadGraphicsEngine()
        {
            graphics.SynchronizeWithVerticalRetrace = Settings.Current.VSync;
            graphics.IsFullScreen = false;

            recreateBackBuffer();
            graphics.ApplyChanges();
        }


        /// <summary>
        /// Changes the back buffer size in response to window resize. 
        /// </summary>
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (_stopResizeRecurse) return;
            _stopResizeRecurse = true;

            //update backbuffer size
            var sz = Window.ClientBounds;
            var renderScale = Settings.Current.RenderSize;
            if (sz != _windowSize && sz.Width > 0 && sz.Height > 0)
            {
                _windowSize = sz;

                graphics.PreferredBackBufferWidth = (int)(sz.Width);
                graphics.PreferredBackBufferHeight = (int)(sz.Height);
                graphics.ApplyChanges();

                recreateBackBuffer();
            }

            Screen.SetWindowSize(Window.ClientBounds.Size.ToPoint());

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
            _clientEngine.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if(!_isLoaded)
            {
                _isLoaded = true;
                GameLoaded?.Invoke();
            }

            _clientEngine.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void SetServer(IReceptor server)
        {
            _clientEngine.SetServer(server);
        }
    }
}
