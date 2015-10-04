using Engine.Objects;
using Engine.Systems.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Events;
using IO;

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
        readonly Unit unit;


        /// <summary>
        /// Gets the unit this behaviour controls. 
        /// </summary>
        public IUnit Unit
        {
            get { return unit; }
        }

        /// <summary>
        /// Gets the current order suggested by the behaviour. 
        /// </summary>
        public IOrder CurrentOrder { get; protected set; }

        /// <summary>
        /// Creates a new behaviour for the given unit. 
        /// </summary>
        /// <param name="u"></param>
        public Behaviour(Unit u)
        {
            this.unit = u;

            unit.UnitInVisionRange += OnUnitInVisionRange;
            unit.DamageReceived += OnDamageReceived;
        }

        public Behaviour(Behaviour b)
         : this(b.unit)
        { }

        /// <summary>
        /// The event fired whenever the controlled unit takes damage. 
        /// </summary>
        protected virtual void OnDamageReceived(UnitDamagedArgs args) { }

        /// <summary>
        /// The event fired whenever the controlled unit takes damage. 
        /// </summary>
        protected virtual void OnUnitInVisionRange(RangeArgs<Unit> args) { }

        /// <summary>
        /// Returns whether the current behaviour should take control of the character. 
        /// </summary>
        /// <param name="msElapsed"></param>
        /// <returns></returns>
        public abstract bool TakeControl(int msElapsed);

        public abstract void Update(int msElapsed);

        //public virtual void OnStopped() { }

        //public virtual void OnStarted() { }
    }
}
