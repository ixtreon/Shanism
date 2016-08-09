using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// Gets the current movement state, also order, of an unit.
    /// </summary>
    public struct MovementState
    {
        public static readonly MovementState Stand = new MovementState(float.NaN, float.NaN);

        public readonly float MoveDirection;

        public readonly float MaxDistance;

        public bool IsMoving => !float.IsNaN(MoveDirection);

        public bool HasMaxDistance => !float.IsNaN(MaxDistance);

        public MovementState(float direction)
        {
            MoveDirection = direction;
            MaxDistance = float.NaN;
        }

        public MovementState(float direction, float maxDist)
        {
            MoveDirection = direction;
            MaxDistance = maxDist;
        }

    }
}
