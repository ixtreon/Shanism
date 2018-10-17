using System.Numerics;

namespace Shanism.Client.UI
{
    /// <summary>
    /// The event raised whenever a mouse button does something. Extends <see cref="MouseArgs"/>. 
    /// </summary>
    public class MouseButtonArgs : MouseArgs
    {
        public MouseButton Button { get; }

        public MouseButtonArgs(Control sender, Vector2 pos, MouseButton btn)
            : base(sender, pos)
        {
            Button = btn;
        }
    }
}
