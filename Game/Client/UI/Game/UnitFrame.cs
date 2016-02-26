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
    class UnitFrame : Control
    {
        const double largePadding = Padding * 2;

        const double hpBarHeight = Padding * 4;
        const double manaBarHeight = Padding * 3;

        public UnitControl Target;


        readonly HealthBar healthBar;
        readonly ManaBar manaBar;
        readonly BuffBar buffBar;
        readonly Label nameLabel;
        readonly Label xpLabel;

        /// <summary>
        /// The size of the unit sprite that is drawn. 
        /// </summary>
        public double SpriteSize
        {
            get { return Size.Y - 2 * largePadding; }
        }

        /// <summary>
        /// The size of the sprite + the border around it. 
        /// </summary>
        public double SpriteBoxSize
        {
            get { return SpriteSize + 2 * largePadding; }
        }


        public UnitFrame()
        {
            Size = new Vector(0.6f, 0.2f);
            BackColor = Color.Black.SetAlpha(100);

            var nameFont = Content.Fonts.FancyFont;
            var labelLength = Size.X - SpriteBoxSize - Padding;

            nameLabel = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(SpriteBoxSize, largePadding),
                Size = new Vector(labelLength, nameFont.UiHeight),
                AutoSize = false,

                //BackColor = Color.Green,

                Font = nameFont,
                Text = "Manqche",
                TextColor = Color.White,
                TextXAlign = 0.5f,
            };

            var xpFont = Content.Fonts.SmallFont;
            xpLabel = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(SpriteBoxSize, nameLabel.Bottom + Padding),
                Size = new Vector(labelLength, xpFont.UiHeight),
                AutoSize = false,

                Font = Content.Fonts.SmallFont,
                Text = "Level ???",
                TextColor = Color.White,
                TextXAlign = 0.5f,

                //BackColor = Color.Blue,

                ToolTip = "???/???",
            };

            manaBar = new ManaBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Bottom | AnchorMode.Right,
                Location = new Vector(SpriteBoxSize, Size.Y - Padding - manaBarHeight),
                Size = new Vector(Size.X - SpriteBoxSize - Padding, manaBarHeight),

                ForeColor = Color.DarkBlue,
            };

            healthBar = new HealthBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Bottom | AnchorMode.Right,
                Location = new Vector(SpriteBoxSize, manaBar.Top - hpBarHeight),
                Size = new Vector(manaBar.Size.X, hpBarHeight),

                ForeColor = Color.DarkRed,
            };

            buffBar = new BuffBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Bottom | AnchorMode.Right,
                Location = new Vector(0, Size.Y),
                Size = new Vector(Size.X, 0.15f),

                BackColor = Color.Transparent,
                CanHover = false,
            };


            Add(xpLabel);
            Add(nameLabel);
            Add(manaBar);
            Add(healthBar);
            Add(buffBar);
        }

        protected override void OnUpdate(int msElapsed)
        {
            var unitTarget = Target?.Unit;

            IsVisible = (unitTarget != null);

            healthBar.Target = unitTarget;
            manaBar.Target = unitTarget;
            buffBar.Target = unitTarget;

            nameLabel.Text = unitTarget?.Name;
            xpLabel.Text = "Level {0}".F(unitTarget?.Level ?? 0);

            var heroTarget = unitTarget as IO.Objects.IHero;
            if (heroTarget != null)
                xpLabel.ToolTip = "{0}/{1} XP".F(heroTarget.Experience, heroTarget.ExperienceNeeded);
            else
                xpLabel.ToolTip = string.Empty;

        }

        public override void OnDraw(Graphics g)
        {
            //background
            base.OnDraw(g);

            //unit model
            if (Target != null)
                g.Draw(Target.Sprite, new Vector(largePadding), new Vector(SpriteSize));
        }
    }
}
