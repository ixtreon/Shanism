using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Properties;
using Client.Sprites;
using Client.UI;

namespace Client.Objects
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
            
            base.Update(Unit.IsDead ? 0 :  msElapsed);

            if (Unit.IsDead)
                this.ZOrder -= 10 * Constants.Client.WindowHeight;
        }
        
        public override void Draw(SpriteBatch sb)
        {
            //update period depending on move state
            var moving = false;
            if(Sprite is AnimatedSprite)
                ((AnimatedSprite)Sprite).Period = moving ? 100 : 1000;

            Vector2 sz = new Vector2((float)Unit.Size);

            var c = Unit.IsDead ? Color.Black : Color.White;
            Sprite.DrawScreen(sb, ScreenPosition, ScreenSize, c);

            if ((MouseOver || Settings.Default.AlwaysShowHealthBars) && !Unit.IsDead)
            {
                var barBackColor = Color.Black.SetAlpha(100);
                var barForeColor = Color.DarkRed.SetAlpha(210);
                var barHeight = Screen.UiToScreen(0.03);
                var barPosition = ScreenPosition - new Point(0, barHeight);
                ValueBar.DrawValueBar(sb, Unit.Life, Unit.MaxLife, barPosition, new Point(ScreenSize.X, barHeight), barBackColor, barForeColor);
            }

        }
    }
}
