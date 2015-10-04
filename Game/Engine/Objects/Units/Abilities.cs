using Engine.Objects.Game;
using Engine.Systems.Orders;
using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    partial class Unit
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
        /// Gets the ability being currently cast. 
        /// </summary>
        IAbility IUnit.CastingAbility
        {
            get
            {
                if (OrderType == IO.Common.OrderType.Casting)
                    return ((CastOrder)Order).Ability;
                return null;
            }
        }

        /// <summary>
        /// Gets a list of all the abilities of this hero. 
        /// </summary>
        public IEnumerable<Ability> Abilities
        {
            get { return abilities.Values; }
        }


        IEnumerable<IAbility> IUnit.Abilities
        {
            get
            {
                return abilities.Values;
            }
        }

        /// <summary>
        /// Gets the progress of the ability being currently cast, or -1 if the unit is not casting any ability. 
        /// </summary>
        public int CastingProgress
        {
            get
            {
                if (OrderType == IO.Common.OrderType.Casting)
                    return ((CastOrder)Order).Progress;
                return -1;
            }
        }

        /// <summary>
        /// Adds the given ability to the spellbook of this hero. 
        /// </summary>
        /// <param name="a"></param>
        public void AddAbility(Ability a)
        {
            if (a.Owner != null)
                throw new Exception("This ability already belongs to somebody else...");

            if (abilities.ContainsKey(a.Name))
                throw new Exception("Ye already have this ability!");

            if (IsDead)
                return;

            a.Owner = this;
            abilities.Add(a.Name, a);
            OnAbilityGained?.Invoke(a);
        }
        
        /// <summary>
        /// Removes the given ability from the spellbook of this hero. 
        /// </summary>
        public void RemoveAbility(Ability a)
        {
            if (a.Owner != this)
                throw new Exception("This ability is not yours!");

            abilities.Remove(a.Name);
            OnAbilityLost?.Invoke(a);
        }

        /// <summary>
        /// Activates (casts) the specified ability. 
        /// </summary>
        internal bool activateAbility(Ability ability, object target)
        {
            //check if we have the ability
            if (!abilities.ContainsValue(ability))
                throw new InvalidOperationException("Trying to cast a spell that's not ours!");

            if (ability.Owner != this)
                throw new InvalidOperationException("Trying to cast a spell from another guy even thou we have it?!");

            //check boring stuff (mana, life?, cd)
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
    }
}
