using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common.Interfaces.Objects;

namespace Shanism.Common.Interfaces.Entities
{

    /// <summary>
    /// Represents an in-game unit. This includes NPCs, heroes, buildings. 
    /// </summary>
    public interface IUnit : IEntity
    {
        #region General

        /// <summary>
        /// Gets the owner of the unit. 
        /// </summary>
        uint OwnerId { get; }

        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Gets whether this unit is dead. 
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// Gets the type of this unit's current order. 
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        /// Gets the unit state flags that currently affect this unit. 
        /// </summary>
        StateFlags States { get; }

        /// <summary>
        /// Gets the vision range of this unit. 
        /// </summary>
        double VisionRange { get; }

        #endregion

        #region Life & Mana

        /// <summary>
        /// Gets the current life of this unit. 
        /// </summary>
        double Life { get; }

        /// <summary>
        /// Gets the maximum life of this unit. 
        /// </summary>
        double MaxLife { get; }

        /// <summary>
        /// Gets the life regeneration of this unit in points per seecond. 
        /// </summary>
        double LifeRegen { get; }


        /// <summary>
        /// Gets the current mana of this unit. 
        /// </summary>
        double Mana { get; }

        /// <summary>
        /// Gets the maximum mana of this unit. 
        /// </summary>
        double MaxMana { get; }

        /// <summary>
        /// Gets the mana regeneration of this unit in points per seecond. 
        /// </summary>
        double ManaRegen { get; }

        #endregion

        #region Movement

        /// <summary>
        /// Gets whether the unit is currently moving. 
        /// </summary>
        bool IsMoving { get; }

        /// <summary>
        /// Gets the direction in which this unit is moving, if <see cref="IsMoving"/> is true. 
        /// </summary>
        double MoveDirection { get; }

        /// <summary>
        /// Gets the movement speed of this unit. 
        /// </summary>
        double MoveSpeed { get; }
        #endregion

        #region Combat

        /// <summary>
        /// Gets the attack cooldown of this unit in milliseconds. 
        /// </summary>
        int AttackCooldown { get; }

        /// <summary>
        /// Gets the defense of this unit. 
        /// </summary>
        double Defense { get; }

        /// <summary>
        /// Gets the minimum damage dealt by this unit. 
        /// </summary>
        double MinDamage { get; }

        /// <summary>
        /// Gets the maximum damage dealt by this unit. 
        /// </summary>
        double MaxDamage { get; }

        /// <summary>
        /// Gets the bonus magic damage this unit deals. 
        /// </summary>
        double MagicDamage { get; }
        #endregion

        #region Casting

        /// <summary>
        /// Gets the ability this unit is currently casting or null otherwise. 
        /// </summary>
        uint CastingAbilityId { get; }

        /// <summary>
        /// Gets the progress of the ability the unit is currently casting.  
        /// </summary>
        int CastingProgress { get; }

        /// <summary>
        /// Gets the total casting time of the ability the unit is currently casting.  
        /// </summary>
        int TotalCastingTime { get; }

        #endregion

        #region NYI

        //IReadOnlyCollection<IItem> BackpackItems { get; }

        //IReadOnlyDictionary<EquipSlot, IItem> EquipItems { get; }

        /// <summary>
        /// Gets the collection of buffs currently affecting this unit. 
        /// </summary>
        IReadOnlyCollection<IBuffInstance> Buffs { get; }

        /// <summary>
        /// Gets all abilities this unit possesses. 
        /// </summary>
        IReadOnlyCollection<IAbility> Abilities { get; }
        #endregion
    }

    public static class IUnitExt
    {
        public static bool IsCasting(this IUnit u)
        {
            return (u.States & StateFlags.Casting) == StateFlags.Casting;
        }

        public static bool IsInvulnerable(this IUnit u)
        {
            return (u.States & StateFlags.Invulnerable) == StateFlags.Invulnerable;
        }
    }
}
