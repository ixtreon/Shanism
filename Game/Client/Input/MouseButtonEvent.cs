using Client.UI;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    /// <summary>
    /// The event raised whenever a mouse button does something. Extends <see cref="MouseEvent"/>. 
    /// </summary>
    class MouseButtonEvent : MouseEvent
    {
        public MouseButton Button { get; }


        public MouseButtonEvent(Control sender, Vector pos, MouseButton btn)
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
