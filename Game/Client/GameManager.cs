using Client.Input;
using Client.Objects;
using Client.UI;
using Client.UI.CombatText;
using IO.Common;
using IO.Message;
using IO.Message.Client;
using IO.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

    // A top level control, contains uimanager, objectmanager
    // could be extended to containt mapmanager, too, even tho its not a control
    class GameManager : Control
    {
        /// <summary>
        /// The main UI window. 
        /// </summary>
        public UiManager Interface { get; private set; }

        /// <summary>
        /// The guy that handles objects. 
        /// </summary>
        public ObjectManager Objects { get { return ObjectManager.Default; } }


        public FloatingTextProvider FloatingText => Interface.FloatingText;


        public event Action<ActionMessage> ActionPerformed;

        public GameManager()
        {
            AbsolutePosition = new Vector(0);
            CanFocus = true;
            GameActionActivated += onActionActivated;

            Interface = new UiManager();

            Objects.ObjectClicked += onObjectClicked;
            Objects.TerrainClicked += onGroundClicked;

            Add(Interface);
            Add(Objects);

            Interface.BringToFront();
        }

        public void ReloadUi()
        {
            Remove(Interface);
            Interface = new UiManager();
            Add(Interface);
        }

        void onActionActivated(GameAction ga)
        {
            switch(ga)
            {
                case GameAction.ReloadUi:
                    ReloadUi();
                    break;

                case GameAction.ShowHealthBars:
                    ShanoSettings.Current.AlwaysShowHealthBars = !ShanoSettings.Current.AlwaysShowHealthBars;
                    break;

                default:
                    //propagate to both interface and objects, let them handle it
                    Interface.ActivateAction(ga);
                    Objects.ActivateAction(ga);
                    break;
            }
        }

        void onGroundClicked(MouseButtonArgs e)
        {
            //clear target
            if (e.Button == MouseButton.Left)
                Interface.Target = null;
        }

        void onObjectClicked(MouseButtonArgs e)
        {
            //target a unit or nothing
            if (e.Button == MouseButton.Left)
                Interface.Target = e.Control as UnitControl;
        }

        protected override void OnUpdate(int msElapsed)
        {
            Interface.Maximize();
            Objects.Maximize();

            // update the interface's main hero from the objects' main hero. 
            Interface.MainHeroControl = Objects.MainHeroControl;
            Interface.Hover = HoverControl as UnitControl;

            UpdateMain(msElapsed);

            //cast abilities
            //do it here so it can be spammed, not just performed on click
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                var targetGuid = (HoverControl as ObjectControl)?.Object.Id ?? 0;
                var targetLoc = Screen.ScreenToGame(mouseState.Position.ToPoint());
                var justPressedKey = (oldMouseState.RightButton == ButtonState.Released);

                ActionMessage msg;
                if (tryCastAbility(Interface.CurrentAbility, justPressedKey, out msg))
                    ActionPerformed?.Invoke(msg);
            }
        }


        bool tryCastAbility(IAbility ab, bool displayErrors, out ActionMessage msg)
        {
            msg = null;
            var targetGuid = (HoverControl as ObjectControl)?.Object.Id ?? 0;
            var targetLoc = Screen.ScreenToGame(mouseState.Position.ToPoint());
            var mainHero = Objects.MainHero;

            if (mainHero == null || ab == null)
                return false;

            var justClicked = mouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released;
            if (ab.CurrentCooldown > 0 && justClicked)
            {
                if(displayErrors)
                    FloatingText.AddLabel(targetLoc, "Ability in cooldown", Color.Red, FloatingTextStyle.Top);
                Interface.DisplayError("Ability in cooldown");
                return false;
            }

            if (ab.TargetType != AbilityTargetType.NoTarget && targetLoc.DistanceTo(Objects.MainHero.Position) > ab.CastRange)
            {
                if(displayErrors)
                    FloatingText.AddLabel(targetLoc, "Out of range", Color.Red, FloatingTextStyle.Top);
                Interface.DisplayError("Out of range");
                return false;
            }

            if (Objects.MainHero.Mana < ab.ManaCost)
            {
                if(displayErrors)
                    FloatingText.AddLabel(targetLoc, "Not enough mana", Color.Red, FloatingTextStyle.Top);
                Interface.DisplayError("Not enough mana");
                return false;
            }

            msg = new ActionMessage(ab.Id, targetGuid, targetLoc);
            return true;
        }

    }
}
