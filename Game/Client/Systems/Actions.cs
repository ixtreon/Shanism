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
        readonly ObjectSystem Objects;

        public IHero Hero { get; set; }


        public ActionSystem(Interface ui, ObjectSystem objects)
        {
            Interface = ui;
            Objects = objects;
        }


        public override void Update(int msElapsed)
        {

            //cast abilities if button is held
            if (MouseInfo.RightDown)
            {
                ActionMessage msg;
                if (tryCastAbility(Interface.CurrentAbility, MouseInfo.RightJustPressed, out msg))
                    SendMessage(msg);
            }
        }


        /// <summary>
        /// Makes some client-side checks before generating an <see cref="ActionMessage"/>
        /// for the activation of a given ability. 
        /// </summary>
        /// <param name="ab">The ab.</param>
        /// <param name="displayErrors">if set to <c>true</c> [display errors].</param>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        bool tryCastAbility(IAbility ab, bool displayErrors, out ActionMessage msg)
        {
            msg = null;
            if (ab == null || ab.TargetType == AbilityTargetType.Passive)
                return false;

            //get target object and ground location
            var targetGuid = Objects.MainHeroGuid;
            var targetLoc = MouseInfo.InGamePosition;


            //cooldown
            if (ab.CurrentCooldown > 0)
            {
                if (displayErrors)
                    Interface.FloatingText.AddLabel(targetLoc, $"{ab.CurrentCooldown / 1000.0:0.0} sec!", Color.Red, FloatingTextStyle.Top);
                return false;
            }

            //target
            if (ab.TargetType != AbilityTargetType.NoTarget 
                && targetLoc.DistanceTo(Hero.Position) > ab.CastRange)
            {
                if (displayErrors)
                {
                    Interface.FloatingText.AddLabel(targetLoc, "Out of range", Color.Red, FloatingTextStyle.Top);
                    Interface.RangeIndicator.Show(ab.CastRange);
                }
                return false;
            }

            //mana
            if (Hero.Mana < ab.ManaCost)
            {
                if (displayErrors)
                    Interface.FloatingText.AddLabel(targetLoc, "Not enough mana", Color.Red, FloatingTextStyle.Top);
                return false;
            }

            msg = new ActionMessage(ab.Id, targetGuid, targetLoc);
            return true;
        }

    }
}
