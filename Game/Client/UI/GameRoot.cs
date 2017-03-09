using Shanism.Client.UI.Chat;
using Shanism.Client.UI.Game;
using Shanism.Client.UI.Menus;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Input;

namespace Shanism.Client.UI
{
    class GameRoot : RootControl
    {
        public readonly UnitFrame HeroFrame;
        public readonly CastBar HeroCastBar;
        public readonly SpellBar HeroAbilities;
        public readonly UnitFrame TargetFrame;
        public readonly UnitHoverFrame HoverFrame;
        public readonly MenuBar Menus;
        public readonly ChatBar ChatBar;
        public readonly ChatFrame ChatBox;

        readonly ErrorTextControl errors;
        public readonly FloatingTextProvider FloatingText;
        public readonly RangeIndicator RangeIndicator;

        public event Action<string> ChatSent;

        public event Action<IAbility> AbilityActivated;

        public GameRoot()
        {
            Size = new Vector(2, 1);
            CanHover = true;
            CanFocus = true;
            GameActionActivated += onActionActivated;

            var castBarSize = new Vector(0.5f, 0.08f);
            var unitFrameXOffset = 0.25;
            var chatFont = Content.Fonts.NormalFont;

            /* add controls: order is important, unless ZValue is manually set. */

            //game indicators
            Add(FloatingText = new FloatingTextProvider());
            Add(RangeIndicator = new RangeIndicator());
            Add(errors = new ErrorTextControl());

            // hero, target & hover
            Add(HeroFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 - unitFrameXOffset - UnitBox.DefaultSize.X, 0),
            });
            Add(TargetFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 + unitFrameXOffset, 0),
            });
            Add(HoverFrame = new UnitHoverFrame(0.02, 0.02)
            {
                ParentAnchor = AnchorMode.Top,
            });

            Add(HeroAbilities = new SpellBar(0)
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(0.6, 0.8),
            });
            HeroAbilities.AbilityActivated += (a) => AbilityActivated?.Invoke(a);
            Add(HeroCastBar = new CastBar
            {
                ParentAnchor = AnchorMode.Bottom,

                Size = castBarSize,
                Location = new Vector(1 - castBarSize.X / 2, 0.55),
            });

            //chat
            var chatSize = new Vector(HeroAbilities.Size.X, Content.Fonts.NormalFont.HeightUi + 2 * Control.Padding);
            Add(ChatBar = new ChatBar
            {
                ParentAnchor = AnchorMode.Bottom,
                Font = chatFont,
                Size = chatSize,
                Location = HeroAbilities.Location - new Vector(0, chatSize.Y),
            });
            Add(ChatBox = new ChatFrame
            {
                Size = new Vector(chatSize.X, ChatFrame.DefaultSize.Y),
                Location = new Vector(2, 0) - new Vector(chatSize.X, 0),
                ParentAnchor = AnchorMode.Right | AnchorMode.Top,
            });
            ChatBox.SetProvider(ChatBar);
            ChatBar.ChatSent += (msg) => ChatSent?.Invoke(msg);

            //menus
            Add(Menus = new MenuBar(this));

            //tooltips
            Add(new Tooltips.SimpleTip());
            Add(new Tooltips.AbilityTip());
        }

        private void onActionActivated(ClientAction ga)
        {
            switch (ga)
            {
                case ClientAction.ShowHealthBars:
                    Settings.Current.AlwaysShowHealthBars = !Settings.Current.AlwaysShowHealthBars;
                    break;


                case ClientAction.Chat:
                    if (!ChatBar.HasFocus)
                        ChatBar.SetFocus();

                    break;

                default:
                    HeroAbilities.ActivateAction(ga);
                    Menus.ActivateAction(ga);
                    break;
            }
        }

        protected override void OnUpdate(int msElapsed)
        {
            Maximize();
            UpdateMain(msElapsed);
        }
    }
}
