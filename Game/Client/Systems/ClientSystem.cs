using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// An abstract system for the client engine. 
    /// Can handle messages and update itself. 
    /// </summary>
    abstract class ClientSystem
    {
        public IReceptor Server;

        public ClientState ClientState;


        public event Action<IOMessage> MessageSent;

        internal void SendMessage(IOMessage msg) => MessageSent?.Invoke(msg);

        public virtual void HandleMessage(IOMessage ioMsg) { }

        public abstract void Update(int msElapsed);
    }

}
