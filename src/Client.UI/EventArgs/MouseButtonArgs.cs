using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    /// <summary>
    /// The event raised whenever a mouse button does something. Extends <see cref="MouseArgs"/>. 
    /// </summary>
    public class MouseButtonArgs : MouseArgs
    {
        public MouseButton Button { get; }


        public MouseButtonArgs(Control sender, Vector pos, MouseButton btn)
            : base(sender, pos)
        {
            Button = btn;
        }

    }


    public enum MouseButton
    {
        Left = 0,
        Right,
        Middle,
        Side1,
        Side2,
    }
}
