using Engine.Entities;
using System;
using System.Collections.Generic;

namespace Engine.Systems.Abilities
{
    /// <summary>
    /// A collection of all the abilities of a unit.  
    /// </summary>
    public interface IUnitAbilities : IEnumerable<Ability>
    {
        /// <summary>
        /// The unit who owns these abilities. 
        /// </summary>
        Unit Owner { get; }


        /// <summary>
        /// The event raised just after a spell was cast. 
        /// </summary>
        event Action OnAbilityCast;

        /// <summary>
        /// The event raised when this unit learns a new ability. 
        /// </summary>
        event Action<Ability> OnAbilityGained;

        /// <summary>
        /// The event raised when this unit loses an ability. 
        /// </summary>
        event Action<Ability> OnAbilityLost;


        /// <summary>
        /// Adds the given ability to the spellbook of this hero. 
        /// </summary>
        /// <param name="a">The ability that is to be added. </param>
        void Add(Ability a);

        /// <summary>
        /// Removes the given ability from the spellbook of this hero. 
        /// </summary>
        /// <returns>Whether the ability was successfully found and removed. </returns>
        bool Remove(Ability a);


        /// <summary>
        /// Tries to get the value of the given ability from this unit's spell book. 
        /// Returns null if the ability is not found. 
        /// </summary>
        /// <param name="key">The name of the ability to look for. </param>
        Ability TryGet(string abilityId);
    }
}