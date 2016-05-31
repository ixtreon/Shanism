using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Maps;
using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Engine.Events;
using Shanism.Common.Objects;
using ProtoBuf;
using Shanism.Common.Util;

namespace Shanism.Engine.Systems.Abilities
{
    /// <summary>
    /// Represents a passive or active ability that belongs to a single <see cref="Entity"/>. 
    /// </summary>
    /// <seealso cref="Shanism.Engine.GameObject" />
    /// <seealso cref="IO.Objects.IAbility" />
    public abstract class Ability : GameObject, IAbility
    {


        /// <summary>
        /// Gets the object type of this ability. 
        /// Always has a value of <see cref="ObjectType.Ability"/>. 
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Ability;

        /// <summary>
        /// Gets the hero who owns and casts this ability. 
        /// </summary>
        public Unit Owner { get; private set; }

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
        public string Icon { get; set; } = Shanism.Common.Constants.Content.DefaultValues.Icon;

        /// <summary>
        /// Gets the currently remaining cooldown of this ability. 
        /// </summary>
        public int CurrentCooldown { get; internal set; }

        /// <summary>
        /// Gets or sets the cooldown of this ability in milliseconds. 
        /// </summary>
        public int Cooldown { get; set; } = 1500;

        /// <summary>
        /// Gets or sets the mana cost of this ability. 
        /// </summary>
        public int ManaCost { get; set; } = 1;

        /// <summary>
        /// Gets or sets the casting time of the ability, in milliseconds. 
        /// </summary>
        public int CastTime { get; set; }

        /// <summary>
        /// Gets or sets the casting range of the ability in units. 
        /// </summary>
        public double CastRange { get; set; } = 15;

        /// <summary>
        /// Gets whether this ability is active (i.e. can be cast instantly or on a target). 
        /// </summary>
        public bool IsActive { get { return TargetType != AbilityTargetType.Passive; } }

        /// <summary>
        /// Gets or sets the target types of this ability, if it is targeted. 
        /// </summary>
        public AbilityTargetType TargetType { get; set; } = AbilityTargetType.NoTarget;

        /// <summary>
        /// Gets or sets the AoE of this ability, if it is targeted. 
        /// </summary>
        public int TargetAoE { get; set; }


        /// <summary>
        /// Creates a new ability with default parameters. 
        /// </summary>
        protected Ability()
        {
        }

        /// <summary>
        /// Creates a new ability of the specified type. 
        /// </summary>
        /// <param name="targetType"></param>
        protected Ability(AbilityTargetType targetType)
        {
            TargetType = targetType;
        }


        internal AbilityCastArgs Cast(object target)
        {
            var e = new AbilityCastArgs(this, Owner, target);
            OnCast(e);

            return e;
        }

        /// <summary>
        /// Permanently sets the owner of this ability. 
        /// </summary>
        internal void SetOwner(Unit owner)
        {
            if (Owner != null) throw new InvalidOperationException("This spell already has an owner!");
            Owner = owner;
            OnLearned();
        }


        /// <summary>
        /// Gets whether this ability can target another unit. 
        /// </summary>
        public bool CanTargetUnits() => TargetType.HasFlag(AbilityTargetType.UnitTarget);


        /// <summary>
        /// Gets whether this ability can target a location the ground. 
        /// </summary>
        public bool CanTargetGround() => TargetType.HasFlag(AbilityTargetType.PointTarget);

        /// <summary>
        /// Called whenever this ability is cast by its owner. 
        /// </summary>
        protected virtual void OnCast(AbilityCastArgs e) { }

        /// <summary>
        /// Called when this ability is initially learned by an unit. 
        /// </summary>
        protected virtual void OnLearned() { }

        /// <summary>
        /// Called once every frame once this ability is learned. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last frame, in milliseconds.</param>
        protected virtual void OnUpdate(int msElapsed) { }

        /// <summary>
        /// Checks if range, coooldown, mana cost and range to target are all OK. 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CanCast(object target)
        {
            if (target is Unit && ((Unit)target).IsDead)
                return false;

            return IsActive 
                && (CurrentCooldown <= 0) 
                && (Owner.Mana >= ManaCost) 
                && checkRange(target);
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

        bool checkRange(object target)
        {
            var tLoc = getTargetLocation(target);
            if (!tLoc.HasValue)
                return true;

            return tLoc.Value.DistanceTo(Owner.Position) <= CastRange;
        }

        Vector? getTargetLocation(object target)
        {
            if (!IsActive)
                return null;

            var targetUnit = target as Unit;
            var targetLoc = (target as Vector?) ?? (targetUnit?.Position);

            switch(TargetType)
            {
                case AbilityTargetType.NoTarget:
                case AbilityTargetType.Passive:
                    return null;

                case AbilityTargetType.PointTarget:
                    if (targetLoc == null)
                        throw new Exception("Expected a point but received something else!");
                    return targetLoc;

                case AbilityTargetType.UnitTarget:
                    if (targetUnit == null)
                        throw new Exception("Expected a unit but received something else!");
                    return targetUnit.Position;

                case AbilityTargetType.PointOrUnitTarget:
                    if (targetLoc == null)
                        throw new Exception("Expected a point or unit but received something else!");
                    return targetLoc;

                default:
                    throw new Exception("Unexpected ability type!");
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
