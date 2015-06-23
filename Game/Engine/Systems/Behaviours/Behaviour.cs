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
        private readonly Unit unit;

        public IUnit Unit
        {
            get { return unit; }
        }

        public Order CurrentOrder { get; set; }

        public bool IsActive { get; protected set; }

        public Behaviour(Unit u)
        {
            this.unit = u;

            unit.UnitInVisionRange += OnUnitInVisionRange;
            unit.DamageReceived += OnDamageReceived;
        }

        public Behaviour(Behaviour b)
         : this(b.unit)
        { }

        protected virtual void OnDamageReceived(UnitDamagedArgs args) { }

        protected virtual void OnUnitInVisionRange(RangeArgs args) { }

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
