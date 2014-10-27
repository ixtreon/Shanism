using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace IO.Common
{
    [ProtoContract]
    public struct MovementState
    {
        [ProtoMember(1)]
        public int XDirection;

        [ProtoMember(2)]
        public int YDirection;

        public MovementState(int dx, int dy)
        {
            XDirection = dx;
            YDirection = dy;
        }
    }
}
