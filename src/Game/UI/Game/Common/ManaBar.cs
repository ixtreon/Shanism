using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common.Entities;

namespace Shanism.Client.UI.Game
{
    class ManaBar : ValueBar
    {
        public readonly Color[] ColorPalette =
        {
            new Color(0, 0, 191),
            new Color(50, 50, 255),
        };

        public IUnit Target { get; set; }

        public override void Update(int msElapsed)
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
                MaxValue = Target.Stats[UnitField.MaxMana];
                ForeColor = Color.Lerp(ColorPalette[0], ColorPalette[1], (float)(Value / MaxValue).Clamp(0, 1));
                ToolTip = $"{Target.Stats[UnitField.ManaRegen]:+0.0;-0.0;0}/sec";
            }

            base.Update(msElapsed);
        }
    }
}
