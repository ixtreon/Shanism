using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common.Objects;

namespace Shanism.Common.Entities
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


        MovementState MovementState { get; }

        /// <summary>
        /// Gets the unit state flags that currently affect this unit. 
        /// </summary>
        StateFlags StateFlags { get; }

        /// <summary>
        /// Gets the vision range of this unit. 
        /// </summary>
        float VisionRange { get; }

        #endregion

        /// <summary>
        /// Gets the current life of this unit. 
        /// </summary>
        float Life { get; }
        /// <summary>
        /// Gets the current mana of this unit. 
        /// </summary>
        float Mana { get; }

        IUnitStats BaseStats { get; }

        IUnitStats Stats { get; }


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
            return (u.StateFlags & StateFlags.Casting) == StateFlags.Casting;
        }
    }
}
