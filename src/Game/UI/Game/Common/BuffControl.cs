using Shanism.Client.Sprites;
using Shanism.Common;
using Shanism.Common.Objects;
using System.Numerics;

namespace Shanism.Client.UI.Game
{
    class BuffBox : Control
    {
        public new float Size
        {
            get => base.Size.X;
            set => base.Size = new Vector2(value);
        }

        public IBuffInstance Buff { get; }

        Sprite currentTexture;
        float percCooldownLeft;
        Color currentTint;


        public BuffBox(IBuffInstance buff)
        {
            ToolTip = "wut";
            Buff = buff;
        }

        public override void Update(int msElapsed)
        {
            var b = Buff.Prototype;

            //update icon
            if (b.HasIcon)
            {
                if (b.Icon != currentTexture?.Name)
                {
                    currentTexture = Content.Icons.Get(b.Icon);
                }

                currentTint = b.IconTint;
            }

            //update cooldown shade
            if (b.FullDuration > 0)
                percCooldownLeft = 0;
            else
                percCooldownLeft = (float)Buff.DurationLeft / b.FullDuration;
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            //draw the buff
            if(currentTexture != null)
                c.DrawSprite(currentTexture, Vector2.Zero, base.Size, currentTint);

            if(percCooldownLeft > 0)
            {
                var shSize = base.Size * new Vector2(1, percCooldownLeft);
                var shPos = base.Size - shSize;
                c.FillRectangle(shPos, shSize, UiColors.ControlBackground);
            }
        }
    }
}
