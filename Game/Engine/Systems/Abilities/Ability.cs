using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using Engine.Objects;
using IO;
using IO.Common;
using Engine.Events;

namespace Engine.Objects.Game
{
    public abstract class Ability : ScenarioObject, IAbility
    {

        /// <summary>
        /// Gets the hero who owns this ability. 
        /// </summary>
        public Unit Owner { get; internal set; }

        /// <summary>
        /// Gets or sets the name of this ability. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description text of this ability. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the icon of this ability. 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets the currently remaining cooldown of this ability. 
        /// </summary>
        public int CurrentCooldown { get; internal set; }

        /// <summary>
        /// Gets or sets the cooldown of this ability in milliseconds. 
        /// </summary>
        public int Cooldown { get; set; }

        /// <summary>
        /// Gets or sets the mana cost of this ability. 
        /// </summary>
        public int ManaCost { get; set; }


        /// <summary>
        /// Gets or sets the casting time of the ability, in milliseconds. 
        /// </summary>
        public int CastTime { get; set; }

        /// <summary>
        /// Gets or sets the casting range of the ability in units. 
        /// </summary>
        public double CastRange { get; set; }

        /// <summary>
        /// Gets or sets whether the ability can be cast. 
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets whether ability requires a target, if it is active. 
        /// </summary>
        public bool IsTargeted { get; set; }

        /// <summary>
        /// Gets or sets the target types of this ability, if it is targeted. 
        /// </summary>
        public AbilityTargetType TargetType { get; set; }

        /// <summary>
        /// Gets or sets the AoE of the 
        /// </summary>
        public int TargetAoE { get; set; }



        ///// <summary>
        ///// Gets the <see cref="Type"/> of this ability. 
        ///// </summary>
        //public readonly AbilityType Type;

        private Ability()
        {
            this.Icon = "default";
        }

        /// <summary>
        /// Creates a new non-target ability. Can be either a passive or an active, instant cast ability. 
        /// </summary>
        /// <param name="isActive"></param>
        public Ability(bool isActive)
            : this()
        {
            IsActive = isActive;
            IsTargeted = false;

            TargetType = AbilityTargetType.NoTarget;
            
            this.CastRange = 5;
            this.ManaCost = 1;
            Cooldown = 500;
        }

        /// <summary>
        /// Creates a new active, targeted ability. 
        /// </summary>
        /// <param name="targetType"></param>
        public Ability(AbilityTargetType targetType)
            : this(true)
        {
            IsTargeted = true;
            TargetType = targetType;

        }

        internal AbilityCastArgs Cast(object target)
        {
            var e = new AbilityCastArgs(this, target);

            OnCast(e);

            return e;
        }

        /// <summary>
        /// Gets whether this ability can target another unit. 
        /// </summary>
        public bool CanTargetUnits()
        {
            return TargetType == AbilityTargetType.UnitTarget || TargetType == AbilityTargetType.PointOrUnitTarget;
        }

        /// <summary>
        /// Gets whether this ability can target a location the ground. 
        /// </summary>
        public bool CanTargetGround()
        {
            return TargetType == AbilityTargetType.PointTarget || TargetType == AbilityTargetType.PointOrUnitTarget;
        }

        //only one of the following three will get called by the engine. 
        //make sure to override the correct one, 
        //depending on your choice for an abilityType

        /// <summary>
        /// Can be overridden in derived classes to implement custom functionality. 
        /// </summary>
        public virtual void OnCast(AbilityCastArgs e) { }

        public virtual void OnUpdate(int msElapsed) { }

        public bool CanCast(object target)
        {
            if (target is Unit && ((Unit)target).IsDead)
                return false;

            //check if in control (no stuns or stuff)
            //TODO

            //check range
            if (!checkRange(target))
                return false;

            //check cooldown
            if (CurrentCooldown > 0)
                return false;

            //check mana, (TODO: also life?) cost
            if (Owner.Mana < ManaCost)
                return false;

            return true;
        }

        internal override void Update(int msElapsed)
        {
            CurrentCooldown = Math.Max(0, CurrentCooldown - msElapsed);

            OnUpdate(msElapsed);
        }

        /// <summary>
        /// Triggers the ability cooldown, mana cost. 
        /// </summary>
        internal void trigger()
        {
            CurrentCooldown = Cooldown;
            Owner.Mana -= ManaCost;
        }

        private bool checkRange(object target)
        {
            var tLoc = getTargetLocation(target);
            if (!tLoc.HasValue)
                return true;

            return tLoc.Value.DistanceTo(Owner.Position) <= CastRange;
        }

        private Vector? getTargetLocation(object target)
        {
            if (!IsActive)
                return null;

            var isUnit = target is Unit;
            var isPoint = target is Vector;

            if (isUnit && CanTargetUnits())
                return ((Unit)target).Position;

            if (isPoint && CanTargetGround())
                return (Vector)target;

            //return null;
            throw new Exception("Expected a {0} target but received a {1}".Format(TargetType, target?.GetType()));
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
