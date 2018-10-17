using System.Numerics;

namespace Shanism.Client.UI
{
    public class MouseScrollArgs : MouseArgs
    {
        public float ScrollDelta { get; }

        public MouseScrollArgs(Control sender, Vector2 absPos, float scrollDelta) 
            : base(sender, absPos)
        {
            ScrollDelta = scrollDelta;
        }
    }
}
