using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Game
{
    /// <summary>
    /// The different states a client could be in relative to its server.
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// No connection to the server. 
        /// </summary>
        Disconnected,

        /// <summary>
        /// Awaiting server's handshake reply.
        /// </summary>
        AwaitReply,

        /// <summary>
        /// Playing the game.
        /// </summary>
        Playing,
    }
}
