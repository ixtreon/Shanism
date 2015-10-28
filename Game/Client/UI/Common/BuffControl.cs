using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using IO.Objects;
using Client.Textures;
using Microsoft.Xna.Framework;
using IO.Common;

namespace Client.UI.Common
{
    class BuffControl : Control
    {
        public new double Size
        {
            get { return base.Size.X; }
            set { base.Size = new Vector(value); }
        }

        public IBuffInstance Buff { get; set; }

        private Texture2D buffTexture;

        public BuffControl(float size = 0.05f)
        {
            Size = size;
            TooltipText = "lapai pishki :D";
        }

        public override void Update(int msElapsed)
        {
            buffTexture = Buff?.GetIconTexture();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            //draw the buff
            if(buffTexture != null)
                sb.DrawUi(buffTexture, AbsolutePosition, base.Size, Color.White);
        }
    }
}
