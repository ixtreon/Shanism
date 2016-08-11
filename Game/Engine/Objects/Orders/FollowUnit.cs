using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// A behaviour that makes the controlled unit follow the specified target unit. 
    /// </summary>
    class FollowUnit : Order
    {
        public Unit Target { get; }

        public float MinDistance { get; set; }

        public FollowUnit(Unit owner) : base(owner) { }

        public FollowUnit(Unit owner, Unit target, float minDistance = 0) 
            : this(owner)
        {
            Target = target;
            MinDistance = minDistance;
        }

        public override bool TakeControl()
        {
            return Target != null && !Target.IsDead;
        }

        public override void Update(int msElapsed)
        {
            var distanceLeft = (float)Owner.Position.DistanceTo(Target.Position) - MinDistance;
            if (distanceLeft < 0)
            {
                var ang = (float)Owner.Position.DistanceTo(Target.Position);
                CurrentState = new Shanism.Common.MovementState(ang, distanceLeft);
            }
        }

        public override string ToString() => $"Following {Target}";
    }
}
