using Shanism.Engine.Events;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using System.Diagnostics;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// A compound behaviour which makes an unit engage other units that come into range.
    /// Switches between following and attacking the current target with max aggro, unless it escapes too far away.
    /// In peaceful times returns to the "last peaceful position" and then releases control.
    /// </summary>
    class Guard : OrderList
    {
        const int originUpdateDistSq = 1;

        readonly ReturnOrder StayInRange;           // return to origin if target escapes too far
        readonly Aggro Aggravate;                   // attack targets in range
        readonly ReturnOrder ReturnToOrigin;        // return to the origin if nothing else is to be done

        public double ReturnRange { get; set; }


        public Guard(Unit u, float range)
            : base(u)
        {
            ReturnRange = range;

            StayInRange = new ReturnOrder(this, u.Position, ReturnRange);
            Aggravate = new Aggro(Owner);
            ReturnToOrigin = new ReturnOrder(this, u.Position, 0.5);

            AddRange(new Order[]
            {
                StayInRange,
                Aggravate,
                ReturnToOrigin,
            });

            StayInRange.OnReturnStarted += ReturnBehaviour_OnReturnStarted;
            StayInRange.OnReturnFinished += ReturnBehaviour_OnReturnFinished;
        }

        public override bool TakeControl()
        {
            //update return locations in peaceful times
            if (CurrentBehaviour == null
                && StayInRange.ReturnPosition.DistanceToSquared(Owner.Position) > originUpdateDistSq)
            {
                Debug.Assert(Aggravate.CurrentTarget == null);

                ReturnToOrigin.ReturnPosition = Owner.Position;
                StayInRange.ReturnPosition = Owner.Position;
            }
            return base.TakeControl();
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        private void ReturnBehaviour_OnReturnFinished()
        {
            //re-add all visible entities to the aggro table
            Aggravate.ResetTable();

            //reset life?!
            Owner.Life = Owner.MaxLife;
        }

        private void ReturnBehaviour_OnReturnStarted()
        {
            //clear the aggro table
            Aggravate.ClearTable();
        }
    }
}
