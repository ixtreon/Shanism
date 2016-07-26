using Shanism.Common;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    class KeyRepeater
    {
        /// <summary>
        /// The initial delay, in milliseconds, a key must be held in order for the repeat sequence to start. 
        /// </summary>
        const int KeyInitialDelay = 300;

        /// <summary>
        /// The repeat delay, in milliseconds, a key must be held in order for a character to be repeated. 
        /// </summary>
        const int KeyRepeatDelay = 100;


        /// <summary>
        /// Gets or sets the current key that is being held. 
        /// </summary>
        public Keybind CurrentKey { get; private set; }


        /// <summary>
        /// Gets the current character that is being repeated. 
        /// </summary>
        public char? CurrentChar { get; private set; }

        /// <summary>
        /// The time left before the currently held character is repeated again. 
        /// </summary>
        int CurrentDelay { get; set; }

        /// <summary>
        /// The event raised whenever a character is to be displayed. 
        /// </summary>
        public event Action<Keybind, char?> KeyRepeated;

        public void Update(int msElapsed)
        {
            //if current key was released, set it to none
            if (!KeyboardInfo.IsDown(CurrentKey))
                CurrentKey = Keybind.None;

            //if no key, nothing can be done
            if (CurrentKey == Keys.None)
                return;

            //repeat the previously held key
            CurrentDelay -= msElapsed;
            if (CurrentDelay <= 0)
            {
                CurrentDelay = KeyRepeatDelay;
                KeyRepeated?.Invoke(CurrentKey, CurrentChar);
            }
        }

        public void SetKey(Keybind k)
        {
            CurrentKey = k;
            CurrentDelay = KeyInitialDelay;
            CurrentChar = getChar();

            // fire the event on button press
            if (CurrentKey != Keybind.None)
                KeyRepeated?.Invoke(CurrentKey, CurrentChar);
        }

        char? getChar(Keybind k)
        {
            return KeyMap.GetChar(k.Key, k.Modifiers.HasFlag(ModifierKeys.Shift));
        }


        /// <summary>
        /// Gets the current character corresponding to the <see cref="CurrentKey"/>
        /// </summary>
        /// <returns>The character that is pressed, or '\0' if no character was found. </returns>
        char? getChar()
            => getChar(CurrentKey);
    }
}
