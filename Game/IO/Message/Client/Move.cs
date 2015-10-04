using IO.Common;
using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Client
{
    [SerialKiller]
    public class MoveMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.MoveUpdate; }
        }

        [SerialMember]
        public readonly MovementState Direction;

        private MoveMessage() { }

        public MoveMessage(MovementState st)
        {
            Direction = st;
        }
    }
}
