using Shanism.Common.Content;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Message.Server
{
    /// <summary>
    /// The reply of a server to a client willing to join the game. 
    /// Contains whether the client is successfully accepted. 
    /// If the client is accepted also contains the data 
    /// necessary to play the current scenario. 
    /// </summary>
    [ProtoContract]
    public class HandshakeReplyMessage : IOMessage
    {
        /// <summary>
        /// A negative response to a player joining. 
        /// </summary>
        public static readonly HandshakeReplyMessage Negative = new HandshakeReplyMessage(false, null, null);

        /// <summary>
        /// Gets whether the handshake was successful. 
        /// </summary>
        [ProtoMember(1)]
        public readonly bool Success;

        /// <summary>
        /// Contains the json config of the scenario. 
        /// </summary>
        [ProtoMember(2)]
        public readonly byte[] ScenarioData;

        /// <summary>
        /// Contains zipped binary representation of all textures. 
        /// </summary>
        [ProtoMember(3)]
        public readonly byte[] ContentData;

        public override MessageType Type { get { return MessageType.HandshakeReply; } }

        HandshakeReplyMessage() { }

        public HandshakeReplyMessage(bool isSuccessful, byte[] scenarioData, byte[] contentData)
            :this()
        {
            Success = isSuccessful;
            ScenarioData = scenarioData;
            ContentData = contentData;
        }

    }
}
