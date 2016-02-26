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
        /// <summary>
        /// Updates the unit according to this order and returns whether the order should remain. 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="msElapsed"></param>
        /// <returns>True if the order should continue. False if the order has completed. </returns>
        bool Update(Unit u, int msElapsed);

        /// <summary>
        /// Gets this order's type. 
        /// </summary>
        OrderType Type { get; }
    }

    /// <summary>
    /// A base interface for all move orders. 
    /// Defines the direction of the moves, also the final location suggested by the order. 
    /// The latter provides for smoother multiplayerz and npcs. Or does it?
    /// </summary>
    public interface IMoveOrder : IOrder
    {
        /// <summary>
        /// The direction this order moves the unit at.  
        /// </summary>
        double Direction { get; }

        /// <summary>
        /// The final location suggested by the order. 
        /// </summary>
        Vector TargetLocation { get; }
    }
}
