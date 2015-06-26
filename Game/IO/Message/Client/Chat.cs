using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Client
{
    [ProtoContract]
    class ChatMessage : IOMessage
    {
        [ProtoMember(1)]
        public readonly string Channel;

        [ProtoMember(2)]
        public readonly string Message;

        public ChatMessage(string channel, string message)
        {
            this.Channel = channel;
            this.Message = message;
        }
    }
}
