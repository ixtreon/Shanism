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
        const int SystemMessageId = 0;


        /// <summary>
        /// The Guid of the sender, or 0 if this is a system message. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint SenderGuid;

        [ProtoMember(2)]
        public readonly string Message;

        public bool IsSystem {  get { return SenderGuid == SystemMessageId; } }


        ChatMessage() { Type = MessageType.SendChat; }

        public ChatMessage(string message, IPlayer sender)
            : this()
        {
            SenderGuid = sender?.Id ?? SystemMessageId;
            Message = message;
        }
    }
}
