using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message
{
    /// <summary>
    /// An enumeration of all message or command types. 
    /// Enum integer values start from 2 onwards, to satisfy ProtoBuf requirements for the IOMessage class. 
    /// </summary>
    public enum MessageType
    {

        /// -=-=-=-=-=- Client -> Server -=-=-=-=-=- 

        /// <summary>
        /// A message sent by the client to the server to initiate a handshake. 
        /// </summary>
        HandshakeInit = 2,

        /// <summary>
        /// A message sent by the client to request a map chunk. 
        /// </summary>
        MapRequest,

        /// <summary>
        /// A message sent by the client to relay chat messages. 
        /// </summary>
        ClientChat,


        /// -=-=-=-=-=- Server -> Client -=-=-=-=-=- 

        /// <summary>
        /// A message sent by the server in reply to a client's hanshake message. See <see cref="HandshakeInit"/>. 
        /// </summary>
        HandshakeReply,

        /// <summary>
        /// A message sent by the server to inform the player of his status (i.e. if a player has a hero). 
        /// </summary>
        PlayerStatusUpdate,

        /// <summary>
        /// A message sent by the server in reply to a client's map request. 
        /// If the request is valid the response contains data for the specified chunk. 
        /// </summary>
        MapReply,

        /// <summary>
        /// A message sent by the server to inform of a nearby unit being damaged. 
        /// </summary>
        DamageEvent,

        /// <summary>
        /// A message sent by the server to relay chat messages. 
        /// </summary>
        ServerChat,

        /// <summary>
        /// A message sent by the server when an object changes its animation. 
        /// </summary>
        ObjectAnimation,

        /// <summary>
        /// A message sent by the server to inform a client they were disconnected. 
        /// </summary>
        Disconnected,


        /// -=-=-=-=-=- Common -=-=-=-=-=- 

        /// <summary>
        /// A message send by both the client and the server. A generic frame. 
        /// </summary>
        GameFrame,
    }
}
