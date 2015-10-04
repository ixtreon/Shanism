using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Objects;
using Client.Sprites;
using Client.Textures;
using IO;
using Client.UI.Common;

namespace Client.UI
{
    class TargetUi : Control
    {
        const float modelOffset = Anchor * 2;
        const float barHeight = Anchor * 3;

        public UnitControl Target;

        ValueBar healthBar, manaBar;

        BuffBar buffBar;

        public float SpriteSize
        {
            get { return Size.Y - 2 * modelOffset; }
        }

        public float SpriteBoxSize
        {
            get { return SpriteSize + 2 * modelOffset; }
        }

        public TargetUi()
        {
            this.Size = new Vector2(0.5f, 0.15f);
            this.AbsolutePosition = new Vector2(-Size.X / 2, -1);
            this.BackColor = Color.Black.SetAlpha(100);
            
            healthBar = new ValueBar()
            {
                ForeColor = Color.DarkRed,
            };
            this.Add(healthBar);

            buffBar = new BuffBar()
            {
                BackColor = Color.Transparent,
                ClickThrough = true,
            };
            this.Add(buffBar);
        }

        public override void Update(int msElapsed)
        {
            Visible = (Target != null);

            healthBar.RelativePosition = new Vector2(SpriteBoxSize, Size.Y - Anchor - barHeight);
            healthBar.Size = new Vector2(Size.X - SpriteBoxSize - Anchor, barHeight);
            if(Target != null)
            {
                healthBar.Visible = !Target.Unit.IsDead;
                healthBar.Value = Target.Unit.Life;
                healthBar.MaxValue = Target.Unit.MaxLife;
            }

            buffBar.RelativePosition = new Vector2(0, Size.Y);
            buffBar.Size = new Vector2(Size.X, 0.4f);
            buffBar.Target = Target?.Unit;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Target == null)
                return;

            //draw the background
            base.Draw(sb);

            //draw the guy's model
            Target.Sprite.Draw(sb,
                AbsolutePosition + new Vector2(modelOffset),
                new Vector2(SpriteSize));


            //get the total model size including the anchor box
            var boxCenter = SpriteBoxSize + (Size.X - SpriteBoxSize - Anchor) / 2;

            //get the name size
            var nameFont = TextureCache.FancyFont;
            var nameSz = nameFont.MeasureStringUi(Target.Unit.Name);
            var namePos = new Vector2(boxCenter, Anchor);

            nameFont.DrawString(sb, 
                Target.Unit.Name, Color.White, 
                AbsolutePosition + namePos, 0.5f, 0);

            //TextureCache.StraightFont.DrawStringScreen(sb, Target.Unit.Name, Color.White, namePos + new Point(0, 100), 0, 0);

            //get the level
            var lvlFont = TextureCache.SmallFont;
            var sLevel = "Level {0}".Format(Target.Unit.Level);

            var levelSz = lvlFont.MeasureStringUi(sLevel);
            var levelPos = new Vector2(boxCenter, namePos.Y + nameSz.Y);
            lvlFont.DrawString(sb,
                sLevel, Color.White,
                AbsolutePosition + levelPos, 0.5f, 0);

        }
    }
}
