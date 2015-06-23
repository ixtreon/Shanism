using Engine.Objects;
using Engine.Objects.Game;
using IO.Common;
using System;

namespace Engine.Events
{
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

        /// <summary>
        /// Gets the base amount of damage the attacker deals, 
        /// before any resistance or armor checks. 
        /// </summary>
        public readonly double BaseDamage;

        /// <summary>
        /// Gets or sets the amount of damage received. 
        /// </summary>
        public readonly double FinalDamage;

        public UnitDamagedArgs(Unit attacker, Unit receiver, DamageType type, double baseAmount, double finalAmount)
        {
            BaseDamage = baseAmount;
            FinalDamage = finalAmount;
            DamageType = type;
            DamagingUnit = attacker;
            DamagedUnit = receiver;
        }
    }
}