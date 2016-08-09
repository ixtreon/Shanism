using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Shanism.Common;
using Shanism.Engine.Objects.Abilities;
using System.Threading;
using Shanism.Common.Game;
using System.Diagnostics;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Keeps and updates the abilities of an unit. Keeps track of casting abilties. 
    /// </summary>
    /// <seealso cref="Shanism.Engine.Systems.UnitSystem" />
    /// <seealso cref="Shanism.Engine.Systems.IUnitAbilities" />
    class AbilitySystem : UnitSystem, IUnitAbilities
    {

        readonly Unit Owner;

        /// <summary>
        /// Ability ids mapped to the abilities themselves. 
        /// </summary>
        readonly Dictionary<uint, Ability> abilities = new Dictionary<uint, Ability>();


        CastingData castData;

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



        public Ability CastingAbility => castData?.Ability;

        public int CastingProgress => castData?.Progress ?? 0;




        public AbilitySystem(Unit u)
        {
            Owner = u;
        }


        public override void Update(int msElapsed)
        {
            foreach (var kvp in abilities)
                kvp.Value.Update(msElapsed);

            //continue only if casting
            if (castData == null)
                return;

            //interrupt casting if moving or incapacitated
            if (Owner.IsStunned
                || (Owner.MovementState.IsMoving && !castData.Ability.CanCastWalk))
            {
                StopCasting();
                return;
            }

            //break if still casting
            if (!castData.UpdateProgress(msElapsed))
                return;

            //cast the spell if no other spell was cast in the meantime
            //execute and check custom ability handlers
            if (castData.Invoke())
                OnAbilityCast?.Invoke();

            StopCasting();
        }



        #region ICollection<T> implementation

        public int Count => abilities.Count;

        bool ICollection<Ability>.IsReadOnly => false;

        /// <summary>
        /// Adds the given ability to the spellbook of this hero. 
        /// </summary>
        /// <param name="a">The ability that is to be added. </param>
        public void Add(Ability a)
        {
            if (a.Owner != null)
                throw new InvalidOperationException($"The ability {a} already belongs to the unit {Owner}.");

            abilities[a.Id] = a;
            a.SetOwner(Owner);
            OnAbilityLearned?.Invoke(a);
        }

        /// <summary>
        /// Removes the given ability from the spellbook of this hero. 
        /// </summary>
        /// <returns>Whether the ability was successfully found and removed. </returns>
        public bool Remove(Ability a)
        {
            if (a.Owner != Owner)
                throw new InvalidOperationException($"The ability {a} does not belong to the unit {Owner}.");

            var result = abilities.Remove(a.Id);
            if (result)
            {
                a.SetOwner(null);
                OnAbilityLost?.Invoke(a);
            }
            return result;
        }

        public void Clear()
        {
            foreach (var kvp in abilities)
            {
                kvp.Value.SetOwner(null);
                OnAbilityLost?.Invoke(kvp.Value);
            }
            abilities.Clear();
        }

        public bool Contains(Ability item) 
            => abilities.ContainsKey(item.Id);

        void ICollection<Ability>.CopyTo(Ability[] array, int arrayIndex)
            => abilities.Values.CopyTo(array, arrayIndex);

        IEnumerator<Ability> IEnumerable<Ability>.GetEnumerator() => abilities.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => abilities.Values.GetEnumerator();



        /// <summary>
        /// Tries to get the value of the given ability from this unit's spell book. 
        /// Returns null if the ability is not found. 
        /// </summary>
        /// <param name="abilityId">The ID of the ability to look for. </param>
        public Ability TryGet(uint abilityId)
        {
            return abilities.TryGet(abilityId);
        }
        #endregion

        #region Casting

        public void StopCasting()
        {
            castData = null;
            Owner.StateFlags &= ~StateFlags.Casting;
        }

        void setCastData(CastingData cd)
        {
            Debug.Assert(cd != null);

            if (castData == null || !castData.Equals(cd))
            {
                castData = cd;
                Owner.StateFlags |= StateFlags.Casting;
            }
        }

        /// <summary>
        /// Tries to cast the given no-target ability.
        /// </summary>
        /// <param name="ability"></param>
        public bool BeginCasting(Ability ability)
        {
            throwIfAbilityNotOurs(ability);

            if (!ability.CanCast())
                return false;

            setCastData(new CastingData(ability, Owner.Position));
            return true;
        }

        public bool BeginCasting(Ability ability, Vector p)
        {
            throwIfAbilityNotOurs(ability);

            if (!ability.CanCast(p))
                return false;

            setCastData(new CastingData(ability, p));
            return true;
        }

        public bool BeginCasting(Ability ability, Entity e)
        {
            throwIfAbilityNotOurs(ability);

            if (!ability.CanCast(e))
                return false;

            setCastData(new CastingData(ability, e));
            return true;
        }

        #endregion


        void throwIfAbilityNotOurs(Ability ability)
        {
            if (ability?.Owner != Owner)
                throw new ArgumentException($"The unit `{Owner}` cannot cast the ability `{ability}`.", nameof(ability));
        }


        #region IEnumerable<Ability> Implementation
        #endregion


    }
}
