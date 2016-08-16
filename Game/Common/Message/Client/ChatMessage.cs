using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Client
{
    [ProtoContract]
    public class ChatMessage : IOMessage
    {
        public override MessageType Type => MessageType.ClientChat;

        [ProtoMember(1)]
        public readonly string Channel;

        [ProtoMember(2)]
        public readonly string Message;

        ChatMessage() { }

        public ChatMessage(string channel, string msg)
        {
            Channel = channel;
            Message = msg;
        }
    }
}
