using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Messages
{
    [ProtoContract]
    public class HandshakeInit : ClientMessage
    {
        public override ClientMessageType Type => ClientMessageType.HandshakeInit;


        [ProtoMember(1)]
        public string PlayerName;


        public HandshakeInit() { }

        public HandshakeInit(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
