using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Sprites;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;

namespace Shanism.Client.UI.Game
{
    class BuffBox : Control
    {
        static readonly Color shadeColor = Color.Black.SetAlpha(150);

        public new double Size
        {
            get { return base.Size.X; }
            set { base.Size = new Vector(value); }
        }

        public IBuffInstance Buff { get; }

        StaticSprite currentTexture;
        float shadeRatio;
        Color currentTint;


        public BuffBox(IBuffInstance buff)
        {
            ToolTip = "wut";
            Buff = buff;
        }

        protected override void OnUpdate(int msElapsed)
        {
            var b = Buff.Prototype;

            //update icon
            if (b.HasIcon)
            {
                if (b.Icon != currentTexture?.Texture.Name)
                    currentTexture = Content.Icons.TryGet(b.Icon) ?? Content.Icons.Default;
                currentTint = b.IconTint;
            }

            //update cooldown shade
            if (b.FullDuration > 0)
                shadeRatio = 0;
            else
                shadeRatio = (float)Buff.DurationLeft / b.FullDuration;
        }

        public override void OnDraw(Canvas g)
        {
            base.OnDraw(g);

            //draw the buff
            if(currentTexture != null)
                g.Draw(currentTexture, Vector.Zero, base.Size, currentTint);

            if(shadeRatio > 0)
            {
                var shSize = base.Size * new Vector(1, shadeRatio);
                var shPos = base.Size - shSize;
                g.Draw(Content.Textures.Blank, shPos, shSize, shadeColor);
            }
        }
    }
}
