using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Common.Message;
using Shanism.Engine.Objects.Orders;

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
            if (Owner.IsDead) return;

            //try current order
            if (tryTakeControl(Owner.CurrentOrder, msElapsed))
                return;

            //otherwise revert to default
            Owner.CurrentOrder = null;
            if (tryTakeControl(Owner.DefaultOrder, msElapsed))
                return;

            //or don't do anything
            if (Owner.MovementState.IsMoving)
                Owner.MovementState = Shanism.Common.MovementState.Stand;
        }

        bool tryTakeControl(Order ord, int msElapsed)
        {
            if (ord == null || !ord.TakeControl())
                return false;

            ord.Update(msElapsed);
            Owner.MovementState = ord.CurrentState;
            return true;
        }

        bool? updateOrder(Order o, int msElapsed)
        {
            if (o == null)
                return null;

            var takeControl = o.TakeControl();
            if (takeControl == true)
                o.Update(msElapsed);
            return takeControl;
        }
    }
}
