using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// A behaviour that makes the controlled unit go to the specified target unit. 
    /// </summary>
    class MoveToUnit : Order
    {
        public Unit Target { get; set; }

        public float MinDistance { get; set; }

        public MoveToUnit(Unit u) : base(u) { }

        public MoveToUnit(Unit u, Unit target) : this(u)
        {
            Target = target;
        }

        public override bool TakeControl()
        {
            if (Target == null)
                return false;

            return (float)Owner.Position.DistanceTo(Target.Position) > MinDistance;
        }

        public override void Update(int msElapsed)
        {
            var maxDist = (float)Owner.Position.DistanceTo(Target.Position) - MinDistance;
            var ang = (float)Owner.Position.AngleTo(Target.Position);
            CurrentState = new Shanism.Common.MovementState(ang, maxDist);
        }

        public override string ToString() => $"Following {Target}";
    }
}
