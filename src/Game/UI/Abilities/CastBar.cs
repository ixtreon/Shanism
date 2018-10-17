using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common.Entities;
using System.Numerics;

namespace Shanism.Client.UI
{
    class CastBar : Control
    {
        public IUnit Target { get; set; }

        float barFillValue;
        float secondsLeft;

        public CastBar()
        {
            BackColor = UiColors.ControlBackground;

            Size = new Vector2(0.5f, 0.08f);
        }

        public override void Update(int msElapsed)
        {
            IsVisible = Target != null
                && Target.IsCasting()
                && Target.TotalCastingTime > 0;

            if (IsVisible)
            {
                barFillValue = Target.CastingProgress / Target.TotalCastingTime;
                secondsLeft = (Target.TotalCastingTime - Target.CastingProgress) / 1000f;
            }

            base.Update(msElapsed);
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            if (!IsVisible)
                return;

            var borderSize = Padding;
            var fullSize = Size - new Vector2(borderSize * 2);
            var valSize = fullSize * new Vector2(barFillValue, 1);

            c.FillRectangle(ClientBounds.Position, valSize, Color.Goldenrod);

            var text = $"-{secondsLeft:0.0}s";
            var textPos = new Vector2(2 * Padding, Size.Y / 2);
            c.DrawString(Content.Fonts.NormalFont, text,
                textPos, UiColors.Text, anchor: AnchorPoint.CenterLeft);
        }
    }
}
