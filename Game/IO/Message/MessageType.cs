using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message
{
    /// <summary>
    /// An enumeration of all message or command types. 
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The message the server sends in reply to a client's hanshake message. See <see cref="HandshakeInit"/>. 
        /// </summary>
        HandshakeReply,

        /// <summary>
        /// The messsage the server sends in a reply to a client's map request. 
        /// If the request is valid the response contains data for the specified chunk. 
        /// </summary>
        MapReply,

        /// <summary>
        /// The message a client sends to the server to request a map chunk. 
        /// </summary>
        MapRequest,

        /// <summary>
        /// The message a client sends to update its state
        /// </summary>
        MoveUpdate,

        /// <summary>
        /// The message a client sends to perform an action. 
        /// </summary>
        Action,

        /// <summary>
        /// The message a client sends to the server to initiate a handshake. 
        /// </summary>
        HandshakeInit,

        /// <summary>
        /// The message a client sends to post a chat message. 
        /// </summary>
        SendChat,

        /// <summary>
        /// A message containing a full update of an object's state. 
        /// </summary>
        FullUpdate,

        /// <summary>
        /// A message containing a partial update of am object's state. 
        /// </summary>
        PartialUpdate,

        /// <summary>
        /// A message with the status of a client: camera position and assigned hero id (if any)
        /// </summary>
        PlayerStatusUpdate,

        /// <summary>
        /// A message sent by the server to inform of a nearby unit being damaged. 
        /// </summary>
        UnitDamage,

        /// <summary>
        /// Relay an issued order to all nearby players. 
        /// </summary>
        RelayOrder,

    }
}
