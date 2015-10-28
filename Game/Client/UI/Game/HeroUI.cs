using Microsoft.Xna.Framework.Graphics;
using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Sprites;
using Client.UI.Common;
using IO.Objects;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    /// <summary>
    /// Represents the in-game panel showing a hero's statistics. 
    /// </summary>
    class HeroUi : Control
    {
        public IHero Target { get; set; }

        public readonly ValueLabel strength = new ValueLabel()
        {
            Text = "Strength: ",
        };

        public readonly ValueLabel agility = new ValueLabel()
        {
            Text = "Agility: ",
        };

        public readonly ValueLabel intellect = new ValueLabel()
        {
            Text = "Intelligence: ",
        };

        public readonly ValueLabel vitality = new ValueLabel()
        {
            Text = "Vitality: ",
        };

        public readonly ValueLabel moveSpeed = new ValueLabel(true)
        {
            Text = "MS",
        };

        public readonly ValueLabel armor = new ValueLabel(true)
        {
            Text = "DEF",
        };

        public readonly ValueLabel damage = new ValueLabel(true)
        {
            Text = "DMG",
        };

        public readonly ValueLabel magic = new ValueLabel(true)
        {
            Text = "MGC",
        };


        public readonly HealthBar hpBar;

        public readonly ManaBar manaBar;

        public readonly XpBar xpBar;

        public HeroUi()
        {
            Size = new Vector(1.4f, 0.16f);
            AbsolutePosition = new Vector(0 - Size.X / 2, 1 - Size.Y);

            this.Locked = false;

            xpBar = new XpBar()
            {
                Size = new Vector(Size.X - 2 * Anchor, 3 * Anchor),
                RelativePosition = new Vector(Anchor, 0),
            };

            //strength.RelativePosition = new Vector(Anchor, manaBar.Bottom + Anchor);
            //agility.RelativePosition = new Vector(Anchor, strength.Bottom);
            //intellect.RelativePosition = new Vector(Anchor, agility.Bottom);
            //vitality.RelativePosition = new Vector(Anchor, intellect.Bottom);
            
            var lblTop = xpBar.Bottom + Anchor;
            var lblSize = new Vector(0.12f, Size.Y - lblTop - Anchor);

            damage = new ValueLabel(true)
            {
                Text = "DMG",
                RelativePosition = new Vector(Anchor, lblTop),
                Size = lblSize,
            };

            magic = new ValueLabel(true)
            {
                Text = "MGC",
                RelativePosition = new Vector(damage.Right, lblTop),
                Size = lblSize,
            };

            var r = magic.Right;
            var barHeight = 5 * Anchor;

            hpBar = new HealthBar()
            {
                Size = new Vector(Size.X - 2 * r, barHeight),
                RelativePosition = new Vector(r, lblTop + damage.Size.Y / 2 - barHeight),
                ColorStyle = BarColorStyle.Fixed,
            };

            manaBar = new ManaBar()
            {
                Size = hpBar.Size,
                RelativePosition = new Vector(r, hpBar.Bottom),
                ColorStyle = BarColorStyle.Value,
            };

            armor = new ValueLabel(true)
            {
                Text = "DEF",
                RelativePosition = new Vector(hpBar.Right + Anchor, lblTop),
                Size = lblSize,
            };
            moveSpeed = new ValueLabel(true)
            {
                Text = "MS",
                RelativePosition = new Vector(armor.Right + Anchor, lblTop),
                Size = lblSize,
            };

            //armor.RelativePosition = new Vector(damage.Right + Anchor, strength.RelativePosition.Y);
            //moveSpeed.RelativePosition = new Vector(armor.RelativePosition.X, armor.Bottom);

            this.Add(damage);
            this.Add(magic);
            this.Add(moveSpeed);
            this.Add(armor);

            //this.Add(strength);
            //this.Add(agility);
            //this.Add(intellect);
            //this.Add(vitality);

            this.Add(hpBar);
            this.Add(manaBar);
            this.Add(xpBar);
        }

        /// <summary>
        /// Updates the values of the various children controls. 
        /// </summary>
        public override void Update(int msElapsed)
        {
            this.Visible = (Target != null);

            if (Visible)
            {
                xpBar.Target = Target;
                hpBar.Target = Target;
                manaBar.Target = Target;

                armor.Value = Target.Defense.ToString("0");
                armor.TooltipText = "({0:0.0} + {1:0.0}) defense provides {2}% reduction in physical damage.".Format(Target.BaseDefense, Target.Defense - Target.BaseDefense, "???");

                moveSpeed.Value = Target.MoveSpeed.ToString("0");
                moveSpeed.TooltipText = "{0} tiles per second.".Format(Target.MoveSpeed);

                var attacksPerSec = (double)Target.AttackCooldown / 1000;
                damage.Value = ((Target.MinDamage + Target.MaxDamage) / 2).ToString("0");
                damage.TooltipText = "{0}-{1} damage every {2:0.0} seconds.".Format(Target.MinDamage, Target.MaxDamage, (1000.0 / Target.AttackCooldown));

                magic.Value = Target.MagicDamage.ToString("0");
                magic.TooltipText = "{0} base magic damage.".Format(Target.MoveSpeed);

                manaBar.Value = Target.Mana;
                manaBar.MaxValue = Target.MaxMana;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var backColor = new Color(50, 50, 50, 200);
            SpriteFactory.Blank.DrawScreen(spriteBatch, ScreenPosition + new Point(0, ScreenAnchor * 3 / 2), ScreenSize, backColor);
        }

        
    }
}
