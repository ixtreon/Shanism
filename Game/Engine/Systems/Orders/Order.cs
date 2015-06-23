using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// A base class for all orders. 
    /// </summary>
    public interface Order
    {
        bool Update(Unit u, int msElapsed);

        OrderType Type { get; }

    }
}
