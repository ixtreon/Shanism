using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

using Shanism.Common.Interfaces.Entities;
using Shanism.Client.Drawing;

namespace Shanism.Client.UI.Game
{
    /// <summary>
    /// Displays information about the hover unit such as its name, level, life and mana. 
    /// </summary>
    class UnitHoverFrame : Control
    {
        public UnitSprite Target;

        readonly Label nameLabel;
        readonly Label lvlLabel;
        readonly HealthBar healthBar;
        readonly ManaBar manaBar;



        public UnitHoverFrame(double hpBarHeight = 0.04, double manaBarHeight = 0.03)
        {
            Size = new Vector(0.4f, 0.15f);
            Location = new Vector(0.8, 0);
            BackColor = Color.Black.SetAlpha(75);

            var nameFont = Content.Fonts.FancyFont;
            var lvlFont = Content.Fonts.SmallFont;

            nameLabel = new Label
            {
                AutoSize = false,
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(Padding),
                Size = new Vector(Size.X - 2 * Padding, nameFont.HeightUi),

                Font = nameFont,
                TextColor = Color.White,
                TextXAlign = 0.5f,
                Text = "Dummy Unit",
            };

            lvlLabel = new Label
            {
                AutoSize = false,
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(Padding, nameLabel.Bottom + Padding),
                Size = new Vector(nameLabel.Size.X, lvlFont.HeightUi),

                Font = lvlFont,
                TextColor = Color.White,
                TextXAlign = 0.5f,
                Text = "Level 42",
            };

            healthBar = new HealthBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Bottom | AnchorMode.Right,
                Location = new Vector(Padding, lvlLabel.Bottom + Padding),
                Size = new Vector(Size.X - 2 * Padding, hpBarHeight),
                ShowText = false,

                ForeColor = Color.DarkRed,
            };

            manaBar = new ManaBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Bottom | AnchorMode.Right,
                Location = new Vector(Padding, healthBar.Bottom),
                Size = new Vector(healthBar.Size.X, manaBarHeight),
                ShowText = false,

                ForeColor = Color.DarkBlue,
            };

            Size = new Vector(Size.X, manaBar.Bottom + Padding);

            Add(nameLabel);
            Add(lvlLabel);
            Add(healthBar);
            Add(manaBar);
        }

        protected override void OnUpdate(int msElapsed)
        {
            var unitTarget = Target?.Unit;

            IsVisible = (unitTarget != null);
            if(IsVisible)
            {
                healthBar.IsVisible = manaBar.IsVisible = !unitTarget.IsDead;

                healthBar.Target = unitTarget;
                manaBar.Target = unitTarget;

                nameLabel.Text = unitTarget.Name;
                lvlLabel.Text = $"Level {unitTarget.Level}";

                var heroTarget = unitTarget as IHero;
                if (heroTarget != null)
                    lvlLabel.ToolTip = $"{heroTarget.Experience}/{heroTarget.ExperienceNeeded} XP";
                else
                    lvlLabel.ToolTip = string.Empty;

            }
        }

        public override void OnDraw(Graphics g)
        {
            if (Target == null)
                return;

            //background
            base.OnDraw(g);
        }
    }
}
