using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    /// <summary>
    /// Keeps track of the game input (keyboard, mouse)
    /// and output (screen, resources).
    /// </summary>
    class GameComponent
    {
        /// <summary>
        /// Gets the current state of the keyboard.
        /// Part of the game input.
        /// </summary>
        public KeyboardInfo Keyboard { get; }

        /// <summary>
        /// Gets the current state of the mouse.
        /// Part of the game input.
        /// </summary>
        public MouseInfo Mouse { get; }

        /// <summary>
        /// Gets the current state of the game screen.
        /// Part of the game output.
        /// </summary>
        public Screen Screen { get; }

        /// <summary>
        /// Gets the current collection of loaded game content.
        /// Part of the game resources.
        /// </summary>
        public ContentList Content { get; }


        public GameComponent(GameComponent c)
        {
            Screen = c.Screen;
            Content = c.Content;

            Keyboard = c.Keyboard;
            Mouse = c.Mouse;
        }

        public GameComponent(GraphicsDevice device, ContentManager contentManager)
        {
            Screen = new Screen(device);
            Content = new ContentList(device, contentManager);

            Keyboard = new KeyboardInfo();
            Mouse = new MouseInfo(Screen);
        }

        public GraphicsDevice GraphicsDevice => Screen.GraphicsDevice;

        public void Update(int msElapsed, bool isActive)
        {
            Keyboard.Update(msElapsed, isActive);
            Mouse.Update(msElapsed, isActive);
        }

    }
}
