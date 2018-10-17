using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Orders;
using Shanism.Common;

namespace Shanism.Engine.Systems
{
    class OrderSystem : UnitSystem
    {
        readonly Unit Owner;

        public OrderSystem(Unit owner)
        {
            Owner = owner;
        }

        public override void Update(int msElapsed)
        {
            if(Owner.IsDead) 
                return;

            //try current order
            if(tryUpdate(msElapsed, Owner.CurrentOrder))
                return;

            //otherwise revert to default
            Owner.CurrentOrder = null;
            if(tryUpdate(msElapsed, Owner.DefaultOrder))
                return;

            //or don't do anything
            Owner.MovementState = MovementState.Stand;
        }

        bool tryUpdate(int msElapsed, Order order)
        {
            if(order == null || !order.TakeControl())
                return false;

            order.Update(msElapsed);
            Owner.MovementState = order.CurrentState;
            return true;
        }
    }
}
