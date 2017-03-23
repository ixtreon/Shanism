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
using Shanism.Common.Message;
using Shanism.Client.Systems;
using Shanism.Client.Sprites;

namespace Shanism.Client.UI
{
    partial class GameRoot : RootControl
    {
        public UnitFrame HeroFrame { get; private set; }
        public CastBar HeroCastBar { get; private set; }
        public SpellBar HeroAbilities { get; private set; }
        public UnitFrame TargetFrame { get; private set; }
        public UnitHoverFrame HoverFrame { get; private set; }
        public MenuBar Menus { get; private set; }
        public ChatBar ChatBar { get; private set; }
        public ChatFrame ChatBox { get; private set; }
        public ErrorTextControl Errors { get; private set; }
        public FloatingTextProvider FloatingText { get; private set; }
        public RangeIndicator RangeIndicator { get; private set; }

        public event Action<IAbility> AbilityActivated;

        IChatSource serverChatSource;
        IChatConsumer serverChatEndpoint;

        SpriteSystem objects;

        public GameRoot()
        {
            Size = new Vector(2, 1);
            CanHover = true;
            CanFocus = true;
            GameActionActivated += onActionActivated;
        }

        public void Init(IChatSource chatSource, IChatConsumer chatConsumer, SpriteSystem objects)
        {
            this.objects = objects;
            serverChatSource = chatSource;
            serverChatEndpoint = chatConsumer;

            ReloadUi();
        }

        private void onActionActivated(ClientAction ga)
        {
            switch (ga)
            {
                case ClientAction.ShowHealthBars:
                    Settings.Current.AlwaysShowHealthBars ^= true;
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

        UnitSprite _curHeroSprite;

        protected override void OnUpdate(int msElapsed)
        {
            Maximize();
            UpdateMain(msElapsed);

            //update hero pointer
            if (objects.MainHeroSprite != _curHeroSprite)
            {
                Menus.OurHero = objects.MainHero;
                HeroCastBar.Target = objects.MainHero;
                HeroFrame.TargetSprite = objects.MainHeroSprite;

                _curHeroSprite = objects.MainHeroSprite;

                // adds all abilities of the hero to the default bar
                var abilities = objects.MainHero?.Abilities ?? Enumerable.Empty<IAbility>();
                HeroAbilities.SetAbilities(abilities);
                HeroAbilities.SelectButton(0);
            }

            //update target & hover ccontrols
            if (HasHover)
            {
                var unitHover = objects.HoverSprite as UnitSprite;
                HoverFrame.Target = unitHover;

                if (MouseInfo.LeftJustPressed)
                    TargetFrame.TargetSprite = unitHover;
            }
        }

        public void HandleMessage(IOMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
