using IO;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    /// <summary>
    /// Contains the past and current keyboard data, along with a chat provider that helps decypher that info. 
    /// </summary>
    static class KeyboardInfo
    {

        static HashSet<Keys> oldKeysDown = new HashSet<Keys>();
        static HashSet<Keys> newKeysDown = new HashSet<Keys>();

        

        public static ChatProvider ChatProvider { get; private set; }


        public static IEnumerable<Keys> JustPressedKeys { get; private set; }

        public static IEnumerable<Keys> JustReleasedKeys { get; private set; }

        public static IEnumerable<GameAction> JustActivatedActions { get; private set; }

        //static IEnumerable<GameAction> JustReleasedActions { get; private set; }

        /// <summary>
        /// Gets whether the Control key is down. 
        /// </summary>
        public static bool IsControlDown => oldKeysDown.Contains(Keys.LeftControl) || newKeysDown.Contains(Keys.RightControl);

        /// <summary>
        /// Gets whether the Alt key is down. 
        /// </summary>
        public static bool IsAltDown => oldKeysDown.Contains(Keys.LeftAlt) || newKeysDown.Contains(Keys.RightAlt);

        /// <summary>
        /// Gets whether the Shift key is down. 
        /// </summary>
        public static bool IsShiftDown => oldKeysDown.Contains(Keys.LeftShift) || newKeysDown.Contains(Keys.RightShift);



        static KeyboardInfo()
        {
            ChatProvider = new ChatProvider();
        }

        /// <summary>
        /// Updates the available keyboard and chat info. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public static void Update(int msElapsed)
        {
            oldKeysDown = newKeysDown;
            newKeysDown = new HashSet<Keys>(Keyboard.GetState().GetPressedKeys());

            JustPressedKeys = newKeysDown.Except(oldKeysDown).ToList();
            JustReleasedKeys = oldKeysDown.Except(newKeysDown).ToList();

            JustActivatedActions = Enum<GameAction>.Values
                .Where(isJustActivated).ToList();

            //inform the chat provider
            ChatProvider.Update(msElapsed, JustPressedKeys);
        }


        static bool isJustActivated(GameAction ga)
        {
            var kb = ShanoSettings.Current.Keybinds[ga];
            if (!checkModifiers(kb.Modifiers))
                return false;

            var k = kb.Key;
            if (ShanoSettings.Current.QuickButtonPress)
                return !oldKeysDown.Contains(k) && newKeysDown.Contains(k);
            return oldKeysDown.Contains(k) && !newKeysDown.Contains(k);
        }

        /// <summary>
        /// Gets whether a key is down. 
        /// </summary>
        public static bool IsDown(Keys k)
        {
            return newKeysDown.Contains(k);
        }

        /// <summary>
        /// Gets whether a key was just activated. 
        /// The definition of activation is determined by <see cref="Settings.QuickButtonPress"/>. 
        /// </summary>
        public static bool IsActivated(Keys k)
        {
            if (ShanoSettings.Current.QuickButtonPress)
                return !oldKeysDown.Contains(k) && newKeysDown.Contains(k);

            return oldKeysDown.Contains(k) && !newKeysDown.Contains(k);
        }


        /// <summary>
        /// Gets whether the key for the given game action is down. 
        /// </summary>
        /// <param name="a">The game action whose key to check. </param>
        /// <returns>Whether the key is currently down. </returns>
        public static bool IsDown(GameAction a)
        {
            var kb = ShanoSettings.Current.Keybinds[a];
            return checkModifiers(kb.Modifiers) && IsDown(kb.Key);    //TODO: fix for modifiers
        }

        public static bool IsActivated(GameAction a)
        {
            var k = ShanoSettings.Current.Keybinds[a];
            return IsActivated(k);
        }

        /// <summary>
        /// Gets whether a keybind was just activated. 
        /// The definition of activation is determined by <see cref="Settings.QuickButtonPress"/>. 
        /// </summary>
        public static bool IsActivated(Keybind kb)
        {
            return checkModifiers(kb.Modifiers) && IsActivated(kb.Key);
        }

        static bool checkModifiers(ModifierKeys mods)
        {
            if (mods == ModifierKeys.None)
                return true;

            var isCtrlOk = !mods.HasFlag(ModifierKeys.Control) || IsControlDown;
            var isAltOk = !mods.HasFlag(ModifierKeys.Alt) || IsAltDown;
            var isShiftOk = !mods.HasFlag(ModifierKeys.Shift) || IsShiftDown;

            return isCtrlOk && isAltOk && isShiftOk;
        }

    }
}
