﻿using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Common.Game;
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
    class MouseArgs
    {
        /// <summary>
        /// The control that triggered the event. 
        /// </summary>
        public Control Control { get; }

        /// <summary>
        /// The position of the mouse cursor relative to the triggering control. 
        /// </summary>
        public Vector RelativePosition { get; }

        /// <summary>
        /// Gets the absolute position of the mouse cursor on the screen. 
        /// </summary>
        public Vector AbsolutePosition { get; }


        public MouseArgs(Control sender, Vector absPos)
        {
            Control = sender;
            RelativePosition = absPos - sender.AbsolutePosition;

            AbsolutePosition = absPos;
        }
    }
}
