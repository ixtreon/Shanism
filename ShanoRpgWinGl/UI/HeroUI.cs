using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShanoRpgWinGl.Sprites;

namespace ShanoRpgWinGl.UI
{
    /// <summary>
    /// Represents the in-game panel showing a hero's statistics. 
    /// </summary>
    class HeroUi : UserControl
    {
        public readonly IHero Hero;

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

        public readonly ValueLabel moveSpeed = new ValueLabel()
        {
            Text = "Speed: ",
        };

        public readonly ValueLabel armor = new ValueLabel()
        {
            Text = "Armor: ",
        };

        public readonly ValueLabel damage = new ValueLabel()
        {
            Text = "Damage: ",
        };


        public readonly ValueBar hpBar;

        public readonly ValueBar manaBar;

        public readonly ValueBar xpBar;

        public HeroUi(IHero h)
        {
            Size = new Vector2(1.2f, 0.45f);
            AbsolutePosition = new Vector2(0 - Size.X / 2, 1 - Size.Y);

            this.Hero = h;
            this.Locked = false;

            hpBar = new ValueBar()
            {
                Size = new Vector2(Size.X - 2 * Anchor, 0.05f),
                RelativePosition = new Vector2(Anchor, Anchor),
                ForeColor = Color.DarkRed,
                ClickThrough = true,
                ShowText = true,
            };
            manaBar = new ValueBar()
            {
                Size = hpBar.Size,
                RelativePosition = new Vector2(Anchor, hpBar.Bottom),
                ForeColor = Color.DarkBlue,
                ClickThrough = true,
                ShowText = true,
            };
            xpBar = new ValueBar()
            {
                Size = hpBar.Size,
                RelativePosition = new Vector2(Anchor, manaBar.Bottom),
                ForeColor = Color.Goldenrod,
                ClickThrough = true,
                ShowText = true,
            };

            strength.RelativePosition = new Vector2(Anchor, manaBar.Bottom + Anchor);
            agility.RelativePosition = new Vector2(Anchor, strength.Bottom);
            intellect.RelativePosition = new Vector2(Anchor, agility.Bottom);
            vitality.RelativePosition = new Vector2(Anchor, intellect.Bottom);

            damage.RelativePosition = new Vector2(strength.Right + Anchor, strength.RelativePosition.Y);
            armor.RelativePosition = new Vector2(damage.RelativePosition.X, damage.Bottom + Anchor * 3);
            moveSpeed.RelativePosition = new Vector2(damage.RelativePosition.X, armor.Bottom);

            this.Add(moveSpeed);
            this.Add(armor);
            this.Add(damage);

            this.Add(strength);
            this.Add(agility);
            this.Add(intellect);
            this.Add(vitality);

            this.Add(hpBar);
            this.Add(manaBar);
        }

        public override void Update(int msElapsed)
        {
            //update the displayed stats
            strength.Value = Hero.Strength.ToString("0");
            strength.TooltipText = "Your current strength. ";
            agility.Value = Hero.Agility.ToString("0");
            agility.TooltipText = "Your current agility. ";
            intellect.Value = Hero.Intellect.ToString("0");
            intellect.TooltipText = "Your current intellect. ";
            vitality.Value = Hero.Vitality.ToString("0");
            vitality.TooltipText = "Your current vitality. ";

            moveSpeed.Value = Hero.MoveSpeed.ToString("0.0");

            armor.Value = Hero.Defense.ToString("0.0");

            damage.Value = Hero.MinDamage + "-" + Hero.MaxDamage;


            hpBar.Value = Hero.Life;
            hpBar.MaxValue = Hero.MaxLife;

            manaBar.Value = Hero.Mana;
            manaBar.MaxValue = Hero.MaxMana;

            base.Update(msElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var backColor = new Color(50, 50, 50, 200);
            SpriteCache.BlankTexture.Draw(spriteBatch, ScreenPosition, ScreenSize, backColor);

            base.Draw(spriteBatch);
        }

        
    }
}
