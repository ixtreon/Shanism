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
        public static MovementState Stand = new MovementState(0, 0);

        private int dx, dy;

        [ProtoMember(1)]
        public int XDirection
        {
            get { return dx; }
            set { dx = Math.Sign(value); }
        }

        [ProtoMember(2)]
        public int YDirection
        {
            get { return dy; }
            set { dy = Math.Sign(value); }
        }

        public Vector DirectionVector
        {
            get { return new Vector(dx, dy).Normalize(); }
        }

        public MovementState(int px, int py)
        {
            dx = Math.Sign(px);
            dy = Math.Sign(py);
        }

        public bool IsMoving
        {
            get { return XDirection != 0 || YDirection != 0; }
        }

        public static bool operator ==(MovementState a, MovementState b)
        {
            return a.XDirection == b.XDirection && a.YDirection == b.YDirection;
        }
        public static bool operator != (MovementState a, MovementState b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return dx + 2 * dy; //9 possible options
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MovementState))
                return false;
            return (MovementState)obj == this;
        }
    }
}
