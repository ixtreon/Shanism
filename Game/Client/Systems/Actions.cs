using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Client.UI.CombatText;
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
            //cast abilities if button is held
            IAbility ab;
            if (MouseInfo.RightDown
                && (ab = Interface.CurrentAbility) != null)
            {
                ClientState.ActionId = ab.Id;
                ClientState.ActionTargetId = Objects.HoverSprite?.Entity.Id ?? 0;        //TODO: implement 
                ClientState.ActionTargetLoc = MouseInfo.InGamePosition;
            }
            else
                ClientState.ActionId = 0;
        }

    }
}
