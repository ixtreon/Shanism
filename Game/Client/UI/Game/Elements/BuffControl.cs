using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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

        public IBuffInstance Buff { get; set; }

        Texture2D buffTexture;
        float shadeRatio;


        public BuffBox()
        {
            ToolTip = "wut";
        }

        protected override void OnUpdate(int msElapsed)
        {
            //update icon
            if (Buff.Prototype.Icon != buffTexture?.Name)
                buffTexture = Content.Textures.TryGetIcon(Buff.Prototype.Icon) 
                    ?? Content.Textures.DefaultIcon;

            //update cooldown shade
            if (Buff.Prototype.FullDuration > 0)
                shadeRatio = 0;
            else
                shadeRatio = (float)Buff.DurationLeft / Buff.Prototype.FullDuration;
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            //draw the buff
            if(buffTexture != null)
                g.Draw(buffTexture, Vector.Zero, base.Size, Color.White);

            if(shadeRatio > 0)
            {
                var shSize = base.Size * new Vector(1, shadeRatio);
                var shPos = base.Size - shSize;
                g.Draw(Content.Textures.Blank, shPos, shSize, shadeColor);
            }
        }
    }
}
