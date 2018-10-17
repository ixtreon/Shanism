using System.Numerics;

namespace Shanism.Client.UI.Tooltips
{
    /// <summary>
    /// A text-only tooltip.
    /// </summary>
    public class TextTooltip : TooltipBase
    {
        string text;

        public Font Font { get; set; }

        public float MaxWidth { get; set; } = 0.55f;

        public TextTooltip()
        {
            Font = Content.Fonts.NormalFont;
            BackColor = UiColors.ControlBackground;
            Padding = LargePadding;
        }

        public override void Update(int msElapsed)
        {
            var newText = HoverControl?.ToolTip as string;
            IsVisible = !string.IsNullOrWhiteSpace(newText);
            if (!IsVisible)
                return;

            if (newText != text)
            {
                text = newText;
                Size = Font.MeasureString(text, MaxWidth) + new Vector2(Padding * 2);
            }

            base.Update(msElapsed);
        }

        public override void Draw(Canvas g)
        {
            base.Draw(g);
            g.DrawString(Font, text, UiColors.Text, new Vector2(Padding),
                maxWidth: MaxWidth);
        }
    }
}
