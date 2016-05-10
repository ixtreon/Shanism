using Shanism.Common.Game;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Client
{
    [ProtoContract]
    public class MoveMessage : IOMessage
    {
        

        [ProtoMember(1)]
        public readonly MovementState Direction;

        public override MessageType Type { get { return MessageType.MoveUpdate; } }

        MoveMessage() { }

        public MoveMessage(MovementState st)
            : this()
        {
            Direction = st;
        }
    }
}
