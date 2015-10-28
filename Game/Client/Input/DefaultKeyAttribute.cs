using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls
{
    /// <summary>
    /// Used to annotate keybindings with their default keys. 
    /// </summary>
    internal class DefaultKeyAttribute : Attribute
    {
        public readonly Keys Key;
        public readonly ModifierKeys Modifier;
        public DefaultKeyAttribute(Keys k, ModifierKeys modKeys = ModifierKeys.None)
        {
            this.Key = k;
            this.Modifier = modKeys;
        }

        public static DefaultKeyAttribute Get(Keybind k)
        {
            var attrs = typeof(Keybind).GetMember(k.ToString())[0]
                .GetCustomAttributes(typeof(DefaultKeyAttribute), false);
            if (!attrs.Any())
                throw new Exception("No default key for that key dude!");

            var attr = attrs[0];
            return (DefaultKeyAttribute)attr;
        }
    }

    [Flags]
    internal enum ModifierKeys
    {
        None = 0,
        Control = 1,
        Shift = 2,
        Alt = 4,
    }
}
