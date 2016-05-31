using Shanism.Client.Input;
using Shanism.Client.Objects;
using Shanism.Client.UI;
using Shanism.Client.UI.CombatText;
using Shanism.Common.Game;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using Shanism.Common.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Client.Systems;
using Shanism.Client.Map;
using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client
{

    // A top level control, contains all the client systems
    class GameManager : Control
    {


        #region Systems

        readonly List<ClientSystem> systems = new List<ClientSystem>();

        readonly ActionSystem actions;
        readonly MoveSystem movement;
        readonly ChatSystem chat;
        readonly ObjectSystem objects;

        /// <summary>
        /// Manages chunks and sends requests for new ones. 
        /// </summary>
        readonly MapSystem map;

        UiSystem @interface;

        #endregion


        /// <summary>
        /// The main UI window. 
        /// </summary>
        public UiSystem Interface => @interface;

        /// <summary>
        /// The guy that handles objects. 
        /// </summary>
        public ObjectSystem Objects => objects;

        public MapSystem Map => map;


        public FloatingTextProvider FloatingText => Interface.FloatingText;


        public event Action<IOMessage> MessageSent;


        public GameManager(GraphicsDevice graphicsDevice)
        {
            AbsolutePosition = Vector.Zero;
            CanFocus = true;

            GameActionActivated += onActionActivated;

            //add systems n controls
            systems.Add(movement = new MoveSystem());
            systems.Add(chat = new ChatSystem());
            systems.Add(objects = new ObjectSystem());
            systems.Add(@interface = new UiSystem(Objects));
            systems.Add(actions = new ActionSystem(Interface));
            systems.Add(map = new MapSystem(graphicsDevice));

            Objects.ObjectClicked += onObjectClicked;
            Objects.TerrainClicked += onGroundClicked;

            foreach (var sys in systems)
                sys.MessageSent += (m) => MessageSent?.Invoke(m);

            Add(Objects.Root);
            Add(Interface.Root);
        }


        public void ReloadUi()
        {
            Remove(Interface.Root);
            @interface = new UiSystem(Objects);
            Add(Interface.Root);
        }


        void onActionActivated(ClientAction ga)
        {
            switch (ga)
            {
                case ClientAction.ToggleDebugInfo:
                    ClientEngine.ShowDebugStats = !ClientEngine.ShowDebugStats;
                    break;

                case ClientAction.ReloadUi:
                    ReloadUi();
                    break;

                case ClientAction.ShowHealthBars:
                    Settings.Current.AlwaysShowHealthBars = !Settings.Current.AlwaysShowHealthBars;
                    break;

                default:
                    //propagate to *both* interface and objects, let them handle it
                    Interface.Root.ActivateAction(ga);
                    Objects.Root.ActivateAction(ga);
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
            // update the interface's main hero from the objects' main hero. 
            Interface.MainHeroControl = Objects.MainHeroControl;
            Interface.Hover = HoverControl as UnitControl;

            actions.Hero = Objects.MainHero;

            UpdateMain(msElapsed);


            foreach (var sys in systems)
                sys.Update(msElapsed);
        }

    }
}
