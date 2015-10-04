using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// A base interface for all orders. 
    /// </summary>
    public interface IOrder
    {
        bool Update(Unit u, int msElapsed);

        OrderType Type { get; }
    }

    /// <summary>
    /// A base interface for all move orders. Defines the direction of the moves. 
    /// </summary>
    public interface IMoveOrder : IOrder
    {
        double Direction { get; }

        Vector SuggestedLocation { get; }
    }
}
