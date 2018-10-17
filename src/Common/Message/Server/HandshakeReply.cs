using Shanism.Common.Content;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Messages
{
    /// <summary>
    /// The reply of a server to a client willing to join the game. 
    /// Contains whether the client is successfully accepted. 
    /// If the client is accepted also contains the data 
    /// necessary to play the current scenario. 
    /// </summary>
    [ProtoContract]
    public class HandshakeReply : ServerMessage
    {
        /// <summary>
        /// A negative response to a player joining. 
        /// </summary>
        public static readonly HandshakeReply Negative = new HandshakeReply();


        public override ServerMessageType Type => ServerMessageType.HandshakeReply;


        /// <summary>
        /// Gets whether the handshake was successful. 
        /// </summary>
        [ProtoMember(1)]
        public readonly bool Success = false;

        /// <summary>
        /// Gets the player's unique identifier as assigned by the server.
        /// Only valid if <see cref="Success"/> is set to true.
        /// </summary>
        [ProtoMember(2)]
        public readonly uint PlayerId;

        /// <summary>
        /// Contains the json config of the scenario. 
        /// </summary>
        [ProtoMember(3)]
        public readonly byte[] ScenarioData;

        /// <summary>
        /// Contains zipped binary representation of all textures. 
        /// </summary>
        [ProtoMember(4)]
        public readonly byte[] ContentData;

        HandshakeReply() { }

        public HandshakeReply(uint playerId, 
            byte[] scenarioData, byte[] contentData)
            :this()
        {
            Success = true;
            PlayerId = playerId;
            ScenarioData = scenarioData;
            ContentData = contentData;
        }

    }
}
