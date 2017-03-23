using Shanism.Common;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Shanism.Client.Input
{
    /// <summary>
    /// Contains the past and current keyboard data, along with a chat provider that helps decypher that info. 
    /// </summary>
    public class KeyboardInfo
    {
        readonly KeyRepeater keyRepeater;

        IReadOnlyDictionary<ClientAction, Keybind> keybinds;


        HashSet<Keys> oldKeysDown = new HashSet<Keys>();
        HashSet<Keys> newKeysDown = new HashSet<Keys>();


        public bool QuickButtonPress { get; set; } = true;

        public IEnumerable<Keys> JustPressedKeys { get; private set; } = new List<Keys>();

        public IEnumerable<Keys> JustReleasedKeys { get; private set; } = new List<Keys>();

        public IEnumerable<ClientAction> JustActivatedActions { get; private set; } = new List<ClientAction>();

        public ModifierKeys Modifiers { get; private set; }


        public event Action<Keybind, char?> CharacterTyped;


        public KeyboardInfo()
        {
            keyRepeater = new KeyRepeater(this);
            keyRepeater.CharacterTyped += (k, c) => CharacterTyped?.Invoke(k, c);
        }

        public void SetKeybinds(IReadOnlyDictionary<ClientAction, Keybind> keybinds)
        {
            this.keybinds = keybinds;
        }


        /// <summary>
        /// Updates the available keyboard and chat info. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public void Update(float msElapsed, bool isActive)
        {
            if(!isActive)
            {
                JustActivatedActions = Enumerable.Empty<ClientAction>();
                return;
            }


            Modifiers = (isShiftDown() ? ModifierKeys.Shift : ModifierKeys.None)
                | (isControlDown() ? ModifierKeys.Control : ModifierKeys.None)
                | (isAltDown() ? ModifierKeys.Alt : ModifierKeys.None);


            oldKeysDown = newKeysDown;
            newKeysDown = new HashSet<Keys>(Keyboard.GetState().GetPressedKeys());

            JustPressedKeys = newKeysDown.Except(oldKeysDown).ToList();
            JustReleasedKeys = oldKeysDown.Except(newKeysDown).ToList();

            JustActivatedActions = keybinds?
                .Where(kvp => IsActivated(kvp.Value))
                .Select(kvp => kvp.Key)
                .ToList() ?? Enumerable.Empty<ClientAction>();

            //key repeater
            keyRepeater.Update(msElapsed);
        }

        bool isControlDown()
            => newKeysDown.Contains(Keys.LeftControl) || newKeysDown.Contains(Keys.RightControl);

        bool isAltDown()
            => newKeysDown.Contains(Keys.LeftAlt) || newKeysDown.Contains(Keys.RightAlt);

        bool isShiftDown()
            => newKeysDown.Contains(Keys.LeftShift) || newKeysDown.Contains(Keys.RightShift);

        /// <summary>
        /// Gets whether a key is down. 
        /// </summary>
        bool IsDown(Keys k)
            => newKeysDown.Contains(k);

        /// <summary>
        /// Gets whether a key was just activated. 
        /// The definition of activation is determined by <see cref="QuickButtonPress"/>. 
        /// </summary>
        bool IsActivated(Keys k)
        {
            if(QuickButtonPress)
                return !oldKeysDown.Contains(k) && newKeysDown.Contains(k);

            return oldKeysDown.Contains(k) && !newKeysDown.Contains(k);
        }


        /// <summary>
        /// Gets whether the key for the given game action is down. 
        /// </summary>
        /// <param name="a">The game action whose key to check. </param>
        /// <returns>Whether the key is currently down. </returns>
        public bool IsDown(ClientAction a)
        {
            Keybind k;
            if(keybinds == null || !keybinds.TryGetValue(a, out k))
                return false;
            return IsDown(k);
        }


        public bool IsDown(Keybind kb)
            => checkModifiers(kb.Modifiers) && IsDown(kb.Key);


        /// <summary>
        /// Gets whether a keybind was just activated. 
        /// The definition of activation is determined by <see cref="QuickButtonPress"/>. 
        /// </summary>
        public bool IsActivated(Keybind kb)
            => checkModifiers(kb.Modifiers) && IsActivated(kb.Key);

        bool checkModifiers(ModifierKeys mods)
            => (~Modifiers & mods) == 0;
    }
}
