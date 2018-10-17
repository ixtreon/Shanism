using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Messages
{
    /// <summary>
    /// An enumeration of all message types sent by the server.
    /// </summary>
    public enum ServerMessageType
    {
        /// <summary>
        /// A response to a player's request to join the game.
        /// </summary>
        HandshakeReply = 1,

        /// <summary>
        /// Contains updated player status (observer, player) and main hero ID, if available.
        /// </summary>
        PlayerStatus,

        /// <summary>
        /// Contains map data about a requested map chunk.
        /// </summary>
        MapData,

        /// <summary>
        /// Informs a player of a nearby damage event.
        /// </summary>
        DamageEvent,

        /// <summary>
        /// A chat message sent by a player or the game server.
        /// </summary>
        Chat,

        /// <summary>
        /// Informs a player they are no longer part of the game.
        /// </summary>
        Disconnected,
    }

    /// <summary>
    /// An enumeration of all message types sent by the client.
    /// </summary>
    public enum ClientMessageType
    {

        /// <summary>
        /// An initial request to join some server.
        /// </summary>
        HandshakeInit = 1,

        /// <summary>
        /// A request for a given map chunk.
        /// </summary>
        MapRequest,

        /// <summary>
        /// A chat message from the player.
        /// </summary>
        Chat,
    }
}
