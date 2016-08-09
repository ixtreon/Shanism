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
    public abstract class Ability : GameObject, IAbility, IEquatable<Ability>
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
        /// Gets the current cooldown of this ability in milliseconds. 
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
        /// Gets or sets whether this ability can be cast while moving.
        /// </summary>
        public bool CanCastWalk { get; set; }

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

        internal override void Update(int msElapsed)
        {
            if (CurrentCooldown > 0)
                CurrentCooldown = Math.Max(0, CurrentCooldown - msElapsed);

            Scripts.Enqueue(() => OnUpdate(msElapsed));
        }


        #region Property Shortcuts


        internal double CastRangeSquared => CastRange * CastRange;

        /// <summary>
        /// Gets whether this ability is active (i.e. castable, non-passive). 
        /// </summary>
        public bool IsActive => TargetType != AbilityTargetType.Passive;


        /// <summary>
        /// Gets whether this ability requires a target. 
        /// </summary>
        public bool RequiresTarget => IsActive && (TargetType != AbilityTargetType.NoTarget);


        /// <summary>
        /// Gets whether this ability can target another unit. 
        /// </summary>
        public bool CanTargetUnits => (TargetType & AbilityTargetType.UnitTarget) != 0;


        /// <summary>
        /// Gets whether this ability can target a location the ground. 
        /// </summary>
        public bool CanTargetGround => (TargetType & AbilityTargetType.PointTarget) != 0;

        #endregion


        #region Virtual Methods

        /// <summary>
        /// Called whenever this ability is cast by its owner. 
        /// </summary>
        protected virtual void OnCast(AbilityCastArgs e) { }

        /// <summary>
        /// Called when this ability is initially learned by an unit. 
        /// </summary>
        protected virtual void OnLearned() { }

        /// <summary>
        /// Called when this ability is unlearned by an unit. 
        /// Executed right before unlearning happens, 
        /// so <see cref="Owner"/> still points to the unit who owned the ability. 
        /// </summary>
        protected virtual void OnUnlearned() { }

        /// <summary>
        /// Called once every frame once this ability is learned. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last frame, in milliseconds.</param>
        protected virtual void OnUpdate(int msElapsed) { }

        #endregion

        /// <summary>
        /// Determines whether this ability can be currently 
        /// cast without any target. 
        /// </summary>
        public bool CanCast()
        {
            return canCast() && !RequiresTarget;
        }

        /// <summary>
        /// Determines whether this ability can be currently
        /// cast using the specified in-game location as a target.
        /// </summary>
        public bool CanCast(Vector v)
        {
            return canCast() && CanTargetGround && checkDistance(v);
        }

        /// <summary>
        /// Determines whether this ability can be currently
        /// cast using the specified entity as a target.
        /// </summary>
        public bool CanCast(Entity e)
        {
            return canCast() && RequiresTarget && checkDistance(e.Position);
        }

        /// <summary>
        /// Determines whether an ability can be cast with the provided args.
        /// </summary>
        internal static bool CanCast(CastingData cd)
        {
            var ab = cd.Ability;

            switch (ab.TargetType)
            {
                case AbilityTargetType.Passive:
                    return false;

                case AbilityTargetType.NoTarget:
                    return true;

                case AbilityTargetType.PointTarget:
                    return cd.TargetType == AbilityTargetType.PointTarget
                        && ab.CanCast(cd.TargetLocation);

                case AbilityTargetType.UnitTarget:
                    return cd.TargetType == AbilityTargetType.UnitTarget
                        && ab.CanCast(cd.TargetEntity);

                case AbilityTargetType.PointOrUnitTarget:
                    if (cd.TargetType == AbilityTargetType.UnitTarget)
                        return ab.CanCast(cd.TargetEntity);

                    if (cd.TargetType == AbilityTargetType.PointTarget)
                        return ab.CanCast(cd.TargetLocation);

                    return false;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Tries to cast the given spell
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        internal bool Invoke(CastingData args)
        {
            Debug.Assert(args.Ability == this);

            //check prerequisites (mana, cd, others?)
            if (!Ability.CanCast(args))
                return false;

            //run custom ability code
            var e = new AbilityCastArgs(Owner, args);
            OnCast(e);

            if (!e.Success)
                return false;

            //activate cd, mana, rotate, animate
            CurrentCooldown = Cooldown;
            Owner.Mana -= ManaCost;

            var tLoc = args.TargetLocation;
            Owner.Facing = Owner.Position.AngleTo(tLoc);
            Owner.PlayAnimation(Shanism.Common.Constants.Animations.Cast, false);

            return true;
        }

        /// <summary>
        /// Sets the owner of this ability. 
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


        bool canCast()
            => IsActive && CurrentCooldown <= 0 && Owner.Mana >= ManaCost;

        bool checkDistance(Vector tar)
            => Owner.Position.DistanceToSquared(tar) <= CastRangeSquared;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Name;

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
            => Equals(obj as Ability);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => (int)Id;

        public bool Equals(Ability other)
            => other?.Id == Id;
    }
}
