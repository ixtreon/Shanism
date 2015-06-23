using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls
{
    class ChatProvider
    {
        const int KeyInitialDelay = 300;
        const int KeyRepeatDelay = 100;

        public Keys CurrentKey { get; private set; }

        public char CurrentChar { get; private set; }

        public int CurrentDelay { get; private set; }

        public event Action<Keys, char> CharPressed;

        public void Update(int msElapsed, IEnumerable<Keys> newKeys)
        {
            var newKey = newKeys.FirstOrDefault();

            //check if there is a new key down
            if (newKey != Keys.None)
            {
                //set it, and fire the press event. 
                CurrentKey = newKey;
                CurrentDelay = KeyInitialDelay;

                CurrentChar = getCharacter();
                if(CharPressed != null)
                    CharPressed(CurrentKey, CurrentChar);
                return;
            }

            //check if there was any key down before that
            if (CurrentKey == Keys.None)
                return;

            //check if the key is still down
            if(!KeyManager.IsDown(CurrentKey))
            {
                CurrentKey = Keys.None;
                return;
            }
            
            //repeat the previously held key
            CurrentDelay -= msElapsed;
            if (CurrentDelay <= 0)
            {
                CurrentDelay = KeyRepeatDelay;
                if (CharPressed != null)
                    CharPressed(CurrentKey, CurrentChar);
            }
        }

        /// <summary>
        /// Gets the current character corresponding to the <see cref="CurrentKey"/>
        /// </summary>
        /// <returns>The character that is pressed, or '\0' if no character was found. </returns>
        private char getCharacter()
        {
            if (CurrentKey == Keys.Space)
                return ' ';
            if (CurrentKey == Keys.Back)
                return '\b';


            char outChar = '\0';


            if (mapSequentialKeys(Keys.A, Keys.Z, 'a', 'A', ref outChar))
                return outChar;

            if (mapSequentialKeys(Keys.NumPad0, Keys.NumPad9, '0', '0', ref outChar))
                return outChar;


            if (mapOtherKeys(Keys.D0, Keys.D9, "0123456789", ")!@#$%^&*(", ref outChar))
                return outChar;

            if (mapOtherKeys(Keys.OemSemicolon, Keys.OemTilde, ";=,-./`", ":+<_>?~", ref outChar))
                return outChar;

            if (mapOtherKeys(Keys.OemOpenBrackets, Keys.OemQuotes, "[\\]'", "{|}\"", ref outChar))
                return outChar;


            Console.WriteLine("Unrecognized character: {0}", CurrentKey.ToString());
            return '\0';
        }


        private bool mapSequentialKeys(Keys from, Keys to, char cFrom, char cFromCaps, ref char c)
        {
            var inRange = CurrentKey >= from && CurrentKey <= to;
            if (inRange)
            {
                var id = CurrentKey - from;
                if (KeyManager.IsShiftDown)
                    c = (char)(cFromCaps + id);
                else
                    c = (char)(cFrom + id);
            }
            return inRange;
        }

        private bool mapOtherKeys(Keys from, Keys to, string sNormal, string sCaps, ref char c)
        {
            Debug.Assert(sNormal.Length == sCaps.Length);
            var inRange = CurrentKey >= from && CurrentKey <= to;
            if(inRange)
            {
                var id = CurrentKey - from;
                if (KeyManager.IsShiftDown)
                    c = sCaps[id];
                else
                    c = sNormal[id];
            }
            return inRange;
        }
    }
}
