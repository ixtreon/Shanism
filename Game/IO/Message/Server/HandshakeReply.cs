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

        /// <summary>
        /// Gets the scenario name. 
        /// </summary>
        [ProtoMember(2)]
        public readonly string ScenarioName;

        /// <summary>
        /// Gets the size of the scenario, in bytes. 
        /// </summary>
        [ProtoMember(3)]
        public readonly long ScenarioSize;

        /// <summary>
        /// Gets the hash value for the scenario. 
        /// </summary>
        [ProtoMember(4)]
        public readonly string ScenarioHash;


        public HandshakeReplyMessage() { }

        public HandshakeReplyMessage(bool isSuccessful)
        {
            Success = isSuccessful;
        }

    }
}
