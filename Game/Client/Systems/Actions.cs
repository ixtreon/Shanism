using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Input;
using Shanism.Client.UI;
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

        readonly Interface Interface;
        readonly SpriteSystem Objects;

        public IHero Hero { get; set; }


        public ActionSystem(Interface ui, SpriteSystem objects)
        {
            Interface = ui;
            Objects = objects;
        }


        public override void Update(int msElapsed)
        {
            if (Objects.MainHero == null)
                return;

            //cast abilities if button is held
            if (MouseInfo.RightDown
                && Interface.CurrentAbility != null
                && Interface.CurrentAbility.TargetType != AbilityTargetType.Passive)
            {
                ClientState.ActionId = Interface.CurrentAbility.Id;
                ClientState.ActionTargetId = Objects.HoverSprite?.Entity.Id ?? 0; 
                ClientState.ActionTargetLoc = getCastTargetLocation();
            }
            else
                ClientState.ActionId = 0;
        }

        Vector getCastTargetLocation()
        {
            var m = MouseInfo.InGamePosition;
            if (!Settings.Current.ExtendCast)
                return m;

            var r = Interface.CurrentAbility.CastRange;
            var o = Objects.MainHero.Position;
            var d = m - o;
            var l = d.Length();
            if (l <= r)
                return m;
            return o + d * (r / l);
        }

    }
}
