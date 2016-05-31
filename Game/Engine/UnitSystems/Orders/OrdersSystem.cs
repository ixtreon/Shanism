using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Common.Message;
using Shanism.Common.Game;

namespace Shanism.Engine.Systems
{
    class OrdersSystem : UnitSystem
    {
        readonly Unit Owner;

        public OrdersSystem(Unit owner)
        {
            Owner = owner;
        }

        internal override void Update(int msElapsed)
        {
            //dead units have no orders
            if (Owner.IsDead
                || Owner.Order == null
                || Owner.States.HasFlag(UnitFlags.Stunned))
                return;

            if (!Owner.Order.Update(Owner, msElapsed))
                Owner.ClearOrder();
        }
    }
}
