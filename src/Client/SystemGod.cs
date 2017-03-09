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
    /// The keeper of all in-game systems. 
    /// </summary>
    class SystemGod : GameComponent
    {
        readonly PlayerState clientState;

        readonly IReceptor server;

        readonly List<ClientSystem> systems = new List<ClientSystem>();


        readonly GameComponent game;

        Screen Screen => game.Screen;

        ContentList Content => game.Content;


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


        public SystemGod(GameComponent game, 
            IReceptor server, PlayerState playerState)
            : base(game)
        {
            this.game = game;
            this.server = server;
            this.clientState = playerState;

            Control.SetContext(game);

            objects = new SpriteSystem(game);
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
            systems.Add(terrain = new Terrain(this));
            systems.Add(objects);
            systems.Add(chat = new ChatSystem(this));
            systems.Add(@interface = new UiSystem(this, objects, chat));
            systems.Add(movement = new MoveSystem(this, @interface));
            systems.Add(actions = new ActionSystem(this, @interface, objects));

            foreach (var sys in systems)
            {
                sys.Server = server;
                sys.ClientState = clientState;
                sys.MessageSent += (m) => MessageSent?.Invoke(m);
            }
        }

        public void HandleMessage(IOMessage msg)
        {
            foreach (var sys in systems)
                sys.HandleMessage(msg);
        }


        public void Update(int msElapsed)
        {
            actions.Hero = objects.MainHero;

            if(Keyboard.JustActivatedActions.Contains(ClientAction.ReloadUi))
                Reload();


            foreach (var sys in systems)
                sys.Update(msElapsed);
        }

    }
}
