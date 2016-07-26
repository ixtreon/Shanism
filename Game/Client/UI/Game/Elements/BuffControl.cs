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
        public new double Size
        {
            get { return base.Size.X; }
            set { base.Size = new Vector(value); }
        }

        public IBuffInstance Buff { get; set; }

        Texture2D buffTexture;

        public BuffBox()
        {
            ToolTip = "asdasdasdasd";
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (Buff != null)
                buffTexture = Content.Textures.TryGetIcon(Buff.Icon) ?? Content.Textures.DefaultIcon;
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            //draw the buff
            if(buffTexture != null)
                g.Draw(buffTexture, Vector.Zero, base.Size, Color.White);

            if(Buff.FullDuration > 0)
            {
                var shSize = base.Size * new Vector(1, (double)Buff.DurationLeft / Buff.FullDuration);
                var shPos = base.Size - shSize;
                g.Draw(Content.Textures.Blank, shPos, shSize, Color.Black.SetAlpha(150));
            }
        }
    }
}
