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
    /// The event raised whenever the mouse does something. 
    /// </summary>
    class MouseArgs : EventArgs
    {
        /// <summary>
        /// The control that triggered the event. 
        /// </summary>
        public Control Control { get; }

        /// <summary>
        /// The position of the mouse cursor relative to the triggering control. 
        /// </summary>
        public Vector Position { get; }

        public MouseArgs(Control sender, Vector absPos)
        {
            Control = sender;
            Position = absPos - sender.AbsolutePosition;
        }
    }
}
