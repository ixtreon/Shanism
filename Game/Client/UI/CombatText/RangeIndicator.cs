using IO.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CombatText
{
    class RangeIndicator : Control
    {
        const int BlinkDuration = 250;
        const int WaitDuration = 250;

        bool isCustom;
        double range;
        int durationLeft;

        //needs testing..
        static bool isBlinkShown(int msRemaining)
            => (msRemaining % (BlinkDuration + WaitDuration)) < BlinkDuration;


        public void ShowRange(double range, int duration, bool blink)
        {
            this.range = range;
            this.durationLeft = duration;
            isCustom = true;
        }


        protected override void OnUpdate(int msElapsed)
        {
            if (durationLeft > 0)
                durationLeft -= Math.Min(durationLeft, msElapsed);

            if (durationLeft <= 0)
                isCustom = false;

            var curAbilityHover = (HoverControl as SpellButton)?.Ability;
            if (!isCustom && curAbilityHover != null && ((curAbilityHover.TargetType & AbilityTargetType.PointOrUnitTarget) != 0))
            {
                durationLeft = 1;
                range = curAbilityHover.CastRange;
            }
            base.OnUpdate(msElapsed);
        }

        public override void OnDraw(Graphics g)
        {
            if (durationLeft > 0 && (!isCustom || isBlinkShown(durationLeft)))
            {
                var llGame = Screen.InGameCenter - range;
                var urGame = Screen.InGameCenter + range;
                var llPx = Screen.GameToScreen(llGame); 
                var urPx = Screen.GameToScreen(urGame);
                var rekt = new RectangleF(llPx, urPx - llPx);

                var circleSz = (int)Math.Max(rekt.Width, rekt.Height) / 5;
                if (circleSz <= Content.Circles.MaximumSize)
                {
                    var circle = Content.Circles.GetTexture(circleSz);

                    g.SpriteBatch.ShanoDraw(circle, rekt, Color.LimeGreen);
                }

                //g.Draw(Content.Textures.Blank, Vector.Zero, rekt.Size, Color.Pink);
            }
        }
    }
}
