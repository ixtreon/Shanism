using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Messages
{
    [ProtoContract]
    public class ClientChat : ClientMessage
    {
        public override ClientMessageType Type => ClientMessageType.Chat;

        [ProtoMember(1)]
        public readonly string Channel;

        [ProtoMember(2)]
        public readonly string Message;

        ClientChat() { }

        public ClientChat(string channel, string msg)
        {
            Channel = channel;
            Message = msg;
        }
    }
}
