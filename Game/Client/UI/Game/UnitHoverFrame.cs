using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Objects;
using Client.Textures;
using IO;
using IO.Common;
using Client.UI.Common;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    /// <summary>
    /// Displays information about a given unit such as its name, level, life and mana. 
    /// </summary>
    class UnitHoverFrame : Control
    {
        const double hpBarHeight = Padding * 4;
        const double manaBarHeight = Padding * 3;

        public UnitControl Target;

        readonly HealthBar healthBar;
        readonly ManaBar manaBar;
        readonly Label nameLabel;



        public UnitHoverFrame()
        {
            Size = new Vector(0.4f, 0.15f);
            Location = new Vector(0.8, 0);

            BackColor = Color.Black.SetAlpha(75);

            var nameFont = Content.Fonts.FancyFont;
            nameLabel = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(Padding),
                Size = new Vector(Size.X - 2 * Padding, nameFont.UiHeight),
                AutoSize = false,

                Font = nameFont,
                Text = "Manqche",
                TextColor = Color.White,
                TextXAlign = 0.5f,
            };


            manaBar = new ManaBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Bottom | AnchorMode.Right,
                Location = new Vector(Padding, Size.Y - Padding - manaBarHeight),
                Size = new Vector(Size.X - 2 * Padding, manaBarHeight),

                ForeColor = Color.DarkBlue,
            };

            healthBar = new HealthBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Bottom | AnchorMode.Right,
                Location = new Vector(Padding, manaBar.Top - hpBarHeight),
                Size = new Vector(manaBar.Size.X, hpBarHeight),

                ForeColor = Color.DarkRed,
            };

            Add(nameLabel);
            Add(manaBar);
            Add(healthBar);
        }

        protected override void OnUpdate(int msElapsed)
        {
            var unitTarget = Target?.Unit;

            Visible = (unitTarget != null);
            if(Visible)
            {
                healthBar.Target = unitTarget;
                manaBar.Target = unitTarget;

                nameLabel.Text = unitTarget.Name;
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
