using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Message;
using IO.Common;

namespace Engine.Systems
{
    class OrdersSystem : ShanoSystem
    {
        public OrdersSystem(ShanoEngine game) : base(game)
        {
        }


        internal override void UpdateObject(int msElapsed, GameObject obj)
        {
            var u = obj as Unit;
            if (u == null) return;

            //dead units have no orders
            if (u.IsDead)
                return;

            //stunned units are useless
            if (u.StateFlags.HasFlag(UnitState.Stunned))
                return;

            updateBehaviour(msElapsed, u);
            updateOrder(msElapsed, u);
        }

        static void updateBehaviour(int msElapsed, Unit u)
        {
            if (u.Behaviour == null)
                return;

            u.Behaviour.Update(msElapsed);
            var toContinue = !u.CustomOrder && u.Behaviour.CurrentOrder != u.Order;

            if (toContinue)
                u.SetOrder(u.Behaviour.CurrentOrder, false);
        }

        static void updateOrder(int msElapsed, Unit u)
        {
            if (u.Order == null)
                return;

            var toContinue = u.Order.Update(u, msElapsed);

            if (!toContinue)
                u.OrderStand();
        }
    }
}
