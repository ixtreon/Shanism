using Shanism.Client.Input;
using Shanism.Client.UI.Game;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Systems
{
    class ActionSystem : ClientSystem
    {

        FloatingTextProvider ErrorTextProvider => Interface.FloatingText;

        readonly UiSystem Interface;
        readonly SpriteSystem Objects;

        public IHero Hero { get; set; }


        public ActionSystem(UiSystem ui, SpriteSystem objects)
        {
            Interface = ui;
            Interface.actions = this;
            Objects = objects;
        }


        public override void Update(int msElapsed)
        {
            if (Hero == null)
                return;


            //cast only if there's an ability
            var ab = Interface.CurrentSpellButton?.Ability;
            if (ab == null)
            {
                ClientState.ActionId = 0;
                return;
            }

            //cast only if rightdown or if instacast
            if (!MouseInfo.RightDown && ab.TargetType != AbilityTargetType.NoTarget)
            {
                ClientState.ActionId = 0;
                return;
            }

            //cast only if not passive
            if (ab.TargetType == AbilityTargetType.Passive)
            {
                ClientState.ActionId = 0;
                return;
            }

            //instacasts are spammed until server registers it
            if (ab.TargetType == AbilityTargetType.NoTarget
                && ab.CurrentCooldown > 0)
            {
                ClientState.ActionId = 0;

                if (Interface.PreviousSpellButton != null)
                    Interface.PreviousSpellButton.IsSelected = true;
                else
                    Interface.CurrentSpellButton.IsSelected = false;
                return;
            }

            CastAbility(ab);
        }

        public void CastAbility(IAbility ab)
        {
            var targetLoc = getCastTargetLocation(ab);

            //cooldown
            if (ab.CurrentCooldown > 0)
            {
                if (MouseInfo.RightJustPressed)
                    Interface.FloatingText.AddLabel(targetLoc, $"{ab.CurrentCooldown / 1000.0:0.0} sec!", Color.Red, FloatingTextStyle.Top);
                return;
            }

            //target
            if (ab.TargetType != AbilityTargetType.NoTarget
                && targetLoc.DistanceTo(Hero.Position) > ab.CastRange)
            {
                if (MouseInfo.RightJustPressed)
                {
                    Interface.FloatingText.AddLabel(targetLoc, "Out of range", Color.Red, FloatingTextStyle.Top);
                    Interface.RangeIndicator.Show(ab.CastRange, 1250);
                }
                return;
            }

            //mana
            if (Hero.Mana < ab.ManaCost)
            {
                if (MouseInfo.RightJustPressed)
                {
                    Interface.FloatingText.AddLabel(targetLoc, "Not enough mana", Color.Red, FloatingTextStyle.Top);
                }
                return;
            }


            //cast abilities if button is held
            ClientState.ActionId = ab.Id;
            ClientState.ActionTargetId = Objects.HoverSprite?.Entity.Id ?? 0;
            ClientState.ActionTargetLocation = targetLoc;
        }

        Vector getCastTargetLocation(IAbility ab)
        {
            var mousePos = MouseInfo.InGamePosition;
            if (!Settings.Current.ExtendCast)
                return mousePos;

            var castRange = ab.CastRange;
            var heroPos = Objects.MainHero.Position;
            var distVector = mousePos - heroPos;
            var distScalar = distVector.Length();
            if (distScalar <= castRange)
                return mousePos;

            castRange *= 0.95;
            return heroPos + distVector * (castRange / distScalar);
        }

    }
}
