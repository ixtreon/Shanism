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
        /// Gets the buff prototype of this buff instance. 
        /// </summary>
        public Buff Prototype { get; }

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
        /// Gets the duration left, in milliseconds, before this buff instance expires. 
        /// Only applicable to timed buffs (see <see cref="Buff.IsTimed"/>). 
        /// </summary>
        public int DurationLeft { get; private set; }

        /// <summary>
        /// Gets whether this buff should be removed from its target unit. 
        /// </summary>
        public bool HasExpired => Prototype.IsTimed && DurationLeft <= 0;


        #region IBuff Implementation

        /// <summary>
        /// Gets the icon of the buff.
        /// </summary>
        public string Icon => ((IBuff)Prototype).Icon;

        /// <summary>
        /// Gets whether this buff has an icon
        /// and is displayed in the default buff bar.
        /// </summary>
        public bool HasIcon => ((IBuff)Prototype).HasIcon;

        /// <summary>
        /// Gets the name of the buff.
        /// </summary>
        public string Name => ((IBuff)Prototype).Name;

        /// <summary>
        /// Gets the formatted description of this buff.
        /// </summary>
        public string Description => ((IBuff)Prototype).Description;

        /// <summary>
        /// Gets the type of this buff.
        /// </summary>
        public BuffStackType StackType => ((IBuff)Prototype).StackType;

        /// <summary>
        /// Gets the life modifier of this buff.
        /// </summary>
        public double MaxLife => ((IBuff)Prototype).MaxLife;

        /// <summary>
        /// Gets the mana modifier of this buff.
        /// </summary>
        public double MaxMana => ((IBuff)Prototype).MaxMana;

        /// <summary>
        /// Gets the defense provided by this buff.
        /// </summary>
        public double Defense => ((IBuff)Prototype).Defense;

        /// <summary>
        /// Gets the dodge (evasion) modifier provided by this buff.
        /// </summary>
        public double Dodge => ((IBuff)Prototype).Dodge;

        /// <summary>
        /// Gets the movement speed modifier of this buff.
        /// </summary>
        public double MoveSpeed => ((IBuff)Prototype).MoveSpeed;

        /// <summary>
        /// Gets the movement speed percentage modifier of this buff.
        /// </summary>
        public int MoveSpeedPercentage => ((IBuff)Prototype).MoveSpeedPercentage;

        /// <summary>
        /// Gets the attack speed percentage modifier of this buff.
        /// </summary>
        public int AttackSpeedPercentage => ((IBuff)Prototype).AttackSpeedPercentage;

        /// <summary>
        /// Gets the mnimum damage modifier of this buff.
        /// </summary>
        public double MinDamage => ((IBuff)Prototype).MinDamage;

        /// <summary>
        /// Gets the maximum damage modifier of this buff.
        /// </summary>
        public double MaxDamage => ((IBuff)Prototype).MaxDamage;

        /// <summary>
        /// Gets the strength modifier of this buff.
        /// </summary>
        public double Strength => ((IBuff)Prototype).Strength;

        /// <summary>
        /// Gets the vitality modifier of this buff.
        /// </summary>
        public double Vitality => ((IBuff)Prototype).Vitality;

        /// <summary>
        /// Gets the agility modifier of this buff.
        /// </summary>
        public double Agility => ((IBuff)Prototype).Agility;

        /// <summary>
        /// Gets the intellect modifier of this buff.
        /// </summary>
        public double Intellect => ((IBuff)Prototype).Intellect;

        /// <summary>
        /// Gets the total duration of the buff, in miliseconds.
        /// If this value is nonpositive the buff lasts indefinitely.
        /// </summary>
        public int FullDuration => ((IBuff)Prototype).FullDuration;

        /// <summary>
        /// Gets the unit states that are applied to units affected by this buff.
        /// </summary>
        public StateFlags UnitStates => ((IBuff)Prototype).UnitStates;

        /// <summary>
        /// Gets the life regen modifier of this buff.
        /// </summary>
        public double LifeRegen => ((IBuff)Prototype).LifeRegen;

        /// <summary>
        /// Gets the mana regen modifier of this buff.
        /// </summary>
        public double ManaRegen => ((IBuff)Prototype).ManaRegen;

        #endregion



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

            Prototype.OnUpdate(this);
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
