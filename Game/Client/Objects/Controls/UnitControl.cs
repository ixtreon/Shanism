using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework.Graphics;
using Client.UI;
using Client.UI.Common;
using IO.Objects;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;

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

        protected override void OnUpdate(int msElapsed)
        {
            base.OnUpdate(Unit.IsDead ? 0 :  msElapsed);

            if (Unit.IsDead)
                ZOrder = 1;
        }
        
        public override void OnDraw(Graphics g)
        {
            var sz = new Vector(Unit.Scale);

            var c = Unit.IsDead ? Color.Black : Color.White;
            g.Draw(Sprite, Vector.Zero, Size, c, (float)ZOrder);

            if ((MouseOver || ShanoSettings.Current.QuickButtonPress) && !Unit.IsDead)
            {
                var barBackColor = Color.Black.SetAlpha(100);
                var barForeColor = Color.DarkRed.SetAlpha(210);
                var barHeight = Screen.UiScale * 0.03;
                var barPosition = ScreenLocation - new Point(0, (int)barHeight);
                //ValueBar.DrawValueBar(g.SpriteBatch, Unit.Life, Unit.MaxLife, barPosition, new Point(ScreenSize.X, barHeight), barBackColor, barForeColor);
            }

        }
    }
}
