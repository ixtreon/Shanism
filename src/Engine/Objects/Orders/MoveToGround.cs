using Ix.Math;
using Shanism.Common;
using Shanism.Engine.Entities;
using System.Numerics;

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
        public Vector2 Target { get; set; }

        /// <summary>
        /// The distance from the target at which to stop.
        /// </summary>
        public float MinDistance { get; set; }

        public MoveToGround(Unit owner, Vector2 target, float minDistance = 0) : base(owner)
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
            var maxDist = Owner.Position.DistanceTo(Target) - MinDistance;
            var ang = Owner.Position.AngleTo(Target);

            CurrentState = new MovementState(ang, maxDist);
        }

        public override string ToString() => $"Following {Target}";
    }
}
