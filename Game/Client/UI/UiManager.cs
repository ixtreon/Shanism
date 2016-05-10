using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Shanism.Client.Objects;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Input;
using Shanism.Client.UI.Menus;
using Shanism.Common.Message.Client;
using Shanism.Common.Objects;
using Shanism.Common.Game;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Client.UI.CombatText;
using Shanism.Client.UI.Chat;

namespace Shanism.Client.UI
{
    class UiManager : Control
    {
        public FloatingTextProvider FloatingText { get; } = new FloatingTextProvider();
        public RangeIndicator RangeIndicator { get; } = new RangeIndicator();


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


        public UiManager()
        {
            Size = new Vector(2, 1);
            CanHover = false;
            GameActionActivated += onActionActivated;

            var unitFrameXOffset = 0.25;
            heroFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 - unitFrameXOffset - UnitFrame.DefaultSize.X, 0),
            };
            targetFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 + unitFrameXOffset, 0),
            };

            hoverFrame = new UnitHoverFrame(0.02, 0.02)
            {
                ParentAnchor = AnchorMode.Top,
            };

            abilityBar = new SpellBar(0)
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(0.6, 0.8),
            };

            var chatFont = Content.Fonts.NormalFont;
            var chatSize = new Vector(abilityBar.Size.X, Content.Fonts.NormalFont.HeightUi + 2 * Padding);
            chatBar = new ChatBar()
            {
                ParentAnchor = AnchorMode.Bottom,
                Font = chatFont,
                Size = chatSize,
                Location = abilityBar.Location - new Vector(0, chatSize.Y),
            };
            chatBox = new ChatBox
            {
                Size = new Vector(chatSize.X, ChatBox.DefaultSize.Y),
                Location = new Vector(2, 0) - new Vector(chatSize.X, 0),
                ParentAnchor = AnchorMode.Right | AnchorMode.Top,
            };
            chatBox.SetProvider(chatBar);

            var castBarSize = new Vector(0.5f, 0.08f);
            castBar = new CastBar
            {
                ParentAnchor = AnchorMode.Bottom,

                Size = castBarSize,
                Location = new Vector(1 - castBarSize.X / 2, 0.55),
            };
            heroBuffBar = new BuffBar
            {
                AbsolutePosition = new Vector(0, 0),
            };

            /* add controls: order is important, unless ZValue is manually set. */

            //game indicators
            Add(FloatingText);
            Add(RangeIndicator);
            Add((errors = new ErrorTextControl()));

            //game controls
            Add(heroFrame);
            Add(targetFrame);
            Add(hoverFrame);
            Add(abilityBar);
            Add(castBar);

            //chat
            Add(chatBar);
            Add(chatBox);

            //menus
            Add((menus = new MenuBar(this)));

            //tooltips
            Add(new Tooltips.SimpleTip());
            Add(new Tooltips.AbilityTip());

            Maximize();
        }

        void onActionActivated(GameAction ga)
        {
            switch (ga)
            {
                case GameAction.Chat:
                    chatBar.SetFocus();
                    break;
                default:
                    abilityBar.ActivateAction(ga);
                    menus.ActivateAction(ga);
                    break;
            }
        }

        public void DisplayError(string msg)
        {
            errors.LogError(msg);
        }

        public IAbility CurrentAbility => SpellBarButton.CurrentSpellButton?.Ability;

        protected override void OnUpdate(int msElapsed)
        {
            Maximize();

            Ticker.Default.Update(msElapsed);

            menus.OurHero = MainHero;

            castBar.Target = MainHero;
            heroBuffBar.Target = MainHero;
            heroFrame.Target = MainHeroControl;
        }
    }
}
