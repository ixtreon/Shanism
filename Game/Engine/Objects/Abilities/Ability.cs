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
using Shanism.Common.StubObjects;
using ProtoBuf;
using Shanism.Common.Util;
using Shanism.Common.Interfaces.Objects;
using System.Threading;

namespace Shanism.Engine.Objects.Abilities
{
    /// <summary>
    /// Represents a passive or active ability that belongs to a single <see cref="Entity"/>. 
    /// </summary>
    /// <seealso cref="GameObject" />
    /// <seealso cref="IAbility" />
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

        internal double CastRangeSquared => CastRange * CastRange;

        /// <summary>
        /// Gets or sets whether this ability can be cast while moving.
        /// </summary>
        public bool CanCastWalk { get; set; }

        /// <summary>
        /// Gets whether this ability is active (i.e. castable, non-passive). 
        /// </summary>
        public bool IsActive => TargetType != AbilityTargetType.Passive;

        /// <summary>
        /// Gets or sets the target types of this ability, if it is targeted. 
        /// </summary>
        public AbilityTargetType TargetType { get; set; } = AbilityTargetType.NoTarget;

        /// <summary>
        /// Gets or sets the AoE of this ability, if it is targeted. 
        /// </summary>
        public int TargetAoE { get; set; }

        /// <summary>
        /// Gets or sets the name of the animation that will be played whenever the ability is cast. 
        /// </summary>
        public string Animation { get; set; } = Shanism.Common.Constants.Animations.Cast;


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


        internal bool Invoke(CastingData args)
        {
            //check prerequisites (mana, cd, others?)
            if (!CanCast(args))
                return false;

            var e = new AbilityCastArgs(Owner, args);

            OnCast(e);

            if (!e.Success)
                return false;

            triggerCooldowns();

            var tLoc = args.TargetLocation;
            Owner.Facing = Owner.Position.AngleTo(tLoc);
            Owner.PlayAnimation(Shanism.Common.Constants.Animations.Cast, false);
            return true;
        }

        /// <summary>
        /// Permanently sets the owner of this ability. 
        /// </summary>
        internal void SetOwner(Unit newOwner)
        {
            if (newOwner != null)
            {
                Owner = newOwner;
                OnLearned();
            }
            else
            {
                OnUnlearned();
                Owner = newOwner;
            }
        }


        /// <summary>
        /// Gets whether this ability can target another unit. 
        /// </summary>
        public bool CanTargetUnits => TargetType.HasFlag(AbilityTargetType.UnitTarget);


        /// <summary>
        /// Gets whether this ability can target a location the ground. 
        /// </summary>
        public bool CanTargetGround => TargetType.HasFlag(AbilityTargetType.PointTarget);

        /// <summary>
        /// Called whenever this ability is cast by its owner. 
        /// </summary>
        protected virtual void OnCast(AbilityCastArgs e) { }

        /// <summary>
        /// Called when this ability is initially learned by an unit. 
        /// </summary>
        protected virtual void OnLearned()
        {
        }

        /// <summary>
        /// Called when this ability is unlearned by an unit. 
        /// Executed right before unlearning happens, 
        /// so <see cref="Owner"/> still points to the unit who owned the ability. 
        /// </summary>
        protected virtual void OnUnlearned()
        {
        }

        /// <summary>
        /// Called once every frame once this ability is learned. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last frame, in milliseconds.</param>
        protected virtual void OnUpdate(int msElapsed) { }


        //Can we cast the spell. Does not check target/s. 
        bool canCast() => IsActive 
            && (CurrentCooldown <= 0) 
            && (Owner.Mana >= ManaCost);

        /// <summary>
        /// Determines whether this ability can be cast 
        /// using the specified in-game location as a target.
        /// </summary>
        public bool CanCast(Vector v)
        {
            if (!canCast()) return false;

            return TargetType == AbilityTargetType.NoTarget
                || (CanTargetGround && v.DistanceTo(Owner.Position) <= CastRange);
        }

        /// <summary>
        /// Determines whether this ability can be cast 
        /// using the specified entity as a target.
        /// </summary>
        public bool CanCast(Entity e)
        {
            if (!canCast()) return false;

            return TargetType == AbilityTargetType.NoTarget
                || e.Position.DistanceTo(Owner.Position) <= CastRange;
        }


        bool checkDistance(Vector tar)
            => Owner.Position.DistanceToSquared(tar) <= CastRangeSquared;

        /// <summary>
        /// Determines whether this ability can be cast 
        /// using the specified entity as a target.
        /// </summary>
        internal bool CanCast(CastingData cd)
        {
            if (cd.Ability != this)
                return false;

            switch (TargetType)
            {
                case AbilityTargetType.Passive:
                    return false;

                case AbilityTargetType.NoTarget:
                    return true;

                case AbilityTargetType.PointTarget:
                    return cd.IsGroundTarget 
                        && checkDistance((Vector)cd.Target);

                case AbilityTargetType.UnitTarget:
                    return cd.IsEntityTarget 
                        && checkDistance(((Entity)cd.Target).Position);

                case AbilityTargetType.PointOrUnitTarget:
                    if(cd.IsEntityTarget)
                        return checkDistance(((Entity)cd.Target).Position);

                    if(cd.IsGroundTarget)
                        return checkDistance((Vector)cd.Target);

                    return false;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal override void Update(int msElapsed)
        {
            if (CurrentCooldown > 0)
                CurrentCooldown = Math.Max(0, CurrentCooldown - msElapsed);

            OnUpdate(msElapsed);
        }

        /// <summary>
        /// Triggers the ability cooldown and mana cost. 
        /// </summary>
        internal void triggerCooldowns()
        {
            CurrentCooldown = Cooldown;
            Owner.Mana -= ManaCost;
        }
        


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Name;

        public override bool Equals(object obj) => (obj is Ability)
            && Id.Equals(((Ability)obj).Id);

        public override int GetHashCode() => (int)Id;
    }
}
