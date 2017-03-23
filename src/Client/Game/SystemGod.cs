using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Input;
using Shanism.Client.Map;
using Shanism.Client.Systems;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;

namespace Shanism.Client
{

    /// <summary>
    /// The keeper of all in-game systems. 
    /// </summary>
    class SystemGod : ShanoComponent
    {
        readonly PlayerState playerState;

        readonly IReceptor server;

        readonly GameRoot root;

        #region Systems
        public SpriteSystem Objects { get; private set; }

        public Terrain Terrain { get; private set; }

        /// <summary>
        /// Listens for ability casts and informs the server. 
        /// </summary>
        public ActionSystem Actions { get; private set; }

        /// <summary>                                            
        /// Listens for key presses and informs the server. 
        /// </summary>
        public MoveSystem Movement { get; private set; }

        #endregion


        /// <summary>
        /// Raised whenever a system sends a message to the server.
        /// </summary>
        public event Action<IOMessage> MessageSent;


        public SystemGod(IShanoComponent game, 
            GameRoot root,
            IReceptor server, PlayerState playerState)
            : base(game)
        {
            this.root = root;
            this.server = server;
            this.playerState = playerState;

            Reload();
        }


        public void Reload()
        {
            Terrain = new Terrain(this, Screen.GraphicsDevice);
            Objects = new SpriteSystem(this, server);
            Movement = new MoveSystem(root, Keyboard, playerState);
            Actions = new ActionSystem(root, Mouse, playerState, Objects);

            Terrain.MessageSent += (m) => MessageSent?.Invoke(m);
        }


        public void Update(int msElapsed)
        {
            Terrain.Update(msElapsed);
            Objects.Update(msElapsed);

            Movement.Update(msElapsed);
            Actions.Update(msElapsed);
        }

    }
}
