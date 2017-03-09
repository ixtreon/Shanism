using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Client.UI.Game
{
    class HealthBar : ValueBar
    {
        public BarColorStyle ColorStyle { get; set; } = BarColorStyle.Value;

        public readonly Color[] ColorPalette = 
        {
            Color.DarkRed,
            Color.Gold,
            Color.Green,
        };

        public IUnit Target { get; set; }


        protected override void OnUpdate(int msElapsed)
        {
            IsVisible = (Target != null);
            if (!IsVisible)
                return;

            var c = Color.Gold;
            //set values, text

            ToolTip = string.Empty;
            if (Target.IsDead)
                MaxValue = 0;
            else if (Target.StateFlags.HasFlag(StateFlags.Invulnerable))
                MaxValue = Value = 1;
            else
            {
                Value = Target.Life;
                MaxValue = Target.Stats[UnitStat.MaxLife];
                ToolTip = $"{Target.Stats[UnitStat.LifeRegen]:+0.0;-0.0;0}/sec";
                switch (ColorStyle)
                {
                    case BarColorStyle.Fixed:
                        ForeColor = ColorPalette.First();
                        break;
                    case BarColorStyle.Faction: //NYI
                        ForeColor = ColorPalette.Last();
                        break;
                    case BarColorStyle.Value:
                        if (Value >= MaxValue)
                            ForeColor = ColorPalette.Last();
                        else
                        {
                            var n = 1.0 / (ColorPalette.Length - 1);
                            var paletteId = (int)((Value / MaxValue) / n);
                            var amount = (float)(((Value / MaxValue) % n) / n);
                            if (paletteId >= 0 && paletteId < ColorPalette.Length - 1)
                                ForeColor = Color.Lerp(ColorPalette[paletteId], ColorPalette[paletteId + 1], amount);
                        }
                        break;
                }
            }

            base.OnUpdate(msElapsed);
        }
    }

    enum BarColorStyle
    {
        /// <summary>
        /// Colors the bar using a fixed color. 
        /// </summary>
        Fixed, 

        /// <summary>
        /// Colors the bar based on the current value of the bar. 
        /// </summary>
        Value,

        /// <summary>
        /// Colors the bar based on the faction of the unit. 
        /// </summary>
        Faction,
    }
}
