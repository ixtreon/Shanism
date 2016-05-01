using IO;
using IO.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Common
{
    class ManaBar : ValueBar
    {
        public readonly Color[] ColorPalette =
        {
            new Color(0, 0, 191),
            new Color(50, 50, 255),
        };

        public IUnit Target { get; set; }

        protected override void OnUpdate(int msElapsed)
        {
            IsVisible = (Target != null);
            if (!IsVisible)
                return;

            if(Target.IsDead)
            {
                MaxValue = Value = 0;
                ToolTip = string.Empty;
            }
            else
            {
                Value = Target.Mana;
                MaxValue = Target.MaxMana;
                ForeColor = Color.Lerp(ColorPalette[0], ColorPalette[1], (float)(MaxValue / Value).Clamp(0, 1));
                ToolTip = "{0:+0.0;-0.0;0}/sec".F(Target.ManaRegen);
            }

            base.OnUpdate(msElapsed);
        }
    }
}
