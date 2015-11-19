using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Objects;
using Client.Textures;
using IO;
using Client.UI.Common;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;

namespace Client.UI
{
    class UnitFrame : Control
    {
        const double modelOffset = Padding * 2;
        const double barHeight = Padding * 3;

        public UnitControl Target;

        ValueBar healthBar, manaBar;

        BuffBar buffBar;

        public double SpriteSize
        {
            get { return Size.Y - 2 * modelOffset; }
        }

        public double SpriteBoxSize
        {
            get { return SpriteSize + 2 * modelOffset; }
        }

        public UnitFrame()
        {
            this.Size = new Vector(0.5f, 0.15f);
            this.AbsolutePosition = new Vector(-Size.X / 2, -1);
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

            healthBar.Location = new Vector(SpriteBoxSize, Size.Y - Padding - barHeight);
            healthBar.Size = new Vector(Size.X - SpriteBoxSize - Padding, barHeight);
            if(Target != null)
            {
                healthBar.Visible = !Target.Unit.IsDead;
                healthBar.Value = Target.Unit.Life;
                healthBar.MaxValue = Target.Unit.MaxLife;
            }

            buffBar.Location = new Vector(0, Size.Y);
            buffBar.Size = new Vector(Size.X, 0.4f);
            buffBar.Target = Target?.Unit;
        }

        public override void Draw(Graphics g)
        {
            if (Target == null)
                return;

            //background
            base.Draw(g);

            //unit model
            g.Draw(Target.Sprite,
                new Vector(modelOffset),
                new Vector(SpriteSize));


            //get the total model size including the anchor box

            //unit name
            var boxCenter = SpriteBoxSize + (Size.X - SpriteBoxSize - Padding) / 2;
            var nameFont = Content.Fonts.FancyFont;
            var nameSz = nameFont.MeasureStringUi(Target.Unit.Name);
            var namePos = new Vector(boxCenter, Padding);

            g.DrawString(nameFont, 
                Target.Unit.Name, Color.White,
                namePos, 0.5f, 0);

            //TextureCache.StraightFont.DrawStringScreen(sb, Target.Unit.Name, Color.White, namePos + new Point(0, 100), 0, 0);

            //unit level
            var lvlFont = Content.Fonts.SmallFont;
            var sLevel = "Level {0}".Format(Target.Unit.Level);
            var levelSz = lvlFont.MeasureStringUi(sLevel);
            var levelPos = new Vector(boxCenter, namePos.Y + nameSz.Y);

            g.DrawString(lvlFont,
                sLevel, Color.White,
                levelPos, 0.5f, 0);

        }
    }
}
