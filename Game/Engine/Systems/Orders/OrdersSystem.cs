using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Entities;
using IO.Message;
using IO.Common;

namespace Engine.Systems
{
    class OrdersSystem : UnitSystem
    {
        public Unit Owner { get; }

        public OrdersSystem(Unit owner)
        {
            Owner = owner;
        }

        internal override void Update(int msElapsed)
        {
            //dead units have no orders
            if (Owner.IsDead)
                return;

            //stunned units are useless
            if (Owner.StateFlags.HasFlag(UnitFlags.Stunned))
                return;

            if (Owner.Order == null)
                return;

            if (!Owner.Order.Update(Owner, msElapsed))
                Owner.OrderStand();
        }
    }
}
