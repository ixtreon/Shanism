using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Input;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// An abstract system for the client engine. 
    /// Can handle messages and update the client state.
    /// </summary>
    abstract class ClientSystem : GameComponent
    {

        public IReceptor Server { get; set; }

        public PlayerState ClientState { get; set; }


        public event Action<IOMessage> MessageSent;


        public ClientSystem(GameComponent game)
            : base(game)
        {

        }


        internal void SendMessage(IOMessage msg) => MessageSent?.Invoke(msg);

        public virtual void HandleMessage(IOMessage ioMsg) { }

        public abstract void Update(int msElapsed);
    }

}
