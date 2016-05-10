using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    /// <summary>
    /// A keybind that consists of a main key and zero or more modifier keys. 
    /// </summary>
    struct Keybind
    {
        public static readonly Keybind None = new Keybind(ModifierKeys.None, Keys.None);

        public ModifierKeys Modifiers { get; }

        public Keys Key { get; }

        public bool Control => (Modifiers & ModifierKeys.Control) != 0;

        public bool Alt => (Modifiers & ModifierKeys.Alt) != 0;

        public bool Shift => (Modifiers & ModifierKeys.Shift) != 0;

        public Keybind(Keys key)
        {
            Modifiers = ModifierKeys.None;
            Key = key;
        }

        [JsonConstructor]
        public Keybind(ModifierKeys modifiers, Keys key)
        {
            if (key.IsModifier())
                throw new InvalidOperationException($"Main key `{key}` is a modifier!");
            Modifiers = modifiers;
            Key = key;
        }

        public string ToShortString()
        {
            if (Key == Keys.None)
                return "N/A";

            var ans = "";
            if (Modifiers.HasFlag(ModifierKeys.Control))    ans += "C";
            if (Modifiers.HasFlag(ModifierKeys.Alt))        ans += "A";
            if (Modifiers.HasFlag(ModifierKeys.Shift))      ans += "S";
            ans += KeyMap.GetChar(Key, false)?.ToString().ToUpper() ?? Key.ToString();
            return ans;
        }

        public static bool operator ==(Keybind a, Keybind b)
        {
            return a.Key == b.Key && a.Modifiers == b.Modifiers;
        }

        public static bool operator !=(Keybind a, Keybind b)
        {
            return !(a == b);
        }

        //a key is always a keybind; the opposite's not always true
        public static implicit operator Keybind(Keys k)
        {
            return new Keybind(k);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Keybind))
                return false;
            return (Keybind)obj == this;
        }

        public override string ToString()
        {
            if (Key == Keys.None)
                return "N/A";

            if (Modifiers != ModifierKeys.None)
                return Modifiers.ToString() + "+" + Key.ToString();
            return Key.ToString();
        }

        public override int GetHashCode()
        {
            return (((long)Key << 32) | (long)Modifiers).GetHashCode();
        }

    }
}
