using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    /// <summary>
    /// The event raised whenever the mouse does something. 
    /// </summary>
    public class MouseArgs : EventArgs
    {
        /// <summary>
        /// The position of the mouse cursor relative to the triggering control. 
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// The absolute position of the mouse cursor in UI space.
        /// </summary>
        public Vector2 AbsolutePosition { get; }

        public MouseArgs(Control sender, Vector2 absPos)
        {
            AbsolutePosition = absPos;
            Position = absPos - sender.AbsoluteLocation;
        }
    }

}
