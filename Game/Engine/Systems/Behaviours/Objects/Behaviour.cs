using Engine.Objects;
using Engine.Systems.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Events;
using IO;
using IO.Objects;

namespace Engine.Systems.Behaviours
{
    /// <summary>
    /// Represents a base class for the creation of unit behaviours. 
    /// 
    /// For example, heroes have entirely passive behaviours since they react to player events (if using WASD). 
    /// NPCs on the other hand have more complex behaviours which specify their actions. 
    /// </summary>
    public abstract class Behaviour
    {
        /// <summary>
        /// Gets the unit this behaviour controls. 
        /// </summary>
        protected Unit Owner { get; }

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

            this.Owner.ObjectSeen += OnObjectSeen;
            this.Owner.DamageReceived += OnDamageReceived;
        }

        private void OnObjectSeen(Entity obj)
        {
            if (obj is Unit)
                OnUnitInVisionRange((Unit)obj);
        }

        /// <summary>
        /// Creates a new behaviour for the same unit. 
        /// </summary>
        /// <param name="b"></param>
        protected Behaviour(Behaviour b)
         : this(b.Owner)
        { }

        /// <summary>
        /// The event fired whenever the controlled unit takes damage. 
        /// </summary>
        protected virtual void OnDamageReceived(UnitDamagedArgs args) { }

        /// <summary>
        /// The event fired whenever a unit comes in range. 
        /// </summary>
        protected virtual void OnUnitInVisionRange(Unit unit) { }

        /// <summary>
        /// Returns whether the current behaviour should take control of the character. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        /// <returns></returns>
        public abstract bool TakeControl(int msElapsed);

        /// <summary>
        /// Causes this behaviour to update its state. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        public abstract void Update(int msElapsed);

        //public virtual void OnStopped() { }

        //public virtual void OnStarted() { }
    }
}
