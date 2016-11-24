using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Game
{
    class RangeIndicator : Control
    {
        const int BlinkDuration = 250;
        const int WaitDuration = 250;


        public Color ForeColor { get; set; } = Color.DarkGreen.SetAlpha(64);

        bool isForced;
        double range;
        int durationLeft;

        static bool isBlinkShown(int msRemaining)
            => (msRemaining % (BlinkDuration + WaitDuration)) < BlinkDuration;


        public void Show(double range, int duration = 1250)
        {
            this.range = range;
            this.durationLeft = duration;
            isForced = true;
        }


        protected override void OnUpdate(int msElapsed)
        {
            if (durationLeft > 0)
                durationLeft -= Math.Min(durationLeft, msElapsed);

            if (durationLeft <= 0)
                isForced = false;

            var curAbilityHover = (HoverControl as SpellButton)?.Ability;
            if (!isForced && curAbilityHover != null && ((curAbilityHover.TargetType & AbilityTargetType.PointOrUnitTarget) != 0))
            {
                durationLeft = 1;
                range = curAbilityHover.CastRange;
            }
            base.OnUpdate(msElapsed);
        }

        public override void OnDraw(Graphics g)
        {
            if (durationLeft <= 0)
                return;

            if (isForced && !isBlinkShown(durationLeft))
                return;

            //DrawOrder = 0;
            var gamePos = Screen.GameCenter - range;
            var screenPos = Screen.GameToScreen(gamePos);
            var screenDiameter = Screen.GameScale * (2 * range);
            var rekt = new RectangleF(screenPos, screenDiameter);

            var circleSz = (int)Math.Max(screenDiameter.X, screenDiameter.Y) * 2;
            if (circleSz < Content.Circles.MaximumSize)
            {
                var circle = Content.Circles.GetTexture(circleSz);

                g.Draw(circle,
                    sourceRectangle: circle.Bounds,
                    position: screenPos.ToVector2(),
                    scale: (screenDiameter / new Vector(circle.Width, circle.Height)).ToVector2(),
                    color: ForeColor.ToXnaColor());
            }
        }
    }
}
