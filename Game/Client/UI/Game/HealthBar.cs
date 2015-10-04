using IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Common
{
    class HealthBar : ValueBar
    {
        public BarColorStyle ColorStyle { get; set; }

        public readonly Color[] ColorPalette = 
        {
            Color.DarkRed,
            Color.Yellow,
            Color.Green,
        };

        public IUnit Target { get; set; }


        public HealthBar()
        {
            ShowText = true;
        }

        public override void Update(int msElapsed)
        {
            if (Target == null)
                return;

            //set values, text

            TooltipText = string.Empty;
            ShowText = false;
            if (Target.IsDead)
                MaxValue = 0;
            else if (Target.Invulnerable)
                MaxValue = Value = 1;
            else
            {
                ShowText = true;
                Value = Target.Life;
                MaxValue = Target.MaxLife;
                TooltipText = "{0:+0.0;-0.0;0}/sec".Format(Target.LifeRegen);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Target == null)
                return;

            var text = ShowText ? "{0:0}/{1:0}".Format(Value, MaxValue) : string.Empty;

            switch(ColorStyle)
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
                        ForeColor = Color.Lerp(ColorPalette[paletteId], ColorPalette[paletteId + 1], amount);
                    }
                    break;
            }

            DrawValueBar(sb, Value, MaxValue, ScreenPosition, ScreenSize, BackColor, ForeColor, text);
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
