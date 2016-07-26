using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Game
{
    /// <summary>
    /// The movement state of a unit. Tells whether the unit is moving and if so, in what direction. 
    /// </summary>
    [ProtoContract]
    public struct MovementState
    {
        /// <summary>
        /// The movement state where a unit is not moving. 
        /// </summary>
        public static MovementState Stand = new MovementState(0, 0);

        [ProtoMember(1)]
        readonly bool isMoving;

        [ProtoMember(2)]
        readonly double angle;
        /// <summary>
        /// Gets a value indicating whether this instance is moving.
        /// </summary>
        public bool IsMoving => isMoving;

        /// <summary>
        /// Gets a direction vector of length 1. 
        /// </summary>
        public Vector DirectionVector => Vector.Zero.PolarProjection(angle, 1);


        public double AngleRad => angle;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementState"/> struct.
        /// </summary>
        /// <param name="dx">The offset in the X direction. </param>
        /// <param name="dy">The offset in the Y direction. </param>
        public MovementState(int dx, int dy)
        {
            isMoving = (dx != 0 || dy != 0);
            angle = Math.Atan2(dy, dx);
        }

        /// <summary>
        /// Implements logical equality. 
        /// </summary>
        public static bool operator ==(MovementState a, MovementState b)
        {
            return a.isMoving == b.isMoving && a.angle.AlmostEqualTo(b.angle, 0.0005);
        }

        /// <summary>
        /// Implements logical not-equal. 
        /// </summary>
        public static bool operator != (MovementState a, MovementState b)
        {
            return !(a == b);
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (isMoving ? (2 * Math.PI + angle).GetHashCode() : -1);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is MovementState))
                return false;
            return (MovementState)obj == this;
        }
    }
}
