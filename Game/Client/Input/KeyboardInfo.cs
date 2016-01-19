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

        static KeyboardState oldKeyboard;
        static KeyboardState newKeyboard;
        

        public static ChatProvider ChatProvider { get; private set; }

        /// <summary>
        /// Gets whether the Control key is down. 
        /// </summary>
        public static bool IsControlDown
        {
            get { return newKeyboard.IsKeyDown(Keys.LeftControl) || newKeyboard.IsKeyDown(Keys.RightControl); }
        }

        /// <summary>
        /// Gets whether the Alt key is down. 
        /// </summary>
        public static bool IsAltDown
        {
            get { return newKeyboard.IsKeyDown(Keys.LeftAlt) || newKeyboard.IsKeyDown(Keys.RightAlt); }
        }

        /// <summary>
        /// Gets whether the Shift key is down. 
        /// </summary>
        public static bool IsShiftDown
        {
            get { return newKeyboard.IsKeyDown(Keys.LeftShift) || newKeyboard.IsKeyDown(Keys.RightShift); }
        }

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
            oldKeyboard = newKeyboard;
            newKeyboard = Keyboard.GetState();

            //inform the chat provider


            var keysJustPressed = newKeyboard.GetPressedKeys().Except(oldKeyboard.GetPressedKeys());
            ChatProvider.Update(msElapsed, keysJustPressed);
        }

        /// <summary>
        /// Gets whether a key is down. 
        /// </summary>
        public static bool IsDown(Keys k)
        {
            return newKeyboard.IsKeyDown(k);
        }

        /// <summary>
        /// Gets whether a key was just activated. 
        /// The definition of activation is determined by <see cref="Settings.QuickButtonPress"/>. 
        /// </summary>
        public static bool IsActivated(Keys k)
        {
            if (ShanoSettings.Current.QuickButtonPress)
                return oldKeyboard.IsKeyUp(k) && newKeyboard.IsKeyDown(k);

            return oldKeyboard.IsKeyDown(k) && newKeyboard.IsKeyUp(k);
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
