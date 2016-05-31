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

        /// <summary>
        /// Listens for ability casts and informs the server. 
        /// </summary>
        readonly ActionSystem actions;

        /// <summary>
        /// Listens for key presses and informs the server. 
        /// </summary>
        readonly MoveSystem movement;

        /// <summary>
        /// Listens for chat messages and sends them to the chatbox
        /// </summary>
        readonly ChatSystem chat;

        /// <summary>
        /// Lists and draws objects. 
        /// </summary>
        readonly ObjectSystem objects;

        /// <summary>
        /// Manages chunks and sends requests for new ones. 
        /// </summary>
        readonly MapSystem map;

        /// <summary>
        /// Holds all elements of the UI. 
        /// </summary>
        UiSystem @interface;

        #endregion


        public void SetUiVisible(bool isVisible) => @interface.Root.IsVisible = IsVisible;

        public void DrawUi(SpriteBatch sb) => @interface.Root.Draw(sb);

        public void DrawObjects(SpriteBatch sb) => objects.Root.Draw(sb);

        public void DrawTerrain() => map.DrawTerrain();

        public IHero MainHero => objects.MainHero;



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
            systems.Add(@interface = new UiSystem(objects));
            systems.Add(actions = new ActionSystem(@interface));
            systems.Add(map = new MapSystem(graphicsDevice));

            objects.ObjectClicked += onObjectClicked;
            objects.TerrainClicked += onGroundClicked;

            @interface.chatBox.SetProvider(chat);

            foreach (var sys in systems)
                sys.MessageSent += (m) => MessageSent?.Invoke(m);

            Add(objects.Root);
            Add(@interface.Root);
        }


        public void ReloadUi()
        {
            Remove(@interface.Root);
            @interface = new UiSystem(objects);
            Add(@interface.Root);
        }

        public void HandleMessage(IOMessage msg)
        {
            foreach (var sys in systems)
                sys.HandleMessage(msg);
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
                    @interface.Root.ActivateAction(ga);
                    objects.Root.ActivateAction(ga);
                    break;
            }
        }

        void onGroundClicked(MouseButtonArgs e)
        {
            //clear target
            if (e.Button == MouseButton.Left)
                @interface.Target = null;
        }

        void onObjectClicked(MouseButtonArgs e)
        {
            //target a unit or nothing
            if (e.Button == MouseButton.Left)
                @interface.Target = e.Control as UnitControl;
        }

        protected override void OnUpdate(int msElapsed)
        {
            // update the interface's main hero from the objects' main hero. 
            @interface.MainHeroControl = objects.MainHeroControl;
            @interface.Hover = HoverControl as UnitControl;

            actions.Hero = objects.MainHero;

            UpdateMain(msElapsed);


            foreach (var sys in systems)
                sys.Update(msElapsed);
        }

    }
}
