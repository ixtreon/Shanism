using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using Shanism.Client.Input;
using Shanism.Client.UI.Game;
using Shanism.Client.UI.Chat;
using Shanism.Client.UI;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using Shanism.Client.Drawing;
using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// Holds all elements of the default user interface. 
    /// </summary>
    /// <seealso cref="Shanism.Client.Systems.ClientSystem" />
    class UiSystem : ClientSystem
    {
        readonly GameRoot root;

        readonly SpriteSystem objects;

        UnitSprite _curHeroSprite;

        public FloatingTextProvider FloatingText => root.FloatingText;

        public RangeIndicator RangeIndicator => root.RangeIndicator;

        public SpellButton CurrentSpellButton => SpellBarButton.CurrentSpellButton;
        public SpellButton PreviousSpellButton => SpellBarButton.PreviousSpellButton;

        public Control Root => root;

        readonly GraphicsDevice device;
        readonly Graphics g;

        public ActionSystem actions;

        public UiSystem(GraphicsDevice device, SpriteSystem objects, IChatProvider chatProvider)
        {
            this.device = device;
            this.objects = objects;
            g = new Graphics(device);

            root = new GameRoot();
            root.AbilityActivated += onAbilityActivated;
            root.ChatBox.SetProvider(chatProvider);
            root.ChatBar.ChatSent += (m) =>
                SendMessage(new Common.Message.Client.ChatMessage(string.Empty, m));
        }

        void onAbilityActivated(IAbility a)
        {
            actions.CastAbility(a);
        }

        // adds all abilities of the hero to the default bar
        void updateBarAbilities(IHero h)
        {
            var abilities = h.Abilities ?? Enumerable.Empty<IAbility>();
            root.HeroAbilities.SetAbilities(abilities);
            root.HeroAbilities.SelectButton(0);
                
        }


        public void Draw()
        {
            g.Begin();

            Root.Draw(g);

            g.End();
        }

        public override void Update(int msElapsed)
        {
            g.Bounds = new RectangleF(Vector.Zero, Screen.UiSize);

            root.Update(msElapsed);

            //update hero pointer
            if (objects.MainHeroSprite != _curHeroSprite)
            {
                root.Menus.OurHero = objects.MainHero;
                root.HeroCastBar.Target = objects.MainHero;
                root.HeroFrame.TargetSprite = objects.MainHeroSprite;

                _curHeroSprite = objects.MainHeroSprite;

                updateBarAbilities(objects.MainHero);
            }

            //update target & hover ccontrols
            if (Control.HoverControl.IsRootControl)
            {
                var unitHover = objects.HoverSprite as UnitSprite;
                root.HoverFrame.Target = unitHover;

                if (MouseInfo.LeftJustPressed)
                    root.TargetFrame.TargetSprite = unitHover;
            }
        }

        public override void HandleMessage(IOMessage ioMsg)
        {
            switch (ioMsg.Type)
            {
                case MessageType.DamageEvent:

                    var dmgEv = (DamageEventMessage)ioMsg;
                    var unit = objects.TryGet(dmgEv.UnitId);
                    if (unit == null)
                        return;

                    var text = dmgEv.ValueChange.ToString("0");
                    FloatingText.AddLabel(unit.Position, text, Color.Red, FloatingTextStyle.Rainbow);
                    break;
            }
        }
    }
}
