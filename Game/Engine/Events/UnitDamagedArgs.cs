using Shanism.Engine.Objects;
using Shanism.Engine.Entities;
using Shanism.Common;
using System;

namespace Shanism.Engine.Events
{
    /// <summary>
    /// The arguments passeed whenever a unit was just damaged.  
    /// </summary>
    public class UnitDamagedArgs : UnitArgs
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

        /// <summary>
        /// Gets the <see cref="DamageFlags"/> flags that specify 
        /// the extra rules when handling the damage instance. 
        /// </summary>
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

        internal UnitDamagedArgs(Unit attacker, Unit receiver, DamageType type, DamageFlags flags, double baseAmount, double finalAmount)
            : base(receiver)
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