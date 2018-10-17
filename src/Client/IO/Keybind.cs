using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;

namespace Shanism.Client.IO
{
    /// <summary>
    /// A keyboard gesture that consists of one main key and zero or more modifier keys. 
    /// </summary>
    public struct Keybind
    {
        public static readonly Keybind None = new Keybind(ModifierKeys.None, Keys.None);


        public ModifierKeys Modifiers { get; }

        public Keys Key { get; }


        public bool Control => Modifiers.HasControl();

        public bool Alt => Modifiers.HasAlt();

        public bool Shift => Modifiers.HasShift();


        public Keybind(Keys key)
            : this(ModifierKeys.None, key) { }

        [JsonConstructor]
        public Keybind(ModifierKeys modifiers, Keys key)
        {
            if (key.IsModifier())
                throw new InvalidOperationException($"Main key `{key}` is a modifier!");
            Modifiers = modifiers;
            Key = key;
        }

        public void Deconstruct(out ModifierKeys modifiers, out Keys key)
        {
            modifiers = Modifiers;
            key = Key;
        }


        public static bool operator ==(Keybind a, Keybind b) 
            => a.Key == b.Key && a.Modifiers == b.Modifiers;

        public static bool operator !=(Keybind a, Keybind b) 
            => !(a == b);

        //a key is always a keybind; the opposite's not always true
        public static explicit operator Keybind(Keys k) 
            => new Keybind(k);

        public override bool Equals(object obj) 
            => (obj is Keybind k) && k == this;

        public override string ToString()
        {
            if (Key == Keys.None)
                return "N/A";

            if (Modifiers != ModifierKeys.None)
                return $"{Modifiers}+{Key}";

            return Key.ToString();
        }

        public string ToShortString(string noneString = "")
        {
            if (Key == Keys.None)
                return noneString;

            var ans = "";
            if (Control) ans += "C";
            if (Alt) ans += "A";
            if (Shift) ans += "S";
            ans += CharMap.Default.GetChar(Key, false)?.ToString().ToUpper() ?? Key.ToString();
            return ans;
        }

        public override int GetHashCode() 
            => (((long)Key << 32) | (long)Modifiers).GetHashCode();

    }
}
