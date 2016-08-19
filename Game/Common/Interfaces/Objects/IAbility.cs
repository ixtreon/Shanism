using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Common.Interfaces.Objects
{
    /// <summary>
    /// Represents a passive or active ability that belongs to a single entity. 
    /// </summary>
    /// <seealso cref="IGameObject" />
    public interface IAbility : IGameObject
    {
        /// <summary>
        /// Gets the name of the ability. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the ability. 
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the icon of the ability. 
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// Gets or sets the tint <see cref="Color"/> of this ability's <see cref="Icon"/>.
        /// </summary>
        Color IconTint { get; }

        /// <summary>
        /// Gets the current cooldown of the ability. 
        /// </summary>
        int CurrentCooldown { get; }

        /// <summary>
        /// Gets the total cooldown of the ability. 
        /// </summary>
        int Cooldown { get; }

        /// <summary>
        /// Gets the mana cost of the ability. 
        /// </summary>
        int ManaCost { get; }

        /// <summary>
        /// Gets the casting time of the ability in milliseconds. 
        /// </summary>
        int CastTime { get; }

        /// <summary>
        /// Gets the casting range of the ability in in-game units. 
        /// </summary>
        double CastRange { get; }

        /// <summary>
        /// Gets the target types of this ability, if it is targeted. 
        /// </summary>
        AbilityTargetType TargetType { get; }
    }
}
