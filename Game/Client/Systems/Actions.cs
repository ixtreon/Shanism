using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Client.UI;
using Shanism.Client.Objects;
using Microsoft.Xna.Framework.Input;
using Shanism.Common.Message.Client;
using Shanism.Common.Objects;
using Shanism.Client.UI.CombatText;
using Shanism.Common.Game;
using Microsoft.Xna.Framework;

namespace Shanism.Client.Systems
{
    class ActionSystem : ClientSystem
    {

        FloatingTextProvider ErrorTextProvider => Interface.FloatingText;

        readonly UiSystem Interface;


        MouseState mouseState;
        MouseState oldMouseState = Mouse.GetState();

        public IHero Hero { get; set; }



        public ActionSystem(UiSystem ui)
        {
            Interface = ui;
        }


        public override void Update(int msElapsed)
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            //cast abilities if button is held
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                var justPressedKey = (oldMouseState.RightButton == ButtonState.Released);

                ActionMessage msg;
                if (tryCastAbility(Interface.CurrentAbility, justPressedKey, out msg))
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
            if (ab == null)
                return false;

            //get target object and ground location
            var targetGuid = (Control.HoverControl as ObjectControl)?.Object.Id ?? 0;
            var targetLoc = Screen.ScreenToGame(mouseState.Position.ToPoint());


            //make some checks so we don't spam the server

            //cooldown
            var justClicked = mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
            if (ab.CurrentCooldown > 0)
            {
                if (displayErrors && justClicked)
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
                    Interface.RangeIndicator.ShowRange(ab.CastRange, 1250, true);
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
