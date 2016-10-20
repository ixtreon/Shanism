using Shanism.Client.Input;
using Shanism.Client.UI.CombatText;
using Shanism.Common;
using Shanism.Common.Game;
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
            if (Objects.MainHero == null)
                return;

            //make some checks so we don't spam the server
            var ab = Interface.CurrentAbility;
            if (ab == null)
            {
                ClientState.ActionId = 0;
                return;
            }

            if (!MouseInfo.RightDown)
            {
                ClientState.ActionId = 0;
                return;
            }


            if (ab.TargetType == AbilityTargetType.Passive)
            {
                ClientState.ActionId = 0;
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
                Interface.FloatingText.AddLabel(targetLoc, "Out of range", Color.Red, FloatingTextStyle.Top);
                Interface.RangeIndicator.Show(ab.CastRange, 1250);
                return;
            }

            //mana
            if (Hero.Mana < ab.ManaCost)
            {
                Interface.FloatingText.AddLabel(targetLoc, "Not enough mana", Color.Red, FloatingTextStyle.Top);
                return;
            }


            //cast abilities if button is held
            ClientState.ActionId = ab.Id;
            ClientState.ActionTargetId = Objects.HoverSprite?.Entity.Id ?? 0;
            ClientState.ActionTargetLoc = targetLoc;
        }

        Vector getCastTargetLocation(IAbility ab)
        {
            var m = MouseInfo.InGamePosition;
            if (!Settings.Current.ExtendCast)
                return m;

            var r = ab.CastRange;
            var o = Objects.MainHero.Position;
            var d = m - o;
            var l = d.Length();
            if (l <= r)
                return m;
            return o + d * (r / l);
        }

    }
}
