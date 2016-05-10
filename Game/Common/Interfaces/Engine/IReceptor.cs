using Shanism.Common.Message;
using Shanism.Common.Message.Network;
using Shanism.Common.Message.Server;
using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// A game receptor as exposed by a <see cref="IShanoEngine"/> to all clients. 
    /// 
    /// Event based for simplicity.  
    /// </summary>
    public interface IReceptor
    {
        /// <summary>
        /// The event raised whenever the server sends a message to the player. 
        /// </summary>
        event Action<IOMessage> MessageSent;


        /// <summary>
        /// Causes the underlying game server to update. 
        /// Called by the client to update the local or network server instance, respectively. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last invocation of this method. </param>
        void UpdateServer(int msElapsed);

        string GetPerfData();

        //GameFrameMessage GetCurrentFrame();
    }
}
