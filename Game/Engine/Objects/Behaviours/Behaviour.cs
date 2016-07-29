using Shanism.Engine.Entities;
using Shanism.Engine.Systems.Orders;
using Shanism.Engine.Events;

namespace Shanism.Engine.Objects.Behaviours
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
    public abstract class Behaviour
    {
        /// <summary>
        /// Gets the unit this behaviour controls. 
        /// </summary>
        protected readonly Unit Owner;

        /// <summary>
        /// Gets the current order suggested by the behaviour. 
        /// </summary>
        public IOrder CurrentOrder { get; protected set; }

        /// <summary>
        /// Creates a new behaviour for the given unit. 
        /// </summary>
        /// <param name="owner"></param>
        protected Behaviour(Unit owner)
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Creates a new behaviour for the same unit. 
        /// </summary>
        /// <param name="b"></param>
        protected Behaviour(Behaviour b)
         : this(b.Owner)
        { }

        /// <summary>
        /// Returns whether the current behaviour should take control of its owner. 
        /// </summary>
        public abstract bool TakeControl();

        /// <summary>
        /// Causes this behaviour to update its state. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        public abstract void Update(int msElapsed);
    }
}
