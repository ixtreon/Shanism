using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShanoRpgWinGl.Properties;
using ShanoRpgWinGl.Sprites;
using ShanoRpgWinGl.UI;

namespace ShanoRpgWinGl.Objects
{
    class UnitControl : ObjectControl
    {
        public IUnit Unit
        {
            get { return (IUnit)this.Object; }
        }



        public UnitControl(IUnit u)
            : base(u) { }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }
        
        public override void Draw(SpriteBatch sb)
        {
            //update period depending on move state
            var moving = false;
            Sprite.Period = moving ? 100 : 1000;

            Vector2 sz = new Vector2((float)Unit.Size);

            var c = Unit.IsDead ? Color.LightGray : Color.White;
            Sprite.Draw(sb, ScreenPosition, ScreenSize, c);

            if ((MouseOver || Settings.Default.AlwaysShowHealthBars) && !Unit.IsDead)
            {
                var barBackColor = Color.Black.SetAlpha(100);
                var barForeColor = Color.DarkRed.SetAlpha(210);
                var barHeight = Screen.UiToScreen(0.03);
                var barPosition = ScreenPosition - new Point(0, barHeight);
                UI.ValueBar.DrawValueBar(sb, Unit.Life, Unit.MaxLife, barPosition, new Point(ScreenSize.X, barHeight), barBackColor, barForeColor);
                //TextureCache.MainFont.DrawString(sb, Unit.CurrentLife.ToString(), Color.Red, ScreenPosition.X, ScreenPosition.Y, 0.5f, 1f);
            }
        }
    }
}
