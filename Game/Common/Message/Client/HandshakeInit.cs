using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Message.Client
{
    [ProtoContract]
    public class HandshakeInitMessage : IOMessage
    {
        

        [ProtoMember(1)]
        public string PlayerName;


        public override MessageType Type => MessageType.HandshakeInit;
        public HandshakeInitMessage() { }

        public HandshakeInitMessage(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
