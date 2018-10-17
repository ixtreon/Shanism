using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using Shanism.Common.Objects;
using System.Numerics;
using Ix.Math;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A simple button that displays an ability's icon and a tooltip. 
    /// </summary>
    class SpellButton : ToggleButton
    {


        string iconName;
        RectangleF? cooldownBox;
        Color borderColor;

        /// <summary>
        /// Gets or sets whether this spell button has a border drawn around it. 
        /// </summary>
        public bool HasBorder { get; set; } = true;

        public Color CooldownTint { get; set; }

        public IAbility Ability { get; set; }

        public SpellButton()
        {
            BackColor = Color.Black;
            CooldownTint = UiColors.HoverOverlay;

            CanDrag = true;
        }

        public override void Update(int msElapsed)
        {
            UpdateIcon(Ability?.Icon);
            Tint = Ability?.IconTint ?? Color.Black;
            ToolTip = Ability;

            cooldownBox = CalcCooldownBox();
        }

        void UpdateIcon(string newIconName)
        {
            if (newIconName == iconName)
                return;

            IconName = newIconName ?? "uncertainty";
            iconName = newIconName;
        }

        RectangleF? CalcCooldownBox()
        {
            if (Ability == null || Ability.Cooldown <= 0 || Ability.CurrentCooldown <= 0)
                return null;

            var perc = (float)Ability.CurrentCooldown / Ability.Cooldown;
            return new RectangleF(0, 1 - perc, 1, perc) * Size;
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            // cooldown
            if (cooldownBox != null)
                c.FillRectangle(cooldownBox.Value, CooldownTint);

            // button border
            if (HasBorder)
            {
                borderColor = IsSelected ? Color.Goldenrod : Color.White;
                if (IsHoverControl)
                    borderColor = borderColor.Darken(25);

                c.DrawSprite(Content.UI.IconBorderHover, Vector2.Zero, Size, borderColor);
            }
        }


    }
}
