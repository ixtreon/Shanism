using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Engine.Events;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// Represents a base class for the creation of unit behaviours. 
    /// At each tick the behaviour selects whether to act by calling the <see cref="TakeControl"/> method.
    /// If that method returns true the behaviour updates its state (see <see cref="Update(int)"/>,
    /// and updates its 'suggestion' for the unit's current order (see <see cref="CurrentOrder"/>. 
    /// </summary>
    /// 
    /// <example>
    /// For example, heroes have entirely passive behaviours since they react to player events (if using WASD). 
    /// NPCs on the other hand have more complex behaviours which specify their actions. 
    /// </example>
    public abstract class Order
    {
        /// <summary>
        /// Gets the unit this behaviour controls. 
        /// </summary>
        protected Unit Owner { get; }

        /// <summary>
        /// Gets the current state suggested by the order. 
        /// </summary>
        public MovementState CurrentState { get; protected set; } = MovementState.Stand;

        /// <summary>
        /// Creates a new behaviour for the given unit. 
        /// </summary>
        /// <param name="owner"></param>
        protected Order(Unit owner)
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Creates a new behaviour for the same unit. 
        /// </summary>
        /// <param name="b"></param>
        protected Order(Order b)
         : this(b.Owner)
        { }

        /// <summary>
        /// Returns whether the current order wants to currently act. 
        /// Returns null if the order is complete and will never act.
        /// </summary>
        public abstract bool TakeControl();

        /// <summary>
        /// Causes this behaviour to update its state. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        public abstract void Update(int msElapsed);
    }
}
