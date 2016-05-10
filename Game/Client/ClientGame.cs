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

            Content.RootDirectory = "Content";
            Window.Title = "ShanoRPG";

            ExitHelper.SetGame(this);

            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
            };

            //no vsync
            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
            graphics.ApplyChanges();


            //hook resizing, set mouse
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            _clientEngine.SetWindowSize(Window.ClientBounds.Size.ToPoint());
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
            if (sz != _windowSize && sz.Width > 0 && sz.Height > 0)
            {
                _windowSize = sz;
                graphics.PreferredBackBufferWidth = sz.Width;
                graphics.PreferredBackBufferHeight = sz.Height;
                graphics.ApplyChanges();

            }

            //inform the engine
            _clientEngine.SetWindowSize(Window.ClientBounds.Size.ToPoint());

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
                //Window_ClientSizeChanged(null, null);
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
