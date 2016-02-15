using Client.Input;
using Client.Objects;
using Client.UI;
using Client.UI.CombatText;
using IO.Common;
using IO.Message.Client;
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
        public ObjectSystem Objects { get { return ObjectSystem.Default; } }


        public CombatText DamageText { get; } = new CombatText();

        public event Action<ActionMessage> ActionPerformed;

        public GameManager()
        {
            AbsolutePosition = new Vector(0);

            Interface = new UiManager();

            Objects.ObjectClicked += onObjectClicked;
            Objects.TerrainClicked += onGroundClicked;

            Add(Interface);
            Add(Objects);
            Objects.Add(DamageText);

            Interface.BringToFront();
        }

        void onGroundClicked(MouseButtonEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                //clear target
                Interface.Target = null;
            }
        }

        void onObjectClicked(MouseButtonEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                //target a unit only
                Interface.Target = e.Control as UnitControl;
            }
        }

        protected override void OnUpdate(int msElapsed)
        {
            //Maximize();
            Interface.Maximize();
            Objects.Maximize();

            UpdateMain(msElapsed);

            // update the interface's main hero from the objects' main hero. 
            Interface.MainHeroControl = Objects.MainHeroControl;

            //do it here so it can be spammed, not just performed on click
            if (mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var ab = Interface.CurrentAbility;

                if (ab == null || ab.CurrentCooldown > 0)
                    return;

                var msgGuid = (HoverControl as ObjectControl)?.Object.Id ?? 0;
                var msgLoc = Screen.ScreenToGame(mouseState.Position.ToPoint());
                var msg = new ActionMessage(ab.Name, msgGuid, msgLoc);

                ActionPerformed?.Invoke(msg);
            }

            Interface.Hover = HoverControl as UnitControl;
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);
        }

        public void ReloadUi()
        {
            Remove(Interface);
            Interface = new UiManager();
            Add(Interface);
        }
    }
}
