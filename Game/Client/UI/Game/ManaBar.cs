using IO;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Common
{
    class ManaBar : HealthBar
    {
        public ManaBar()
        {
            this.ColorPalette[0] = new Color(0, 0, 191);
            this.ColorPalette[1] = new Color(50, 50, 223);
            this.ColorPalette[2] = new Color(100, 100, 255);
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (Target == null)
                return;

            if(Target.IsDead)
            {
                ShowText = false;
                MaxValue = Value = 0;
                ToolTip = string.Empty;
            }
            else
            {
                ShowText = true;
                Value = Target.Mana;
                MaxValue = Target.MaxMana;
                ToolTip = "{0:+0.0;-0.0;0}/sec".F(Target.ManaRegen);
            }
        }
    }
}
