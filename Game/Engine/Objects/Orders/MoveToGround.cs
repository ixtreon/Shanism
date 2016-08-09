using Shanism.Common;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// A behaviour that makes the controlled unit go the specified in-game location. 
    /// </summary>
    class MoveToGround : Order
    {
        /// <summary>
        /// The target position.
        /// </summary>
        public Vector Target { get; }

        /// <summary>
        /// The distance from the target at which to stop.
        /// </summary>
        public float MinDistance { get; }

        public MoveToGround(Unit u, Vector target, float minDistance = 0) : base(u)
        {
            Target = target;
            MinDistance = minDistance;
        }

        public override bool TakeControl()
        {
            return Owner.Position.DistanceTo(Target) > MinDistance;
        }

        public override void Update(int msElapsed)
        {
            var maxDist = (float)Owner.Position.DistanceTo(Target) - MinDistance;
            var ang = (float)Owner.Position.AngleTo(Target);
            CurrentState = new Shanism.Common.MovementState(ang, maxDist);
        }

        public override string ToString() => $"Following {Target}";
    }
}
