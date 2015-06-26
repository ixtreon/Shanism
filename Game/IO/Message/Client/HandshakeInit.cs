using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Client
{
    [ProtoContract]
    public class HandshakeInitMessage : IOMessage
    {
        [ProtoMember(1)]
        public string PlayerName;


        private HandshakeInitMessage() { }

        public HandshakeInitMessage(string playerName)
            : this()
        {
            PlayerName = playerName;
        }
    }
}
