using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    [ProtoContract]
    public class ChatMessage : IOMessage
    {
        

        /// <summary>
        /// The Guid of the sender, or 0 if this is a system message. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint SenderGuid;

        /// <summary>
        /// The Guid of the sender, or 0 if this is a public message. 
        /// </summary>
        [ProtoMember(2)]
        public readonly uint ReceiverGuid;

        [ProtoMember(3)]
        public readonly string Message;

        ChatMessage() { Type = MessageType.SendChat; }

        public ChatMessage(string message, IUnit sender, IUnit receiver)
            : this()
        {
            SenderGuid = sender?.Guid ?? 0;
            ReceiverGuid = receiver?.Guid ?? 0;
            Message = message;
        }
    }
}
