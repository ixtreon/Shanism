using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;

namespace Shanism.Client.UI
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
                Texture = Content.Icons.Default;    //TODO: change to empty
                TextureColor = Color.Black;
            }
            else
            {
                ToolTip = ability;
                Texture = Content.Icons.TryGet(ability.Icon) ?? Content.Icons.Default;
                TextureColor = ability.IconTint;
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

            if(HasHover && ability != null)
            {
                if(ability.CastRange > 0)
                {
                    //TODO: visuals on button hover
                }
            }
        }
    }
}
