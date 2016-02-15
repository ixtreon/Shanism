using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO;
using Client.Objects;
using Microsoft.Xna.Framework.Input;
using Client.Input;
using Client.UI.Menus;
using IO.Message.Client;
using IO.Objects;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    class UiManager : Control
    {

        readonly UnitFrame heroFrame;
        readonly UnitFrame targetFrame;
        readonly UnitHoverFrame hoverFrame;
        readonly SpellBar abilityBar;
        readonly MenuBar menus;
        readonly CastBar castBar;
        readonly ChatBox chatBox;
        readonly BuffBar heroBuffBar;

        HeroControl _mainHeroControl;

        /// <summary>
        /// Gets or sets the HeroControl that represents the player's 
        /// current hero, if there is one. 
        /// </summary>
        public HeroControl MainHeroControl
        {
            get { return _mainHeroControl; }
            set
            {
                if(_mainHeroControl != value)
                {
                    _mainHeroControl = value;

                    abilityBar.Controls
                        .OfType<SpellButton>()
                        .Zip(MainHero.Abilities,
                            (sb, a) => sb.Ability = a)
                        .ToArray();
                }
            }
        }

        public IHero MainHero {  get { return MainHeroControl?.Hero; } }

        /// <summary>
        /// Gets or sets the current target of the player. 
        /// </summary>
        public UnitControl Target
        {
            get { return targetFrame.Target; }
            set { targetFrame.Target = value; }
        }

        /// <summary>
        /// Gets or sets the current hover of the player. 
        /// </summary>
        public UnitControl Hover
        {
            get { return hoverFrame.Target; }
            set { hoverFrame.Target = value; }
        }

        public UiManager()
        {
            Size = new Vector(2, 1);
            CanHover = false;

            var castBarSize = new Vector(0.5f, 0.08f);

            heroFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 - 0.25 - 0.6, 0),
            };
            targetFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 + 0.25, 0),
            };
            hoverFrame = new UnitHoverFrame
            {
                ParentAnchor = AnchorMode.Top,
            };

            abilityBar = new SpellBar(0)
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(0.6, 0.8),
            };
            chatBox = new ChatBox();
            castBar = new CastBar
            {
                BackColor = Color.Pink,
                Size = castBarSize,
                Location = new Vector(-castBarSize.X / 2, 0.7),
            };
            heroBuffBar = new BuffBar
            {
                AbsolutePosition = new Vector(0, 0),
            };
            menus = new MenuBar(this);

            //controls
            Add(heroFrame);
            Add(targetFrame);
            Add(hoverFrame);
            Add(abilityBar);
            //Add(chatBox);
            Add(castBar);
            //Add(HeroBuffs);
            Add(menus);

            //tooltips
            this.Add(new Tooltips.SimpleTip());
            this.Add(new Tooltips.AbilityTip());
        }

        public IAbility CurrentAbility
        {
            get { return SpellBarButton.CurrentSpellButton?.Ability; }
        }

        protected override void OnUpdate(int msElapsed)
        {
            menus.OurHero = MainHero;

            castBar.Target = MainHero;
            heroBuffBar.Target = MainHero;
            heroFrame.Target = MainHeroControl;

            Ticker.Default.Update(msElapsed);
        }
    }
}
