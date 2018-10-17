using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Shanism.Client.IO
{
    /// <summary>
    /// Contains the past and current keyboard data, along with a chat provider that helps decypher that info. 
    /// </summary>
    public class KeyboardSystem : ISystem
    {
        public const int KeysCount = 256;

        /// <summary>
        /// The delay in milliseconds
        /// a key must be held for,
        /// in order for the first repeat to occur. 
        /// </summary>
        public const int KeyRepeatInitialDelay = 300;

        /// <summary>
        /// The delay in milliseconds
        /// a key must be held for,
        /// in order for a character to be repeated. 
        /// </summary>
        public const int KeyRepeatConsecutiveDelay = 100;


        readonly IActivatable window;


        readonly Dictionary<ClientAction, Keybind> keybinds = new Dictionary<ClientAction, Keybind>();

        // keys n actions
        readonly List<Keybind> justPressed = new List<Keybind>();
        readonly List<Keybind> justReleased = new List<Keybind>();
        readonly HashSet<ClientAction> justActivated = new HashSet<ClientAction>();


        readonly bool[] oldKeys = new bool[KeysCount];
        readonly bool[] curKeys = new bool[KeysCount];
        ModifierKeys modKeys;
        Keybind? charTyped;

        // repeated chars
        Keybind repeatedKeybind;
        float repeatDelay;

        public bool QuickButtonPress { get; set; } = true;

        public IReadOnlyList<bool> LastState => oldKeys;
        public IReadOnlyList<bool> CurrentState => curKeys;

        public IReadOnlyList<Keybind> JustPressed => justPressed;
        public IReadOnlyList<Keybind> JustReleased => justReleased;
        public IReadOnlyCollection<ClientAction> JustActivated => justActivated;

        /// <summary>
        /// Gets the character typed this frame, if any.
        /// </summary>
        public Keybind? JustTypedCharacter => charTyped;

        //public IEnumerable<ClientAction> JustActivatedActions => justActivated;

        public KeyboardSystem(IActivatable window)
        {
            resetKeybinds();
            this.window = window;
        }

        public void SetKeybinds(IReadOnlyDictionary<ClientAction, Keybind> userKeybinds)
        {
            resetKeybinds();

            foreach (var kb in userKeybinds)
                keybinds[kb.Key] = kb.Value;

            // even if user-keys says otherwise, escape hides stuff
            keybinds[ClientAction.HideMenus] = new Keybind(Keys.Escape);
        }

        /// <summary>
        /// Updates the available keyboard and chat info. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public void Update(int msElapsed)
        {
            justActivated.Clear();

            if (!window.IsActive)
                return;

            //re-fetch the keys that are down
            Array.Copy(curKeys, oldKeys, KeysCount);
            Array.Clear(curKeys, 0, KeysCount);
            var keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            foreach (var k in keyboardState.GetPressedKeys())
                curKeys[(int)k] = true;


            //modifiers
            modKeys
                = ifEitherThen(Keys.LeftControl, Keys.RightControl, ModifierKeys.Control)
                | ifEitherThen(Keys.LeftAlt, Keys.RightAlt, ModifierKeys.Alt)
                | ifEitherThen(Keys.LeftShift, Keys.RightShift, ModifierKeys.Shift);

            // key press/release
            justPressed.Clear();
            justReleased.Clear();
            for (var k = 0; k < KeysCount; k++)
            {
                if(((Keys)k).IsModifier())
                    continue;

                if(oldKeys[k] && !curKeys[k])
                    justReleased.Add(new Keybind(modKeys, (Keys)k));
                else if(!oldKeys[k] && curKeys[k])
                    justPressed.Add(new Keybind(modKeys, (Keys)k));
            }

            // character input
            repeatChars(msElapsed);

            // actions
            updateActions();
        }

        void resetKeybinds()
        {
            keybinds.Clear();
            keybinds[ClientAction.ReloadUi] = new Keybind(ModifierKeys.Control | ModifierKeys.Shift, Keys.R);
            keybinds[ClientAction.HideMenus] = new Keybind(Keys.Escape);
        }

        void updateActions()
        {
            if (keybinds == null)
                return;

            foreach (var kvp in keybinds)
                if (isActivated(kvp.Value))
                    justActivated.Add(kvp.Key);
        }

        void repeatChars(int msElapsed)
        {
            if (justPressed.Any())
            {
                // new key down
                repeatedKeybind = justPressed[0];
                repeatDelay = KeyRepeatInitialDelay;
                charTyped = repeatedKeybind;
            }
            else if (!isDown(repeatedKeybind))
            {
                // old key up
                repeatedKeybind = Keybind.None;
                charTyped = null;
            }
            else if ((repeatDelay -= msElapsed) <= 0)
            {
                // repeat key
                repeatDelay += KeyRepeatConsecutiveDelay;
                charTyped = repeatedKeybind;
            }
            else
                charTyped = null;
        }

        ModifierKeys ifEitherThen(Keys inputA, Keys inputB, ModifierKeys output)
            => (curKeys[(int)inputA] || curKeys[(int)inputB]) ? output : ModifierKeys.None;


        /// <summary>
        /// Gets whether the key for the given game action is down. 
        /// </summary>
        /// <param name="a">The game action whose key to check. </param>
        /// <returns>Whether the key is currently down. </returns>
        public bool IsDown(ClientAction a)
            => keybinds.TryGetValue(a, out var kb)
            && isDown(kb);


        bool isDown(Keybind kb)
        {
            return (modKeys & kb.Modifiers) == kb.Modifiers
                && curKeys[(int)kb.Key];
        }


        /// <summary>
        /// Gets whether a keybind was just activated. 
        /// The definition of activation is determined by <see cref="QuickButtonPress"/>. 
        /// </summary>
        bool isActivated(Keybind kb)
        {
            var key = (int)kb.Key;
            return (modKeys & kb.Modifiers) == kb.Modifiers 
                && oldKeys[key] != curKeys[key]
                && curKeys[key] == QuickButtonPress;
        }
    }
}
