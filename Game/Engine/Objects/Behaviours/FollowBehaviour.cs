using Shanism.Engine.Entities;
using Shanism.Engine.Systems.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Behaviours
{
    /// <summary>
    /// A behaviour that makes the controlled unit follow the specified target unit. 
    /// </summary>
    class FollowBehaviour : Behaviour
    {
        MoveUnit followOrder;

        public Unit Target { get; set; }

        public double Distance { get; set; }

        double DistanceSquared => Distance * Distance;

        public FollowBehaviour(Behaviour b) : base(b)
        {
        }

        public override bool TakeControl()
        {
            if (Target == null)
                return false;

            var dist = Owner.Position.DistanceToSquared(Target.Position);
            if (dist < DistanceSquared)
                return false;

            if(followOrder == null 
                || followOrder.TargetUnit != Target 
                || followOrder.DistanceThrehsold != Distance)
                followOrder = new MoveUnit(Target, Distance, true);

            return true;
        }

        public override void Update(int msElapsed)
        {
            CurrentOrder = followOrder;
        }

        public override string ToString() => $"Following {Target}";
    }
}
