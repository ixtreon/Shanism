using Client.Sprites;
using Client.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI
{
    class Label : Control
    {
        public string Text = string.Empty;

        public Color TextColor = Color.Goldenrod;

        public TextureFont Font { get; set; }

        public bool AutoSize { get; set; }

        public Label()
        {
            this.BackColor = Color.Transparent;
            this.Font = TextureCache.FancyFont;
            this.AutoSize = true;

            this.Size = new Vector2(0.37f, 0.08f);
            this.Locked = true;
            this.ClickThrough = true;
        }
        public override void Update(int msElapsed)
        {
            if(AutoSize)
                this.Size = Screen.ScreenToUi(Font.MeasureString(Text)) + new Vector2(Anchor * 2, Anchor * 2);

            base.Update(msElapsed);
        }

        public override void Draw(SpriteBatch sb)
        {
            SpriteFactory.Blank.Draw(sb, AbsolutePosition, Size, BackColor);

            Font.DrawString(sb, Text, TextColor, AbsolutePosition + new Vector2(Anchor, Anchor), yAnchor: 0.5f);

            //Font.DrawStringScreen(sb, Text, TextColor, ScreenPosition.Add(6, ScreenSize.Y / 2), yAnchor: 0.5f);
        }
    }
}
