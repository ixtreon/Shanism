using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Objects;
using Shanism.Client.Textures;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Client.UI.Common;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Common.Objects;

namespace Shanism.Client.UI
{
    /// <summary>
    /// Displays information about a given unit such as its name, level, life and mana. 
    /// </summary>
    class UnitFrame : Control
    {
        public static readonly Vector DefaultSize = new Vector(0.6f, 0.2f);

        const double largePadding = Padding * 2;

        const double hpBarHeight = Padding * 4;
        const double manaBarHeight = Padding * 3;

        public UnitControl Target;


        readonly HealthBar healthBar;
        readonly ManaBar manaBar;
        readonly BuffBar buffBar;
        readonly Label nameLabel;
        readonly Label lvlLabel;

        /// <summary>
        /// The size of the unit sprite that is drawn. 
        /// </summary>
        double SpriteSize =>  Size.Y - 2 * largePadding;

        /// <summary>
        /// The size of the sprite + the border around it. 
        /// </summary>
        double SpriteBoxSize => SpriteSize + 2 * largePadding;


        public UnitFrame()
        {
            Size = DefaultSize;
            BackColor = Color.Black.SetAlpha(100);

            var nameFont = Content.Fonts.FancyFont;
            var labelLength = Size.X - SpriteBoxSize - Padding;

            nameLabel = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(SpriteBoxSize, largePadding),
                Size = new Vector(labelLength, nameFont.HeightUi),
                AutoSize = false,

                //BackColor = Color.Green,

                Font = nameFont,
                Text = "Manqche",
                TextColor = Color.White,
                TextXAlign = 0.5f,
            };

            var lvlFont = Content.Fonts.SmallFont;
            lvlLabel = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(SpriteBoxSize, nameLabel.Bottom + Padding),
                Size = new Vector(labelLength, lvlFont.HeightUi),
                AutoSize = false,

                Font = Content.Fonts.SmallFont,
                Text = "Level ???",
                TextColor = Color.White,
                TextXAlign = 0.5f,
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


            Add(lvlLabel);
            Add(nameLabel);
            Add(manaBar);
            Add(healthBar);
            Add(buffBar);
        }

        protected override void OnUpdate(int msElapsed)
        {
            var unitTarget = Target?.Unit;

            IsVisible = (unitTarget != null);

            if (IsVisible)
            {
                healthBar.Target = unitTarget;
                manaBar.Target = unitTarget;
                buffBar.Target = unitTarget;

                nameLabel.Text = unitTarget?.Name;
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
            //background
            base.OnDraw(g);

            //unit model
            if (Target != null)
                g.Draw(Target.Sprite, new Vector(largePadding), new Vector(SpriteSize));
        }
    }
}
