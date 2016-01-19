using IO.Common;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Objects
{
    public interface IUnit : IGameObject
    {
        bool IsDead { get; }

        double Life { get; }
        double MaxLife { get; }

        double Mana { get; }
        double MaxMana { get; }

        double MoveSpeed { get; }
        double Defense { get; }
        double MinDamage { get; }
        double MaxDamage { get; }
        double LifeRegen { get; }

        double VisionRange { get; }

        /// <summary>
        /// Gets the mana regeneration rate of the unit. 
        /// </summary>
        double ManaRegen { get; }
        /// <summary>
        /// Gets the bonus magic damage this unit receives. 
        /// </summary>
        double MagicDamage { get; }

        
        /// <summary>
        /// Gets the collection of buffs currently affecting the unit. 
        /// </summary>
        IEnumerable<IBuffInstance> Buffs { get; }

        /// <summary>
        /// Gets the collection of abilities this unit has. 
        /// </summary>
        IEnumerable<IAbility> Abilities { get; }

        /// <summary>
        /// Gets the base defense of the unit. 
        /// </summary>
        double BaseDefense { get; }

        /// <summary>
        /// Gets the attack cooldown of this unit. 
        /// </summary>
        int AttackCooldown { get; }

        /// <summary>
        /// Gets whether this unit is invulnerable. 
        /// </summary>
        bool Invulnerable { get; }

        /// <summary>
        /// Gets the type of this unit's current order. 
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        /// Gets the unit state flags that currently affect this unit. 
        /// </summary>
        UnitState StateFlags { get; }

        /// <summary>
        /// Gets the progress of the ability the unit is currently casting, if <see cref="CastingAbility"/> is not null.  
        /// </summary>
        int CastingProgress { get; }

        /// <summary>
        /// Gets the ability this unit is currently casting or null otherwise. 
        /// </summary>
        IAbility CastingAbility { get; }

        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Gets the owner of the unit. 
        /// </summary>
        IPlayer Owner { get; }

        /// <summary>
        /// Gets whether the unit is currently moving. 
        /// </summary>
        bool IsMoving { get; }

        /// <summary>
        /// Gets the direction in which this unit is moving, if <see cref="IsMoving"/> is true. 
        /// </summary>
        double MoveDirection { get; }
    }
}
