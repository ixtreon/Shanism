using Shanism.Client.UI;
using System;

namespace Shanism.Client
{
    public class DragArgs : EventArgs
    {
        /// <summary>
        /// Gets the control that is being dragged.
        /// </summary>
        public Control Source { get; }

        public DragArgs(Control source)
        {
            Source = source;
        }
    }
    public class DragDropArgs : DragArgs
    {

        /// <summary>
        /// Gets the destination control the <see cref="Source"/> was dropped on,
        /// if available.
        /// </summary>
        public Control Destination { get; }

        public DragDropArgs(Control source, Control destination)
            : base(source)
        {
            Destination = destination;
        }
    }
}
