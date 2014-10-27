using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShanoRpgWinGl.Objects;
using ShanoRpgWinGl.Sprites;

namespace ShanoRpgWinGl.UI
{
    class TargetUi : UserControl
    {
        public UnitControl Target;

        public TargetUi()
        {
            this.Size = new Vector2(0.4f, 0.1f);
            this.AbsolutePosition = new Vector2(-Size.X / 2, -1);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Target == null)
                return;

            var backColor = new Color(50, 50, 50, 200);
            SpriteCache.BlankTexture.Draw(sb, ScreenPosition, ScreenSize, backColor);

            TextureCache.MainFont.DrawString(sb, Target.Unit.Name, Color.White, ScreenPosition, 0, 0);

        }
    }
}
