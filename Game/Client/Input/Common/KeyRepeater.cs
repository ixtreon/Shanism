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
        /// The current keybind that is being held. 
        /// </summary>
        Keybind _keyCombo;

        /// <summary>
        /// The current character that is being repeated. 
        /// </summary>
        char? _currentChar;

        /// <summary>
        /// The time left before the currently held character is repeated again. 
        /// </summary>
        int _repeatDelay;

        /// <summary>
        /// The event raised whenever a character is to be displayed. 
        /// </summary>
        public event Action<Keybind, char?> KeyRepeated;

        bool hasChar => _currentChar.HasValue;


        public void Update(int msElapsed)
        {
            if (!hasChar)
                return;

            //repeat the previously held char
            _repeatDelay -= msElapsed;
            if (_repeatDelay <= 0)
            {
                _repeatDelay = KeyRepeatDelay;
                KeyRepeated?.Invoke(_keyCombo, _currentChar);
            }
        }

        public void SetKey(Keybind k)
        {
            _repeatDelay = KeyInitialDelay;
            _keyCombo = k;
            _currentChar = KeyMap.GetChar(_keyCombo.Key, _keyCombo.Modifiers.HasFlag(ModifierKeys.Shift));

            // fire the event once on button press
            if (hasChar)
                KeyRepeated?.Invoke(_keyCombo, _currentChar);
        }
    }
}
