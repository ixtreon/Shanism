using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Shanism.Common.Message.Server
{
    [ProtoContract]
    public class DisconnectedMessage : IOMessage
    {
        public override MessageType Type => MessageType.Disconnected;

        [ProtoMember(0)]
        public DisconnectReason Reason { get; }


        public DisconnectedMessage(DisconnectReason reason)
        {
            Reason = reason;
        }
    }
}
