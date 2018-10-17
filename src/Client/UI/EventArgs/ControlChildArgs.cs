using System;

namespace Shanism.Client.UI
{
    /// <summary>
    /// The data about something happening to a control and their parent.
    /// </summary>
    public class ControlChildArgs : EventArgs
    {
        public Control Parent { get; }

        public Control Child { get; }

        public ControlChildArgs(Control parent, Control child)
        {
            Parent = parent;
            Child = child;
        }
    }

    /// <summary>
    /// The data about a control's change of parents.
    /// </summary>
    public class ParentChangeArgs : ControlChildArgs
    {
        public Control PreviousParent { get; }

        public ParentChangeArgs(Control child, Control oldParent, Control newParent)
            : base(newParent, child)
        {
            PreviousParent = oldParent; 
        }
    }
}
