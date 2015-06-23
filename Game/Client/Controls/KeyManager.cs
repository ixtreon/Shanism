using Microsoft.Xna.Framework.Input;
using Client.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls
{
    /// <summary>
    /// Contains the current game keybindings, links to a chat provider. 
    /// </summary>
    static class KeyManager
    {
        static readonly DefaultKeyAttribute[] Keybinds = new DefaultKeyAttribute[Enum.GetValues(typeof(Keybind)).Length];

        static KeyboardState oldKb, newKb;


        public static ChatProvider ChatProvider { get; private set; }

        /// <summary>
        /// Gets whether the Control key is down. 
        /// </summary>
        public static bool IsControlDown
        {
            get { return newKb.IsKeyDown(Keys.LeftControl) || newKb.IsKeyDown(Keys.RightControl); }
        }

        /// <summary>
        /// Gets whether the Alt key is down. 
        /// </summary>
        public static bool IsAltDown
        {
            get { return newKb.IsKeyDown(Keys.LeftAlt) || newKb.IsKeyDown(Keys.RightAlt); }
        }

        /// <summary>
        /// Gets whether the Shift key is down. 
        /// </summary>
        public static bool IsShiftDown
        {
            get { return newKb.IsKeyDown(Keys.LeftShift) || newKb.IsKeyDown(Keys.RightShift); }
        }

        static KeyManager()
        {
            ChatProvider = new ChatProvider();
            var keys = Settings.Default.Keybindings.Split('\n');
            int keyData;
            for (int i = 0; i < Keybinds.Length; i++)
                if(i < keys.Length && int.TryParse(keys[i].Trim(), out keyData))
                    Keybinds[i] = new DefaultKeyAttribute((Keys)keyData);
                else
                    Keybinds[i] = DefaultKeyAttribute.Get((Keybind)i);

        }

        public static void Update(int msElapsed)
        {
            oldKb = newKb;
            newKb = Keyboard.GetState();

            var keysJustPressed = newKb.GetPressedKeys().Except(oldKb.GetPressedKeys());
            ChatProvider.Update(msElapsed, keysJustPressed);
        }

        public static void Save()
        {
            Settings.Default.Keybindings = "";
            Settings.Default.Save();
        }

        public static Keys GetKey(Keybind k)
        {
            return Keybinds[(int)k].Key;
        }

        public static bool IsDown(Keys k)
        {
            return newKb.IsKeyDown(k);
        }

        public static bool IsDown(Keybind kb)
        {
            var k = Keybinds[(int)kb];

            if (k == null)
                return false;

            var modsOk =
                (!k.Modifier.HasFlag(ModifierKeys.Control) || IsControlDown)
                && (!k.Modifier.HasFlag(ModifierKeys.Alt) || IsAltDown)
                && (!k.Modifier.HasFlag(ModifierKeys.Shift) || IsShiftDown);

            if (!modsOk)
                return false;

            return IsDown(k.Key);
        }

        public static bool IsActivated(Keys k)
        {
            if (Settings.Default.QuickButtonPress)
                return oldKb.IsKeyUp(k) && newKb.IsKeyDown(k);

            return oldKb.IsKeyDown(k) && newKb.IsKeyUp(k);
        }

        public static bool IsActivated(Keybind kb)
        {
            var k = Keybinds[(int)kb];
         
            if (k == null)
                return false;

            //if quick press activate on keydown, otherwise on keyup
            var modsOk =
                (!k.Modifier.HasFlag(ModifierKeys.Control) || newKb.IsKeyDown(Keys.LeftControl) || newKb.IsKeyDown(Keys.RightControl))
                && (!k.Modifier.HasFlag(ModifierKeys.Alt) || newKb.IsKeyDown(Keys.LeftAlt) || newKb.IsKeyDown(Keys.RightAlt))
                && (!k.Modifier.HasFlag(ModifierKeys.Shift) || newKb.IsKeyDown(Keys.LeftShift) || newKb.IsKeyDown(Keys.RightShift));

            if (!modsOk)
                return false;
            return IsActivated(k.Key);
        }

        /// <summary>
        /// Returns the collection of keybinds activated (press or release as defined by the user)
        /// given the current and past keyboard state. 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Keybind> KeybindsActivated(KeyboardState old, KeyboardState now)
        {
            return Enumerable.Range(0, Keybinds.Length)
                .Cast<Keybind>()
                .Where(i => IsActivated(i));
        }
    }
}
