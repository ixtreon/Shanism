using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Client
{
    [SerialKiller]
    public class HandshakeInitMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.HandshakeInit; }
        }

        [SerialMember]
        public string PlayerName;


        private HandshakeInitMessage() { }

        public HandshakeInitMessage(string playerName)
            : this()
        {
            PlayerName = playerName;
        }
    }
}
