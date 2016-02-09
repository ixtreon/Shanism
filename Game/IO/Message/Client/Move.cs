using IO.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Client
{
    [ProtoContract]
    public class MoveMessage : IOMessage
    {
        

        [ProtoMember(1)]
        public readonly MovementState Direction;

        MoveMessage() { Type = MessageType.MoveUpdate; }

        public MoveMessage(MovementState st)
            : this()
        {
            Direction = st;
        }
    }
}
