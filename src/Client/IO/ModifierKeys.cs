using System;

namespace Shanism.Client.IO
{
    /// <summary>
    /// A combination of one or more of the Ctrl, Alt, Shift keys. 
    /// </summary>
    [Flags]
    public enum ModifierKeys : byte
    {
        None = 0,
        Control = 1,
        Alt = 2,
        Shift = 4,
    }

    public static class ModifierKeysExt
    {
        public static bool HasControl(this ModifierKeys k)
            => (k & ModifierKeys.Control) != 0;

        public static bool HasShift(this ModifierKeys k)
            => (k & ModifierKeys.Shift) != 0;

        public static bool HasAlt(this ModifierKeys k)
            => (k & ModifierKeys.Alt) != 0;
    }
}
