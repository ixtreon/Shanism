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
        float _repeatDelay;

        /// <summary>
        /// The event raised whenever a character is to be displayed. 
        /// </summary>
        public event Action<Keybind, char?> CharacterTyped;


        readonly KeyboardInfo keyboard;

        public KeyRepeater(KeyboardInfo keyboard)
        {
            this.keyboard = keyboard;
        }

        public void Update(float msElapsed)
        {
            //change the key if a new one is pressed
            var newlyPressedKey = keyboard.JustPressedKeys
                .FirstOrDefault(k => !k.IsModifier());
            if (newlyPressedKey != Keys.None)
            {
                var pressedKey = new Keybind(keyboard.Modifiers, newlyPressedKey);

                _repeatDelay = KeyInitialDelay;
                _keyCombo = pressedKey;
                _currentChar = CharMap.GetChar(_keyCombo.Key, _keyCombo.Modifiers.HasFlag(ModifierKeys.Shift));

                //fire the event once on button press
                CharacterTyped?.Invoke(_keyCombo, _currentChar);

                return;
            }

            //exit if key no longer held
            if (_keyCombo == Keybind.None || !keyboard.IsDown(_keyCombo))
            {
                _keyCombo = Keybind.None;
                return;
            }

            //repeat the previously held key
            _repeatDelay -= msElapsed;
            if (_repeatDelay <= 0)
            {
                _repeatDelay = KeyRepeatDelay;
                CharacterTyped?.Invoke(_keyCombo, _currentChar);
            }
        }
    }
}
