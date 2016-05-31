using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using Shanism.Client.UI.Common;
using Shanism.Common.Objects;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Common.Game;

namespace Shanism.Client.Objects
{
    class UnitControl : ObjectControl
    {
        public IUnit Unit { get; }


        public UnitControl(IUnit u)
            : base(u) { Unit = u; }

        protected override void OnUpdate(int msElapsed)
        {
            base.OnUpdate(Unit.IsDead ? 0 :  msElapsed);

            if (Unit.IsDead)
                ZOrder = 0;
        }
        
        public override void OnDraw(Graphics g)
        {
            var sz = new Vector(Unit.Scale);

            var c = Unit.IsDead ? Color.Black : Object.CurrentTint.ToColor();
            g.Draw(Sprite, Vector.Zero, Size, c, (float)ZOrder);

            if ((MouseOver || Settings.Current.QuickButtonPress) && !Unit.IsDead)
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
