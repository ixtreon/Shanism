using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using IO;

namespace Engine.Systems.Abilities
{
    class AbilitySystem : UnitSystem, IUnitAbilities
    {
        /// <summary>
        /// Contains mapping between ability ids (strings) and the spells. 
        /// </summary>
        protected Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();

        /// <summary>
        /// The event raised just after a spell was cast. 
        /// </summary>
        public event Action OnAbilityCast;

        /// <summary>
        /// The event raised when this unit learns a new ability. 
        /// </summary>
        public event Action<Ability> OnAbilityGained;

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

            if (abilities.ContainsKey(a.Name))
                throw new Exception("Ye already have this ability!");

            a.SetOwner(Owner);

            abilities.Add(a.Name, a);
            OnAbilityGained?.Invoke(a);
        }

        /// <summary>
        /// Removes the given ability from the spellbook of this hero. 
        /// </summary>
        /// <returns>Whether the ability was successfully found and removed. </returns>
        public bool Remove(Ability a)
        {
            if (a.Owner != Owner)
                throw new InvalidOperationException("The given ability does not belong to this unit!");

            var result = abilities.Remove(a.Name);
            if(result)
                OnAbilityLost?.Invoke(a);
            return result;
        }

        /// <summary>
        /// Activates (casts) the specified ability. 
        /// </summary>
        internal bool ActivateAbility(Ability ability, object target)
        {
            //check if we have the ability
            if (!abilities.ContainsValue(ability))
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
        /// <param name="key">The name of the ability to look for. </param>
        public Ability TryGet(string abilityId)
        {
            return abilities.TryGet(abilityId);
        }


        #region IEnumerable<Ability> Implementation
        public IEnumerator<Ability> GetEnumerator()
        {
            return abilities.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return abilities.Values.GetEnumerator();
        }
        #endregion


    }
}
