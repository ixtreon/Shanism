using Shanism.Common.StubObjects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Message.Server
{
    [ProtoContract]
    public class ChatMessage : IOMessage
    {
        const int SystemMessageId = Util.GenericId<bool>.None;

        public override MessageType Type => MessageType.ServerChat;


        /// <summary>
        /// The Guid of the sender, or 0 if this is a system message. 
        /// </summary>
        [ProtoMember(1)]
        public readonly int SenderGuid;

        [ProtoMember(2)]
        public readonly string Message;

        public bool IsSystem => SenderGuid == SystemMessageId;

        ChatMessage() { }

        public ChatMessage(string message, IPlayer sender)
            : this()
        {
            SenderGuid = (int?)sender?.Id ?? SystemMessageId;
            Message = message;
        }
    }
}
