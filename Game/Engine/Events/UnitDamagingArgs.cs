using Engine.Common;
using Engine.Entities;
using IO.Common;
using System;

namespace Engine.Events
{
    /// <summary>
    /// The arguments passed whenever a unit is about to get damaged by some other unit. 
    /// </summary>
    public class UnitDamagingArgs : EventArgs
    {
        /// <summary>
        /// Gets the unit that is dealing the damage. 
        /// </summary>
        public readonly Unit DamagingUnit;

        /// <summary>
        /// Gets the unit that is receiving the damage. 
        /// </summary>
        public readonly Unit DamagedUnit;


        public DamageFlags DamageFlags { get; set; }


        /// <summary>
        /// Gets or sets the <see cref="DamageType"/> of the attack. 
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        /// Gets or sets the base amount of damage the attacker deals
        /// before any resistance or armor is factored in. 
        /// </summary>
        public double BaseDamage { get; set; }

        public UnitDamagingArgs(Unit attacker, Unit receiver, DamageType type, DamageFlags flags, double baseAmount)
        {
            BaseDamage = baseAmount;
            DamageType = type;
            DamageFlags = flags;
            DamagingUnit = attacker;
            DamagedUnit = receiver;
        }
    }
}