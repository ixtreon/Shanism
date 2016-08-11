using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Engine.Events;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// An abstract unit order. 
    /// <para>
    /// At each tick the order selects whether to act 
    /// by calling the <see cref="TakeControl"/> method.
    /// 
    /// If that method returns true the behaviour is allowed
    /// to updates its state (see <see cref="Update(int)"/>.
    /// 
    /// The active order issues a <see cref="MovementState"/> and 
    /// may affect its owner in any other way. 
    /// </para>
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
