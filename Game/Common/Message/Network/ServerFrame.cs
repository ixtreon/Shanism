using Shanism.Common.Game;
using Shanism.Common.StubObjects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Network
{
    [ProtoContract]
    public class GameFrameMessage : IOMessage
    {
        [ProtoMember(1)]
        public readonly byte[] Data;


        public override MessageType Type { get { return MessageType.GameFrame; } }

        GameFrameMessage() { }

        public GameFrameMessage(byte[] datas)
            : this()
        {
            Data = datas;
        }
    }
}
