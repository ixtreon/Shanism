using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using IO;
using System.Collections.Concurrent;

namespace Engine.Systems.Abilities
{
    class AbilitySystem : UnitSystem, IUnitAbilities
    {
        /// <summary>
        /// Contains mapping between ability ids and the abilities themselves. 
        /// </summary>
        protected ConcurrentDictionary<uint, Ability> abilities = new ConcurrentDictionary<uint, Ability>();

        /// <summary>
        /// The event raised just after a spell was cast. 
        /// </summary>
        public event Action OnAbilityCast;

        /// <summary>
        /// The event raised when this unit learns a new ability. 
        /// </summary>
        public event Action<Ability> OnAbilityLearned;

        /// <summary>
        /// The event raised when this unit loses an ability. 
        /// </summary>
        public event Action<Ability> OnAbilityLost;


        /// <summary>
        /// The unit who owns these abilities. 
        /// </summary>
        public Unit Owner { get; }


        public AbilitySystem(Unit u)
        {
            Owner = u;
        }



        internal override void Update(int msElapsed)
        {
            foreach (var ab in abilities.Values)
                ab.Update(msElapsed);
        }


        /// <summary>
        /// Adds the given ability to the spellbook of this hero. 
        /// </summary>
        /// <param name="a">The ability that is to be added. </param>
        public void Add(Ability a)
        {
            if (a.Owner != null)
                throw new Exception("This ability already belongs to somebody else...");

            if (!abilities.TryAdd(a.Id, a))
                throw new Exception("Ye already have this ability!");

            a.SetOwner(Owner);
            OnAbilityLearned?.Invoke(a);
        }

        /// <summary>
        /// Removes the given ability from the spellbook of this hero. 
        /// <para/>
        /// If this ability is not owned by the unit, 
        /// its ability list is checked for an ability with the same id. 
        /// </summary>
        /// <returns>Whether the ability was successfully found and removed. </returns>
        public bool Remove(Ability a)
        {
            var result = abilities.TryRemove(a.Id, out a);
            if(result)
                OnAbilityLost?.Invoke(a);
            return result;
        }

        /// <summary>
        /// Activates (casts) the specified ability. 
        /// <para/>
        /// If this ability is not owned by the unit, 
        /// its ability list is checked for an ability with the same id. 
        /// </summary>
        internal bool ActivateAbility(Ability ability, object target)
        {
            //fetch the ability with the same id from the ability dict
            if (!abilities.TryGetValue(ability.Id, out ability))
                throw new InvalidOperationException("The given ability does not belong to this unit!");

            //check if we own the ability
            if (ability.Owner != Owner)
                throw new InvalidOperationException("Trying to cast a spell from another guy even thou we have it?!");

            //check boring stuff (mana, cd, others?)
            if (!ability.CanCast(target))
                return false;

            //execute and check custom ability handlers
            var result = ability.Cast(target);
            if (!result.Success)
                return false;

            //finally activate cooldown, remove mana
            ability.trigger();

            OnAbilityCast?.Invoke();
            return true;
        }

        /// <summary>
        /// Tries to get the value of the given ability from this unit's spell book. 
        /// Returns null if the ability is not found. 
        /// </summary>
        /// <param name="abilityId">The ID of the ability to look for. </param>
        public Ability TryGet(uint abilityId)
        {
            return abilities.TryGet(abilityId);
        }


        #region IEnumerable<Ability> Implementation
        public IEnumerator<Ability> GetEnumerator() => abilities.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => abilities.Values.GetEnumerator();
        #endregion


    }
}
