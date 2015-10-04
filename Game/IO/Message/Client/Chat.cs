using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Client
{
    [SerialKiller]
    class ChatMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.SendChat; }
        }

        [SerialMember]
        public readonly string Channel;

        [SerialMember]
        public readonly string Message;

        public ChatMessage(string channel, string message)
        {
            this.Channel = channel;
            this.Message = message;
        }
    }
}
