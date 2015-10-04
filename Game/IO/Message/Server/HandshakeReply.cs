using IxSerializer.Modules;
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

        /// <summary>
        /// Gets the scenario name. 
        /// </summary>
        [SerialMember]
        public readonly string ScenarioName;

        /// <summary>
        /// Gets the size of the scenario, in bytes. 
        /// </summary>
        [SerialMember]
        public readonly long ScenarioSize;

        /// <summary>
        /// Gets the hash value for the scenario. 
        /// </summary>
        [SerialMember]
        public readonly string ScenarioHash;


        public HandshakeReplyMessage() { }

        public HandshakeReplyMessage(bool isSuccessful)
        {
            Success = isSuccessful;
        }

    }
}
