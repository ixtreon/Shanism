using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Editor.Screens;

namespace Shanism.Editor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class EditorGame : ShanoGame
    {
        public EditorGame()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            CurrentScreen = new MainScreen(this);

        }
    }
}
