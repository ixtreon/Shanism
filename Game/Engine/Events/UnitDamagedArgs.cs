using Engine.Common;
using Engine.Objects;
using Engine.Objects.Entities;
using IO.Common;
using System;

namespace Engine.Events
{
    /// <summary>
    /// The arguments passeed whenever a unit was just damaged.  
    /// </summary>
    public class UnitDamagedArgs : EventArgs
    {
        /// <summary>
        /// Gets the unit that is dealing the damage. 
        /// </summary>
        public readonly Unit DamagingUnit;

        /// <summary>
        /// Gets the unit that is receiving the damage. 
        /// </summary>
        public readonly Unit DamagedUnit;


        /// <summary>
        /// Gets the <see cref="DamageType"/> of the attack. 
        /// </summary>
        public readonly DamageType DamageType;


        public readonly DamageFlags DamageFlags;

        /// <summary>
        /// Gets the base amount of damage the attacker deals
        /// before any resistance or armor is factored in. 
        /// </summary>
        public readonly double BaseDamage;


        /// <summary>
        /// Gets the amount of damage ultimately received by the target. 
        /// </summary>
        public readonly double FinalDamage;

        public UnitDamagedArgs(Unit attacker, Unit receiver, DamageType type, DamageFlags flags, double baseAmount, double finalAmount)
        {
            BaseDamage = baseAmount;
            FinalDamage = finalAmount;
            DamageType = type;
            DamageFlags = flags;
            DamagingUnit = attacker;
            DamagedUnit = receiver;
        }
    }
}