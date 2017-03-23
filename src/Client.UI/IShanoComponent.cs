using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Input;

namespace Shanism.Client
{
    public interface IShanoComponent
    {

        /// <summary>
        /// Gets the current state of the keyboard.
        /// Part of the game input.
        /// </summary>
        KeyboardInfo Keyboard { get; }

        /// <summary>
        /// Gets the current state of the mouse.
        /// Part of the game input.
        /// </summary>
        MouseInfo Mouse { get; }

        /// <summary>
        /// Gets the current state of the game screen.
        /// Part of the game output.
        /// </summary>
        Screen Screen { get; }

        /// <summary>
        /// Gets the current collection of loaded game content.
        /// Part of the game resources.
        /// </summary>
        ContentList Content { get; }
    }
}
