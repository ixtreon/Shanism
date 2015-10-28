using IO.Objects;
using IxSerializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message
{
    [SerialKiller]
    public class ChatMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.SendChat; }
        }

        /// <summary>
        /// The Guid of the sender, or 0 if this is a system message. 
        /// </summary>
        [SerialMember]
        public readonly int SenderGuid;

        /// <summary>
        /// The Guid of the sender, or 0 if this is a public message. 
        /// </summary>
        [SerialMember]
        public readonly int ReceiverGuid;

        [SerialMember]
        public readonly string Message;

        public ChatMessage(string message, IUnit sender, IUnit receiver)
        {
            SenderGuid = sender?.Guid ?? 0;
            ReceiverGuid = receiver?.Guid ?? 0;
            Message = message;
        }
    }
}
