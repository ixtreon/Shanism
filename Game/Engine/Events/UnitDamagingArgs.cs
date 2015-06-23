using Engine.Objects;
using IO.Common;
using System;

namespace Engine.Events
{
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

        /// <summary>
        /// Gets or sets the <see cref="DamageType"/> of the attack. 
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        /// Gets or sets the base amount of damage the attacker deals, 
        /// before any resistance or armor checks. 
        /// </summary>
        public double BaseDamage { get; set; }

        public UnitDamagingArgs(Unit attacker, Unit receiver, DamageType type, double baseAmount)
        {
            BaseDamage = baseAmount;
            DamageType = type;
            DamagingUnit = attacker;
            DamagedUnit = receiver;
        }
    }
}