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
    struct Stand : Order
    {
        //public Stand()
        //{
        //}

        public OrderType Type
        {
            get
            {
                return OrderType.Stand;
            }
        }

        public bool Update(Unit u, int msElapsed)
        {
            return true;
        }
    }
}
