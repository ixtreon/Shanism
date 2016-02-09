using IO.Content;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    [ProtoContract]
    public class HandshakeReplyMessage : IOMessage
    {
        

        /// <summary>
        /// Gets whether the handshake was successful. 
        /// </summary>
        [ProtoMember(1)]
        public readonly bool Success;

        [ProtoMember(2)]
        public readonly byte[] ScenarioData;

        [ProtoMember(3)]
        public readonly byte[] ContentData;

        HandshakeReplyMessage() { Type = MessageType.HandshakeReply; }

        public HandshakeReplyMessage(bool isSuccessful, byte[] scenarioData, byte[] contentData)
            :this()
        {
            Success = isSuccessful;
            ScenarioData = scenarioData;
            ContentData = contentData;
        }

    }
}
