using Client.Input;
using Client.Objects;
using Client.UI;
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
        public ObjectGod Objects { get; }


        public event Action<ActionMessage> ActionPerformed;


        public GameManager()
        {
            Location = Vector.Zero;

            Interface = new UiManager();

            Objects = new ObjectGod();
            Objects.ObjectClicked += onObjectClicked;
            Objects.TerrainClicked += onGroundClicked;

            Add(Interface);
            Add(Objects);
            Objects.SendToBack();
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
            UpdateMain(msElapsed);

            Interface.Maximize();
            Objects.Maximize();

            // update the interface's main hero from the objects' main hero. 
            Interface.MainHeroControl = Objects.MainHeroControl;

            //do it here so it can be spammed, not just performed on click
            if(mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var ab = Interface.CurrentAbility;

                if (ab == null || ab.CurrentCooldown > 0)
                    return;

                var msgGuid = (HoverControl as ObjectControl)?.Object.Guid ?? 0;
                var msgLoc = Screen.ScreenToGame(mouseState.Position.ToPoint());
                var msg = new ActionMessage(ab.Name, msgGuid, msgLoc);

                ActionPerformed?.Invoke(msg);
            }
             
            Interface.Hover = HoverControl as UnitControl;
        }

        readonly object _interfaceLock = new object();

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
