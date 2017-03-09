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
        public event Action<Keybind, char?> CharacterTyped;

        bool hasKey => _keyCombo != Keybind.None;

        readonly KeyboardInfo keyboard;

        public KeyRepeater(KeyboardInfo keyboard)
        {
            this.keyboard = keyboard;
        }


        public void Update(int msElapsed)
        {
            if(keyboard.JustPressedKeys.Any())
            {
                var pressedKey = new Keybind(keyboard.Modifiers, keyboard.JustPressedKeys.First());

                _repeatDelay = KeyInitialDelay;
                _keyCombo = pressedKey;
                _currentChar = KeyMap.GetChar(_keyCombo.Key, _keyCombo.Modifiers.HasFlag(ModifierKeys.Shift));

                // fire the event once on button press
                if(hasKey)
                    CharacterTyped?.Invoke(_keyCombo, _currentChar);
            }
            else if (_keyCombo == Keybind.None || !keyboard.IsDown(_keyCombo))
            {
                _keyCombo = Keybind.None;
                return;
            }

            //repeat the previously held char
            _repeatDelay -= msElapsed;
            if (_repeatDelay <= 0)
            {
                _repeatDelay = KeyRepeatDelay;
                CharacterTyped?.Invoke(_keyCombo, _currentChar);
            }
        }
    }
}
