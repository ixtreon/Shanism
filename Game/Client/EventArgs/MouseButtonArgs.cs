using Shanism.Client.UI;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    /// <summary>
    /// The event raised whenever a mouse button does something. Extends <see cref="MouseArgs"/>. 
    /// </summary>
    class MouseButtonArgs : MouseArgs
    {
        public MouseButton Button { get; }


        public MouseButtonArgs(Control sender, Vector pos, MouseButton btn)
            : base(sender, pos)
        {
            Button = btn;
        }

    }


    enum MouseButton
    {
        Left = 0,
        Right,
        Middle,
        Side1,
        Side2,
    }
}
