using Shanism.Common;
using System.Numerics;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A progress bar that exposes a value between 0 and 1, and some text. 
    /// </summary>
    public class ProgressBar : Control
    {
        float _progress;

        public Color ForeColor { get; set; }

        public bool ShowText { get; set; } = true;

        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the progress as a value in the range from 0.0 to 1.0. 
        /// </summary>
        public float Progress
        {
            get => _progress;
            set => _progress = value.Clamp(0, 1);
        }


        public ProgressBar()
        {
            CanHover = true;
            BackColor = UiColors.ControlBackground;
            ForeColor = UiColors.Button;
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            //value
            if(Progress > 0)
            {
                var valSize = ClientBounds.Size * new Vector2(Progress, 1);
                c.FillRectangle(ClientBounds.Position, valSize, ForeColor);
            }

            //text
            if(ShowText && !string.IsNullOrEmpty(Text))
                c.DrawString(Content.Fonts.NormalFont, Text, Size / 2, UiColors.Text, anchor: AnchorPoint.Center);
        }
    }
}
