using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Common;

namespace Shanism.Client
{
    public class ShanoGame : Game, IShanoComponent
    {
        bool _isLoaded;
        UiScreen _currentScreen;
        protected GraphicsDeviceManager graphics { get; }

        public KeyboardInfo Keyboard { get; private set; }
        public MouseInfo Mouse { get; private set; }
        public Screen Screen { get; private set; }
        public new ContentList Content { get; private set; }

        protected UiScreen CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                if (_currentScreen != value)
                {
                    _currentScreen = value;
                    _currentScreen.Shown();
                }
            }
        }

        /// <summary>
        /// Occurs when the game has finished loading.
        /// </summary>
        public event Action FinishLoading;

        public ShanoGame()
        {
            graphics = new GraphicsDeviceManager(this);

            MaxElapsedTime = TimeSpan.FromMilliseconds(50);

            Control.SetContext(this);
        }

        protected override void Initialize()
        {
            Console.WriteLine("Initializing...");

            Content = new ContentList(GraphicsDevice, base.Content);
            Screen = new Screen(GraphicsDevice);
            Keyboard = new KeyboardInfo();
            Mouse = new MouseInfo(Screen);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("Loading content...");

            if (!Content.LoadDefault())
            {
                Console.WriteLine("Unable to load game content. ");
                return;
            }

            base.LoadContent();
        }

        float msSinceUpdate = 0;

        protected override void Update(GameTime gameTime)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                FinishLoading?.Invoke();
            }

            var msElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            msSinceUpdate += msElapsed;

            const int fps = 30;
            const float sleepTime = 1000f / fps;

            if (msSinceUpdate < sleepTime)
                return;

            msSinceUpdate = 0;

            refreshScreenSize();
            Keyboard.Update((int)sleepTime, IsActive);
            Mouse.Update(IsActive);
            CurrentScreen?.Update((int)sleepTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            CurrentScreen?.Draw();
            base.Draw(gameTime);
        }

        void refreshScreenSize()
        {
            var sz = Window.ClientBounds.Size;

            if (sz.X == Screen.Size.X && sz.Y == Screen.Size.Y)
                return;

            if (sz.X <= 0 || sz.Y <= 0)
                return;

            //drawbuffer
            Screen.SetWindowSize(sz.ToPoint());

            //actual backbuffer
            graphics.PreferredBackBufferWidth = sz.X;
            graphics.PreferredBackBufferHeight = sz.Y;
            graphics.ApplyChanges();
        }
    }
}
