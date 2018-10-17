using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using System;
using System.Numerics;
//using XnaVector = Microsoft.Xna.Framework.Vector2;


namespace Shanism.Client.UI.Game
{
    class RangeIndicator : Control
    {
        const int BlinkDuration = 250;
        const int WaitDuration = 250;



        float curRange;
        Texture2D curTexture;
        //XnaVector screenPos, screenScale;

        bool isHovering;
        bool isBlinking;
        int blinkMsLeft;
        public Color ForeColor { get; set; } = Color.DarkGreen.SetAlpha(64);


        public RangeIndicator()
        {
            IsVisible = false;
        }


        public void Show(float range, int duration = 1250)
        {
            isBlinking = true;
            blinkMsLeft = duration;
            curRange = range;
        }


        public override void Update(int msElapsed)
        {
            blinkMsLeft = Math.Max(blinkMsLeft - msElapsed, 0);
            isBlinking &= (blinkMsLeft > 0);

            var hoverBtn = HoverControl as SpellButton;
            var hoverHasRange = (hoverBtn?.Ability != null)
                && (hoverBtn.Ability.TargetType & AbilityTargetType.PointOrUnitTarget) != 0;

            isHovering = hoverHasRange;
            if (hoverHasRange)
            {
                isBlinking = false;
                curRange = hoverBtn.Ability.CastRange;
            }

            IsVisible = isHovering || (isBlinking && isBlinkShown());
            if (IsVisible)
            {
                var screenSz = (2 * curRange * Screen.Game.Scale).ToXnaVector();
                var reqTexSz = (int)Math.Min(Content.Circles.MaximumSize, Math.Max(screenSz.X, screenSz.Y));

                //screenPos = Screen.GameToScreen(Screen.InGameCenter - new Vector2(curRange)).ToXnaVector();
                curTexture = Content.Circles.GetTexture(reqTexSz);
                //screenScale = screenSz / curTexture.Bounds.Size.ToVector2();
            }
        }

        public override void Draw(Canvas c)
        {
            //TODO

            //c.SpriteBatch.Draw(curTexture,
            //    screenPos, curTexture.Bounds,
            //    ForeColor.ToXnaColor(),
            //    0, XnaVector.Zero,
            //    screenScale,
            //    SpriteEffects.None, 0);
        }

        bool isBlinkShown()
            => blinkMsLeft > 0
            && (blinkMsLeft % (BlinkDuration + WaitDuration)) < BlinkDuration;

    }
}
