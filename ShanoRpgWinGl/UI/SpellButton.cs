using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IO;
using ShanoRpgWinGl.Sprites;

namespace ShanoRpgWinGl.UI
{
    class SpellButton : Button
    {
        private IAbility ability;

        public event Action AbilityChanged;

        private void OnAbilityChanged()
        {
            this.TooltipText =
                string.Format("{0}\n\nCooldown: {1}s\nMana: {2}\n\n{3}",
                ability.Name,
                ((double)Ability.Cooldown / 1000).ToString("0.0"),
                Ability.ManaCost,
                ability.Description);
            this.Texture = TextureCache.Get(TextureType.Icon, ability.Icon);

            if (AbilityChanged != null)
                AbilityChanged();
        }

        public IAbility Ability
        {
            get { return ability; }
            set
            {
                if (ability != value)
                {
                    ability = value;
                    OnAbilityChanged();
                }
            }
        }

        public SpellButton(Keys k = Keys.None, float sz = 0.12f)
        {
            this.Texture = SpriteCache.Icon.Nothing.Texture;
            this.Size = new Vector2(sz, sz);
            this.Keybind = k;
            this.HasBorder = true;

            this.MouseDown += onMouseDown;
        }

        private void onMouseDown(Vector2 p)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            var cooldown = (Ability != null) ? (ability.CurrentCooldown) : (0);

            if (cooldown > 0)
            {
                var cdHeight = ScreenSize.Y * cooldown / Ability.Cooldown;
                var cdPos = new Point(ScreenPosition.X, ScreenPosition.Y + ScreenSize.Y - cdHeight);
                SpriteCache.BlankTexture.Draw(sb, cdPos, new Point(ScreenSize.X, cdHeight), Color.Black.SetAlpha(120));
            }
        }
    }
}
