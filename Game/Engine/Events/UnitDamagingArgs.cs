using Shanism.Engine.Entities;
using Shanism.Common;
using System;

namespace Shanism.Engine.Events
{
    /// <summary>
    /// The arguments passed whenever a unit is about to get damaged by some other unit. 
    /// </summary>
    public class UnitDamagingArgs : UnitArgs
    {
        /// <summary>
        /// Gets the unit that is dealing the damage. 
        /// </summary>
        public Unit DamagingUnit { get; }

        /// <summary>
        /// Gets the unit that is receiving the damage. 
        /// </summary>
        public Unit DamagedUnit { get; }

        /// <summary>
        /// Gets or sets the damage flags of the event. 
        /// </summary>
        public DamageFlags DamageFlags { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DamageType"/> of the event. 
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        /// Gets or sets the base amount of damage the attacker deals
        /// before any resistance or armor is factored in. 
        /// </summary>
        public float BaseDamage { get; set; }

        internal UnitDamagingArgs(Unit attacker, Unit receiver, 
            DamageType type, DamageFlags flags, float baseDamage)
            : base(attacker)
        {
            DamagingUnit = attacker;
            DamagedUnit = receiver;

            DamageType = type;
            DamageFlags = flags;
            BaseDamage = baseDamage;
        }
    }
}