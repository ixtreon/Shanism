using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Server;

namespace Shanism.Common
{
    /// <summary>
    /// A prospective client looking to play on the server.
    ///  
    /// It could be either a local or remote (network'd) one. 
    /// </summary>
    public interface IShanoClient
    {
        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The event raised when a client wants to do something. 
        /// </summary>
        event Action<IOMessage> MessageSent;

        /// <summary>
        /// Gets the current state of the client.
        /// </summary>
        PlayerState State { get; }

    }
}
