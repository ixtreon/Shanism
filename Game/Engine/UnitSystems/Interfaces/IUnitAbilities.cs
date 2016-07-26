using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Abilities;
using System;
using System.Collections.Generic;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// A collection of all the abilities of a unit.  
    /// </summary>
    public interface IUnitAbilities : ICollection<Ability>, IReadOnlyCollection<Ability>
    {

        /// <summary>
        /// The event raised just after a spell was cast. 
        /// </summary>
        event Action OnAbilityCast;

        /// <summary>
        /// The event raised when this unit learns a new ability. 
        /// </summary>
        event Action<Ability> OnAbilityLearned;

        /// <summary>
        /// The event raised when this unit loses an ability. 
        /// </summary>
        event Action<Ability> OnAbilityLost;
        
        /// <summary>
        /// Tries to get the value of the given ability from this unit's spell book. 
        /// Returns null if the ability is not found. 
        /// </summary>
        /// <param name="abilityId">The ID of the ability to look for. </param>
        Ability TryGet(uint abilityId);
    }
}