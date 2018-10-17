using Microsoft.Xna.Framework.Input;
using Shanism.Client.IO;
using System;

namespace Shanism.Client.UI
{
    public class KeyboardArgs : EventArgs
    {
        /// <summary>
        /// Gets the keybind that triggered this event.
        /// </summary>
        public Keybind Keybind { get; }

        public char? RecognizedCharacter { get; }

        public ModifierKeys Modifiers => Keybind.Modifiers;

        public Keys Key => Keybind.Key;

        public KeyboardArgs(ModifierKeys modifiers, Keys key)
            : this(new Keybind(modifiers, key)) { }

        public KeyboardArgs(Keybind k)
        {
            Keybind = k;
            RecognizedCharacter = CharMap.Default.GetChar(k.Key, k.Shift);
        }
    }
}
