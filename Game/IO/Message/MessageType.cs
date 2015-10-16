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
        /// A message sent by both the client and the server to relay chat messages. 
        /// </summary>
        SendChat = 1,

        ///-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        /// <summary>
        /// A message sent by the client to request a map chunk. 
        /// </summary>
        MapRequest,

        /// <summary>
        /// A message sent by the client to update its state
        /// </summary>
        MoveUpdate,

        /// <summary>
        /// A message sent by the client to perform an action. 
        /// </summary>
        Action,

        /// <summary>
        /// A message sent by the client to the server to initiate a handshake. 
        /// </summary>
        HandshakeInit,

        ///-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

        /// <summary>
        /// A message sent by the server in reply to a client's hanshake message. See <see cref="HandshakeInit"/>. 
        /// </summary>
        HandshakeReply,

        /// <summary>
        /// A message sent by the server in reply to a client's map request. 
        /// If the request is valid the response contains data for the specified chunk. 
        /// </summary>
        MapReply,

        /// <summary>
        /// A message sent by the server containing a full update of an object's state. 
        /// </summary>
        FullUpdate,

        /// <summary>
        /// A message sent by the server containing a partial update of am object's state. 
        /// </summary>
        PartialUpdate,

        /// <summary>
        /// A message sent by the server to inform the hero of his status (i.e. if a player has a hero). 
        /// </summary>
        PlayerStatusUpdate,

        /// <summary>
        /// A message sent by the server to inform of a nearby unit being damaged. 
        /// </summary>
        UnitDamage,

        /// <summary>
        /// A message sent by the server when the client sees an object. 
        /// </summary>
        ObjectSeen,

        /// <summary>
        /// A message sent by the server when an object disappears from the client's view. 
        /// </summary>
        ObjectUnseen,

        /// <summary>
        /// A message sent by the server when an object changes its animation. 
        /// </summary>
        ObjectAnimation,
    }
}
