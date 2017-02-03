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
            MoveDirection = normaliseAngle(direction);
            MaxDistance = float.NaN;
        }

        public MovementState(float direction, float maxDist)
        {
            MoveDirection = normaliseAngle(direction);
            MaxDistance = maxDist;
        }

        public MovementState(byte directionByte)
        {
            const byte min = 1;
            const byte max = 255;
            const float scale = 2 * (float)Math.PI / (max - min + 1);

            if (directionByte != 0)
                MoveDirection = ((float)directionByte - min) * scale;
            else
                MoveDirection = float.NaN;
            MaxDistance = float.NaN;
        }

        static float normaliseAngle(float ang)
        {
            const float mult = 2 * (float)Math.PI;

            if (float.IsNaN(ang))
                return float.NaN;

            ang %= mult;
            if (ang >= 0)
                return ang;
            return ang + mult;
        }

        /// <summary>
        /// Gets a byte describing the current direction of movement.
        /// </summary>
        public byte GetDirectionByte()
        {
            // Returns a value in the range 1-255 if the object is moving,
            // or 0 if there is no movemnet. 
            const byte min = 1;
            const byte max = 255;
            const float scale = 2 * (float)Math.PI / (max - min + 1);

            if (!IsMoving)
                return 0;
            return (byte)((byte)Math.Round(MoveDirection / scale) + min);
        }

    }
}
