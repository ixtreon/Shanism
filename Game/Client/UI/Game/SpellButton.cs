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
    /// <summary>
    /// A simple button that displays an ability's icon and a tooltip. 
    /// </summary>
    class SpellButton : Button
    {

        IAbility ability;

        /// <summary>
        /// The event raised whenever the ability on this button is changed. 
        /// </summary>
        public event Action AbilityChanged;

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

        public SpellButton()
        {
            HasBorder = true;
            CanDrag = true;

            OnAbilityChanged();
        }

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
                Texture = Content.Textures.TryGetIcon(ability.Icon) ?? Content.Textures.DefaultIcon;
                TextureColor = Color.White;
            }

            AbilityChanged?.Invoke();
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

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
