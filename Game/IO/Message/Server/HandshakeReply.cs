using IO.Content;
using IxSerializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    [SerialKiller]
    public class HandshakeReplyMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.HandshakeReply; }
        }

        /// <summary>
        /// Gets whether the handshake was successful. 
        /// </summary>
        [SerialMember]
        public readonly bool Success;

        [SerialMember]
        public readonly byte[] ScenarioData;

        [SerialMember]
        public readonly byte[] ContentData;

        public HandshakeReplyMessage() { }

        public HandshakeReplyMessage(bool isSuccessful, byte[] scenarioData, byte[] contentData)
        {
            Success = isSuccessful;
            ScenarioData = scenarioData;
            ContentData = contentData;
        }

    }
}
