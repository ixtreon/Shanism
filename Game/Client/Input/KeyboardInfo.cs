using Shanism.Common;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    /// <summary>
    /// Contains the past and current keyboard data, along with a chat provider that helps decypher that info. 
    /// </summary>
    static class KeyboardInfo
    {

        static HashSet<Keys> oldKeysDown = new HashSet<Keys>();
        static HashSet<Keys> newKeysDown = new HashSet<Keys>();


        public static IEnumerable<Keys> JustPressedKeys { get; private set; }

        public static IEnumerable<Keys> JustReleasedKeys { get; private set; }

        public static IEnumerable<GameAction> JustActivatedActions { get; private set; }

        public static ModifierKeys Modifiers { get; private set; }




        /// <summary>
        /// Updates the available keyboard and chat info. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public static void Update(int msElapsed)
        {
            oldKeysDown = newKeysDown;
            newKeysDown = new HashSet<Keys>(Keyboard.GetState().GetPressedKeys());

            Modifiers = (isShiftDown() ? ModifierKeys.Shift : ModifierKeys.None)
                | (isControlDown() ? ModifierKeys.Control : ModifierKeys.None)
                | (isAltDown() ? ModifierKeys.Alt : ModifierKeys.None);

            JustPressedKeys = newKeysDown.Except(oldKeysDown).ToList();
            JustReleasedKeys = oldKeysDown.Except(newKeysDown).ToList();

            JustActivatedActions = Settings.Current.Keybinds.BoundActions
                .Where(kvp => IsActivated(kvp.Value))
                .Select(kvp => kvp.Key)
                .ToList();
        }

        static bool isControlDown()
            => newKeysDown.Contains(Keys.LeftControl) || newKeysDown.Contains(Keys.RightControl);

        static bool isAltDown()
            => newKeysDown.Contains(Keys.LeftAlt) || newKeysDown.Contains(Keys.RightAlt);

        static bool isShiftDown()
            => newKeysDown.Contains(Keys.LeftShift) || newKeysDown.Contains(Keys.RightShift);

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
            if (Settings.Current.QuickButtonPress)
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
            var kb = Settings.Current.Keybinds[a];
            return checkModifiers(kb.Modifiers) && IsDown(kb.Key);    //TODO: fix for modifiers
        }

        public static bool IsDown(Keybind kb)
        {
            return checkModifiers(kb.Modifiers) && IsDown(kb.Key);
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
            return (~Modifiers & mods) == 0;
        }

    }
}
