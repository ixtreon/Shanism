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
            this.ColorPalette[0] = Color.DarkBlue;
            this.ColorPalette[1] = Color.Blue;
            this.ColorPalette[2] = Color.LightBlue;
        }

        public override void Update(int msElapsed)
        {
            if (Target == null)
                return;

            if(Target.IsDead)
            {
                ShowText = false;
                MaxValue = Value = 0;
                TooltipText = string.Empty;
            }
            else
            {
                ShowText = true;
                Value = Target.Mana;
                MaxValue = Target.MaxMana;
                TooltipText = "{0:+0.0;-0.0;0}/sec".Format(Target.ManaRegen);
            }
        }
    }
}
