using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
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
}
