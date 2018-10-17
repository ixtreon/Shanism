using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Shanism.Common.Messages
{
    [ProtoContract]
    public class Disconnected : ServerMessage
    {
        public override ServerMessageType Type => ServerMessageType.Disconnected;

        /// <summary>
        /// The reason for disconnecting.
        /// </summary>
        [ProtoMember(1)]
        public DisconnectReason Reason { get; set; }

        Disconnected() { }

        public Disconnected(DisconnectReason reason)
        {
            Reason = reason;
        }
    }
}
