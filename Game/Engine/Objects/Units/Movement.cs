using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    partial class Unit
    {
        /// <summary>
        /// Gets the walking speed of the unit, which is the running speed divided by 3. 
        /// </summary>
        public double WalkSpeed { get { return MoveSpeed / 3; } }

        /// <summary>
        /// Causes the unit to move in the given direction. Works only once per update frame 
        /// and only if the unit has not moved during this frame already. 
        /// 
        /// This is the official way to make units walk around. 
        /// </summary>
        /// <param name="msElapsed"></param>
        /// <param name="direction"></param>
        /// <param name="maxDistance"></param>
        /// <param name="walk">Whether to walk instead of running. </param>
        internal void Move(double msElapsed, double direction, double maxDistance = double.MaxValue, bool walk = false)
        {
            //if we have already moved or teleported this turn, return
            if (Position != NewPosition)
                return;

            var speed = walk ? WalkSpeed : MoveSpeed;

            var moveDist = Math.Min(maxDistance, speed * msElapsed / 1000);
            var pos = Position.PolarProjection(direction, moveDist);
            setLocation(pos, true);
        }
    }
}
