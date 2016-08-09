using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Util.Hash;
using Shanism.Engine.Entities;
using System;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Buffs
{
    /// <summary>
    /// Represents one or more instances of a given <see cref="Buff"/> applied by a given <see cref="Unit"/> to a single target unit. 
    /// Implements the <see cref="IBuff"/> interface via the <see cref="Prototype"/> member. 
    /// </summary>
    public class BuffInstance : GameObject, IBuffInstance
    {

        /// <summary>
        /// Gets the object type of this buff instance.
        /// Always returns <see cref="ObjectType.BuffInstance"/>. 
        /// </summary>
        public override ObjectType ObjectType { get; } = ObjectType.BuffInstance;


        /// <summary>
        /// Gets the target unit this buff instance is applied to. 
        /// </summary>
        public Unit Target { get; }

        /// <summary>
        /// Gets the caster of this buff instance, 
        /// or null if it is a global buff. 
        /// </summary>
        public Unit Caster { get; }

        /// <summary>
        /// Gets the buff prototype of this buff instance. 
        /// </summary>
        public Buff Prototype { get; }

        /// <summary>
        /// Gets the duration left, in milliseconds, before this buff instance expires. 
        /// Only applicable to timed buffs (see <see cref="Buff.IsTimed"/>). 
        /// </summary>
        public int DurationLeft { get; private set; }

        /// <summary>
        /// Gets whether this buff should be removed from its target unit. 
        /// </summary>
        public bool HasExpired => Prototype.IsTimed && DurationLeft <= 0;


        IBuff IBuffInstance.Prototype => Prototype;


        /// <summary>
        /// Initializes a new instance of the <see cref="BuffInstance"/> class.
        /// </summary>
        /// <param name="buff">The buff prototype. Must be non-null. </param>
        /// <param name="caster">The caster unit.</param>
        /// <param name="target">The target unit. Must be non-null. </param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public BuffInstance(Buff buff, Unit caster, Unit target)
        {
            if (buff == null) throw new ArgumentNullException(nameof(buff));
            if (target == null) throw new ArgumentNullException(nameof(target));

            Prototype = buff;
            Caster = caster;
            Target = target;

            DurationLeft = buff.FullDuration;

        }

        internal override void Update(int msElapsed)
        {
            if (Prototype.IsTimed)
            {
                DurationLeft -= msElapsed;
                if (HasExpired)
                {
                    Prototype.OnExpired(this);
                    return;
                }
            }

            Scripts.Enqueue(() => Prototype.OnUpdate(this));
        }

        /// <summary>
        /// Refreshes the duration of this buff instance. 
        /// </summary>
        public void RefreshDuration()
        {
            DurationLeft = Prototype.FullDuration;

            Prototype.OnRefresh(this);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is BuffInstance))
                return false;
            var b = (BuffInstance)obj;

            return b.Prototype.Equals(Prototype)
                && (b.Caster == Caster)
                && (b.Target == Target);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Hash.Get(
                Prototype.GetHashCode(),
                Caster?.GetHashCode() ?? 0,
                Target.GetHashCode());
        }
    }
}
