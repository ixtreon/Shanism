using IO;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    class ChatProvider
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
        /// Gets the current key that is being held. 
        /// </summary>
        public Keys CurrentKey { get; private set; }

        /// <summary>
        /// Gets the current character that is being repeated. 
        /// </summary>
        public char CurrentChar { get; private set; }

        /// <summary>
        /// The time left before the currently held character is repeated. 
        /// </summary>
        public int CurrentDelay { get; private set; }

        /// <summary>
        /// The event whenever a visible character is pressed. 
        /// </summary>
        public event Action<Keys, char> CharPressed;

        public void Update(int msElapsed, IEnumerable<Keys> newKeys)
        {
            var newKey = newKeys.FirstOrDefault();

            //check if there is a new key down
            if (newKey != Keys.None)
            {
                //replace the repated key
                CurrentKey = newKey;
                CurrentDelay = KeyInitialDelay;
                CurrentChar = getCharacter();

                // fire the press event
                CharPressed?.Invoke(CurrentKey, CurrentChar);
                return;
            }

            //if no new and no current key, there is nothing to be done
            if (CurrentKey == Keys.None)
                return;

            //if current key was released, 
            if(!KeyboardInfo.IsDown(CurrentKey))
            {
                CurrentKey = Keys.None;
                return;
            }
            
            //repeat the previously held key
            CurrentDelay -= msElapsed;
            if (CurrentDelay <= 0)
            {
                CurrentDelay = KeyRepeatDelay;
                CharPressed?.Invoke(CurrentKey, CurrentChar);
            }
        }

        /// <summary>
        /// Gets the current character corresponding to the <see cref="CurrentKey"/>
        /// </summary>
        /// <returns>The character that is pressed, or '\0' if no character was found. </returns>
        char getCharacter()
        {
            var c = KeyMap.GetChar(CurrentKey, KeyboardInfo.IsShiftDown) ?? '\0';
            if(c == '\0') Console.WriteLine("Unrecognized character: {0}", CurrentKey.ToString());

            return c;
        }

    }
}
