using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// Simply stands at a place. 
    /// </summary>
    struct Stand : IOrder
    {
        public OrderType Type
        {
            get
            {
                return OrderType.Stand;
            }
        }

        public bool Update(Unit u, int msElapsed)
        {
            u.movement.Stop();
            return true;
        }

        public override string ToString() => "Stand";
    }
}
