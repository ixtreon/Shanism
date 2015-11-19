using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IO;
using Client.Textures;
using Client.UI.Common;
using IO.Objects;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;

namespace Client.UI
{
    class SpellButton : Button
    {
        IAbility ability;

        public event Action AbilityChanged;

        /// <summary>
        /// Gets whether dragging from this spell button removes the spell in it. 
        /// </summary>
        public bool Sticky { get; set; } = false;

        void OnAbilityChanged()
        {
            if (ability == null)
            {
                ToolTip = string.Empty;
                Texture = Content.Textures.Blank;
                TextureColor = Color.Black;
            }
            else
            {
                ToolTip = ability;
                Texture = Content.Textures.TryGetIcon(ability.Icon);
                TextureColor = Color.White;
            }
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

        public SpellButton(Keys k = Keys.None, double sz = 0.12)
        {
            HasBorder = true;
            CanDrag = true;

            Keybind = k;
            Size = new Vector(sz, sz);

            OnDrop += SpellButton_OnDrop;

            OnAbilityChanged();
        }

        void SpellButton_OnDrop(Control src)
        {
            if (Sticky)
                return;

            var srcButton = src as SpellButton;
            if (srcButton?.Ability != null)
            {
                var oldAb = Ability;
                Ability = srcButton.Ability;
                if (!srcButton.Sticky)
                    srcButton.Ability = oldAb;
            }
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);

            var cooldown = ability?.CurrentCooldown ?? 0;

            if (cooldown > 0)
            {
                var cdSize = Size * new Vector(1, (double)cooldown / Ability.Cooldown);
                var cdPos = new Vector(0, Size.Y - cdSize.Y);

                g.Draw(Content.Textures.Blank, cdPos, cdSize, Color.Black.SetAlpha(120));
            }

            if(MouseOver && ability != null)
            {
                if(ability.CastRange > 0)
                {
                    //TODO: visuals on button hover
                }
            }
        }
    }
}
