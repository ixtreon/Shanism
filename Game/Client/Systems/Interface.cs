using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Shanism.Client.Objects;
using Shanism.Client.Input;
using Shanism.Client.UI.Menus;
using Shanism.Common.Objects;
using Shanism.Client.UI.CombatText;
using Shanism.Client.UI.Chat;
using Shanism.Client.UI;

namespace Shanism.Client.Systems
{
    class UiSystem : ClientSystem
    {
        public readonly Control Root = new Control();

        readonly UnitFrame heroFrame;
        readonly UnitFrame targetFrame;
        readonly UnitHoverFrame hoverFrame;
        readonly SpellBar abilityBar;
        readonly MenuBar menus;
        readonly CastBar castBar;
        readonly ChatBar chatBar;
        readonly ChatBox chatBox;
        readonly BuffBar heroBuffBar;
        readonly ErrorTextControl errors;

        HeroControl _mainHeroControl;

        public FloatingTextProvider FloatingText { get; }

        public RangeIndicator RangeIndicator { get; }


        /// <summary>
        /// Gets or sets the HeroControl that represents the player's 
        /// current hero, if there is one. 
        /// </summary>
        public HeroControl MainHeroControl
        {
            get { return _mainHeroControl; }
            set
            {
                if (_mainHeroControl != value)
                {
                    _mainHeroControl = value;

                    // adds all abilities of the hero to the bar
                    abilityBar.Controls
                        .OfType<SpellButton>()
                        .Zip(MainHero.Abilities ?? Enumerable.Empty<IAbility>(),
                            (sb, a) => sb.Ability = a)
                        .ToArray();
                }
            }
        }

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


        public IHero MainHero => MainHeroControl?.Hero;

        public IAbility CurrentAbility => SpellBarButton.CurrentSpellButton?.Ability;


        public UiSystem(ObjectSystem objManager)
        {
            if (objManager == null) throw new ArgumentNullException(nameof(objManager));

            Root = new Control
            {
                Size = new Vector(2, 1),
                CanHover = false,
            };
            Root.GameActionActivated += onActionActivated;

            /* add controls: order is important, unless ZValue is manually set. */

            var castBarSize = new Vector(0.5f, 0.08f);
            var unitFrameXOffset = 0.25;
            var chatFont = Content.Fonts.NormalFont;

            //game indicators
            Root.Add(FloatingText = new FloatingTextProvider(objManager));
            Root.Add(RangeIndicator = new RangeIndicator());
            Root.Add(errors = new ErrorTextControl());

            //game controls
            Root.Add(heroFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 - unitFrameXOffset - UnitFrame.DefaultSize.X, 0),
            });
            Root.Add(targetFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 + unitFrameXOffset, 0),
            });
            Root.Add(hoverFrame = new UnitHoverFrame(0.02, 0.02)
            {
                ParentAnchor = AnchorMode.Top,
            });
            Root.Add(abilityBar = new SpellBar(0)
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(0.6, 0.8),
            });
            Root.Add(castBar = new CastBar
            {
                ParentAnchor = AnchorMode.Bottom,

                Size = castBarSize,
                Location = new Vector(1 - castBarSize.X / 2, 0.55),
            });

            //chat
            var chatSize = new Vector(abilityBar.Size.X, Content.Fonts.NormalFont.HeightUi + 2 * Control.Padding);
            Root.Add(chatBar = new ChatBar
            {
                ParentAnchor = AnchorMode.Bottom,
                Font = chatFont,
                Size = chatSize,
                Location = abilityBar.Location - new Vector(0, chatSize.Y),
            });
            Root.Add(chatBox = new ChatBox
            {
                Size = new Vector(chatSize.X, ChatBox.DefaultSize.Y),
                Location = new Vector(2, 0) - new Vector(chatSize.X, 0),
                ParentAnchor = AnchorMode.Right | AnchorMode.Top,
            });
            chatBox.SetProvider(chatBar);

            //menus
            Root.Add(menus = new MenuBar(Root));

            //tooltips
            Root.Add(new UI.Tooltips.SimpleTip());
            Root.Add(new UI.Tooltips.AbilityTip());
            
            Root.BringToFront();
            Root.Maximize();
        }


        public override void Update(int msElapsed)
        {
            Root.Maximize();

            Ticker.Default.Update(msElapsed);

            menus.OurHero = MainHero;

            castBar.Target = MainHero;
            //heroBuffBar.Target = MainHero;
            heroFrame.Target = MainHeroControl;
        }


        void onActionActivated(ClientAction ga)
        {
            switch (ga)
            {
                case ClientAction.Chat:
                    chatBar.SetFocus();
                    break;
                default:
                    abilityBar.ActivateAction(ga);
                    menus.ActivateAction(ga);
                    break;
            }
        }
    }
}
