using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using ProtoBuf;

namespace IO.Commands
{
    [ProtoContract]
    public class MovementArgs : CommandArgs
    {
        [ProtoMember(1)]
        public MovementState MoveState;

        public MovementArgs(MovementState moveState)
            :base(CommandType.MovementUpdate)
        {
            this.MoveState = moveState;
        }
    }
}
