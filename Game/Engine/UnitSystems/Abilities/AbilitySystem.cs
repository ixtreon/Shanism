using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Shanism.Common;
using System.Collections.Concurrent;

namespace Shanism.Engine.Systems.Abilities
{
    class AbilitySystem : UnitSystem, IUnitAbilities
    {
        /// <summary>
        /// Contains mapping between ability ids and the abilities themselves. 
        /// </summary>
        readonly ConcurrentDictionary<uint, Ability> abilities = new ConcurrentDictionary<uint, Ability>();

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


        readonly Unit Owner;


        public Ability CastingAbility { get; private set; }

        public object CastingTarget { get; private set; }

        public int CastingProgress { get; private set; }

        public IEnumerable<Ability> Spellbook => abilities.Select(o => o.Value);


        public AbilitySystem(Unit u)
        {
            Owner = u;
        }


        internal override void Update(int msElapsed)
        {
            foreach (var ab in abilities.Values)
                ab.Update(msElapsed);

            //cast ability progress
            var ca = CastingAbility;
            var ct = CastingTarget;

            if(ca != null)
            {
                CastingProgress += msElapsed;

                if (CastingProgress >= ca.CastTime)
                {
                    //cast the spell
                    Owner.abilities.ActivateAbility(ca, ct);

                    StopCasting();
                }
            }
        }


        /// <summary>
        /// Adds the given ability to the spellbook of this hero. 
        /// </summary>
        /// <param name="a">The ability that is to be added. </param>
        public void Add(Ability a)
        {
            if (a.Owner != null)
                throw new Exception("This ability already belongs to someone else...");

            if (abilities.TryAdd(a.Id, a))
            {
                a.SetOwner(Owner);
                OnAbilityLearned?.Invoke(a);
            }
        }

        /// <summary>
        /// Removes the given ability from the spellbook of this hero. 
        /// </summary>
        /// <returns>Whether the ability was successfully found and removed. </returns>
        public bool Remove(Ability a)
        {
            var result = abilities.TryRemove(a.Id, out a);
            if(result)
                OnAbilityLost?.Invoke(a);
            return result;
        }

        public void StopCasting()
        {
            CastingAbility = null;
            CastingTarget = null;
            CastingProgress = 0;
        }

        /// <summary>
        /// Tries to cast the given ability on the provided target. 
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="target"></param>
        public void CastAbility(Ability ability, object target)
        {
            CastingProgress = 0;
            CastingTarget = target;
            CastingAbility = ability;
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
