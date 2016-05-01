using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    [Flags]
    enum ModifierKeys
    {
        None = 0,

        // order is important. 
        Control = 1,
        Alt = 2,
        Shift = 4,
    }
}
