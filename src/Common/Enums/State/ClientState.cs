using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// The state of a game client as seen by its owner. 
    /// </summary>
    public enum ClientState
    {
        /// <summary>
        /// No connection to the server.
        /// </summary>
        Disconnected,

        /// <summary>
        /// Attempting to connect to a server.
        /// </summary>
        Connecting,
        /// <summary>
        /// Connected to a server but not sent a handshake yet.
        /// </summary>
        Connected,
        /// <summary>
        /// Sent a handshake, and awaiting the server reply.
        /// </summary>
        AwaitingHandshake,
        /// <summary>
        /// Connected to a server but they rejected us joining.
        /// </summary>
        Rejected,
        /// <summary>
        /// Playing the game.
        /// </summary>
        Playing,

    }

    public static class ClientStateExt
    {
        /// <summary>
        /// Gets a player-friendly description of this state.
        /// </summary>
        public static string GetDescription(this ClientState state)
        {
            switch (state)
            {
                case ClientState.Disconnected:
                    return "Disconnected";

                case ClientState.Connecting:
                    return "Connecting...";

                case ClientState.Connected:
                    return "Connected";

                case ClientState.AwaitingHandshake:
                    return "Awaiting handshake...";

                case ClientState.Rejected:
                    return "Connection rejected";

                case ClientState.Playing:
                    return "Playing";
            }
            throw new ArgumentOutOfRangeException(nameof(state));
        }
    }

}
