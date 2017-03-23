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

        string currentTexture = string.Empty;


        /// <summary>
        /// Gets or sets whether this spell button has a border drawn around it. 
        /// </summary>
        public bool HasBorder { get; set; }

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
            AbilityChanged?.Invoke();
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (ability != null)
            {
                ToolTip = ability;
                if (ability.Icon != currentTexture)
                {
                    Texture = Content.Icons.TryGet(ability.Icon) ?? Content.Icons.Default;
                    currentTexture = ability.Icon;
                }

                TextureColor = ability.IconTint;
            }
            else
            {
                ToolTip = null;
                Texture = Content.Icons.Default;
                TextureColor = Color.Black;
            }
        }

        public override void OnDraw(Canvas g)
        {
            base.OnDraw(g);

            //cooldown indicator
            var cooldown = ability?.CurrentCooldown ?? 0;
            if (cooldown > 0)
            {
                var cdSize = Size * new Vector(1, (double)cooldown / Ability.Cooldown);
                var cdPos = new Vector(0, Size.Y - cdSize.Y);

                g.Draw(Content.Textures.Blank, cdPos, cdSize, Color.Black.SetAlpha(120));
            }

            //TODO: visuals on button hover
            if(HasHover && ability != null)
            {
                if(ability.CastRange > 0)
                {
                }
            }

            //button border
            if(HasBorder)
            {
                var border = Content.UI.IconBorderHover;
                var tint = HasHover ? Color.White : new Color(32, 32, 32);
                tint = (CanSelect && IsSelected) ? Color.Gold.Brighten(20) : tint;

                g.Draw(border, Vector.Zero, Size, tint);
            }
        }
    }
}
