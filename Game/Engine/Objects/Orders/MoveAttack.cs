using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Common;

namespace Shanism.Engine.Objects.Orders
{
    class MoveAttack : OrderList
    {
        readonly Aggro aggro;
        readonly MoveToGround move;

        readonly Vector startPosition;
        readonly Vector targetPosition;
        private double RetreatDistance = 20;
        private double RetreatStopDistance = 10;
        bool isRetreating;

        public MoveAttack(Unit owner, Vector target) : base(owner)
        {
            startPosition = owner.Position;
            targetPosition = target;

            Add(aggro = new Aggro(owner));
            Add(move = new MoveToGround(owner, target));
        }

        double distFromPath()
            => PointLineDist(startPosition, targetPosition, Owner.Position);

        public override bool TakeControl()
        {
            var d = distFromPath();
            if (isRetreating)
            {
                if (d < RetreatStopDistance)
                {
                    isRetreating = false;
                    aggro.ResetTable();
                }
            }
            else
            {
                if (d > RetreatDistance)
                {
                    isRetreating = true;
                    aggro.ClearTable();
                }
            }
            return base.TakeControl();
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        static double PointLineDist(Vector la, Vector lb, Vector p)
        {
            var d = la.DistanceTo(lb);
            var area = Math.Abs((lb.Y - la.Y) * p.X - (lb.X - la.X) * p.Y + lb.X * la.Y - lb.Y * la.X);
            return area / d;
        }
    }
}
