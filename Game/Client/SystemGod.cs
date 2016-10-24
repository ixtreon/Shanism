using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Client.Systems;
using Shanism.Client.Map;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Message.Client;

namespace Shanism.Client
{

    /// <summary>
    /// The keeper of all game systems. 
    /// </summary>
    class SystemGod : Control
    {
        readonly ClientState clientState;
        readonly IReceptor server;

        readonly List<ClientSystem> systems = new List<ClientSystem>();

        readonly GraphicsDevice device;

        #region Systems

        /// <summary>
        /// Lists and draws objects. 
        /// </summary>
        readonly SpriteSystem objects;

        Terrain terrain;

        /// <summary>
        /// Listens for ability casts and informs the server. 
        /// </summary>
        ActionSystem actions;

        /// <summary>
        /// Listens for key presses and informs the server. 
        /// </summary>
        MoveSystem movement;

        /// <summary>
        /// Listens for chat messages and sends them to the chatbox
        /// </summary>
        ChatSystem chat;

        UiSystem @interface;

        #endregion


        public SystemGod(GraphicsDevice device, AssetList content, 
            IReceptor server, ClientState clientState)
        {
            this.server = server;
            this.device = device;
            this.clientState = clientState;

            Location = Vector.Zero;
            CanFocus = true;
            Control.SetContent(content);

            GameActionActivated += onActionActivated;

            objects = new SpriteSystem(device, Content);
            Reload();
        }


        public IHero MainHero => objects.MainHero;


        public void SetUiVisible(bool isVisible) 
            => @interface.Root.IsVisible = isVisible;

        public void DrawUi()
            => @interface.Draw();

        public void DrawObjects()
            => objects.Draw();

        public void DrawTerrain()
            => terrain.Draw();

        /// <summary>
        /// Raised whenever a system sends a message to the server.
        /// </summary>
        public event Action<IOMessage> MessageSent;



        public void Reload()
        {
            //systems
            systems.Clear();
            systems.Add(terrain = new Terrain(device, Content.Terrain));
            systems.Add(objects);
            systems.Add(chat = new ChatSystem());
            systems.Add(@interface = new UiSystem(device, objects, chat));
            systems.Add(movement = new MoveSystem(@interface));
            systems.Add(actions = new ActionSystem(@interface, objects));

            foreach (var sys in systems)
            {
                sys.Server = server;
                sys.ClientState = clientState;
                sys.MessageSent += (m) => MessageSent?.Invoke(m);
            }
            //controls
            ClearControls();
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
                    Reload();
                    break;

                case ClientAction.ShowHealthBars:
                    Settings.Current.AlwaysShowHealthBars = !Settings.Current.AlwaysShowHealthBars;
                    break;

                default:
                    //propagate to interface (objects?), let them handle it
                    @interface.Root.ActivateAction(ga);
                    break;
            }
        }


        protected override void OnUpdate(int msElapsed)
        {
            Size = Screen.UiSize;
            Location = Screen.UiSize / -2;

            actions.Hero = objects.MainHero;

            UpdateMain(msElapsed);
            foreach (var sys in systems)
                sys.Update(msElapsed);
        }

    }
}
