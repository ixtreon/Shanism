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

            //check if there was any key down before that
            if (CurrentKey == Keys.None)
                return;

            //check if the current key is still down
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
                CharPressed?.Invoke(CurrentKey, CurrentChar);
            }
        }

        /// <summary>
        /// Gets the current character corresponding to the <see cref="CurrentKey"/>
        /// </summary>
        /// <returns>The character that is pressed, or '\0' if no character was found. </returns>
        private char getCharacter()
        {
            char keyChar;
            if (KeyManager.IsShiftDown)
            {
                if (capsCharDict.TryGetValue(CurrentKey, out keyChar))
                    return keyChar;
            }
            else
            {
                if (normalCharDict.TryGetValue(CurrentKey, out keyChar))
                    return keyChar;
            }

            Console.WriteLine("Unrecognized character: {0}", CurrentKey.ToString());
            return '\0';
        }

        static readonly Dictionary<Keys, char> normalCharDict = new Dictionary<Keys, char>();
        static readonly Dictionary<Keys, char> capsCharDict = new Dictionary<Keys, char>();

        static ChatProvider()
        {
            addSimpleMap(Keys.Space, ' ');
            addSimpleMap(Keys.Back, '\b');


            addSequentialMap(Keys.A, Keys.Z, 'a', 'A');
            addSequentialMap(Keys.NumPad0, Keys.NumPad9, '0', '0');


            addCustomMap(Keys.D0, Keys.D9, "0123456789", ")!@#$%^&*(");
            addCustomMap(Keys.OemSemicolon, Keys.OemTilde, ";=,-./`", ":+<_>?~");
            addCustomMap(Keys.OemOpenBrackets, Keys.OemQuotes, "[\\]'", "{|}\"");
        }

        static void addSimpleMap(Keys k, char c)
        {
            normalCharDict[k] = c;
            capsCharDict[k] = c;
        }

        static void addSequentialMap(Keys from, Keys to, char cFrom, char cFromCaps)
        {
            for(var i = from; i <= to; i++)
            {
                normalCharDict[i] = (char)(i - from + cFrom);
                capsCharDict[i] = (char)(i - from + cFromCaps);
            }
        }

        static void addCustomMap(Keys from, Keys to, string sNormal, string sCaps)
        {
            Debug.Assert(sNormal.Length == sCaps.Length);

            for (var i = from; i <= to; i++)
            {
                normalCharDict[i] = sNormal[i - from];
                capsCharDict[i] = sCaps[i - from];
            }
        }
    }
}
