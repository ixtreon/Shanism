using Shanism.Client.IO;
using Shanism.Common;
using System.Numerics;

namespace Shanism.Client.UI.Tooltips
{
    public abstract class TooltipBase : Control
    {
        public MouseSystem Mouse { get; set; }

        public override void Update(int msElapsed)
        {
            
            if (IsVisible && Mouse != null)
            {
                BringToFront();
                Location = Screen.UI.FromScreen(Mouse.ScreenPosition + Mouse.CursorSize)
                    .Clamp(Vector2.Zero, Screen.UI.Size - Size);
            }
        }
    }
}
