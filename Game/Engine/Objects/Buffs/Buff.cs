using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Systems;
using IO.Objects;
using IO.Common;
using IO;
using IO.Util;

namespace Engine.Objects.Buffs
{

    /// <summary>
    /// Represents any in-game effect which has a temporary or permanent effect on a unit's statistics. 
    /// </summary>
    public class Buff : GameObject, IBuff
    {

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public override ObjectType ObjectType {  get { return ObjectType.BuffInstance; } }


        int _moveSpeed;
        string _rawDescription;


        /// <summary>
        /// Gets or sets icon of the buff. 
        /// </summary>
        public string Icon { get; set; } = IO.Constants.Content.DefaultValues.Icon;

        /// <summary>
        /// Gets or sets the name of the buff. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the raw description of the buff. 
        /// <para/>
        /// Uses <see cref="StringsExt.FormatWith{T}(string, T)"/> to parse "{property_name}" into its value. 
        /// </summary>
        public string RawDescription
        {
            get { return _rawDescription; }
            set
            {
                _rawDescription = value;
                Description = _rawDescription.FormatWith(this);
            }
        }

        /// <summary>
        /// Gets the formatted description of this buff. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the life modifier of this buff. 
        /// </summary>
        public double Life { get; set; }

        /// <summary>
        /// Gets or sets the mana modifier of this buff. 
        /// </summary>
        public double Mana { get; set; }

        /// <summary>
        /// Gets or sets the life regen modifier of this buff. 
        /// </summary>
        public double LifeRegen { get; set; }

        /// <summary>
        /// Gets or sets the mana regen modifier of this buff. 
        /// </summary>
        public double ManaRegen { get; set; }

        /// <summary>
        /// Gets or sets the defense provided by this buff. 
        /// </summary>
        public double Defense { get; set; }

        /// <summary>
        /// Gets or sets the dodge (evasion) modifier provided by this buff. 
        /// </summary>
        public double Dodge { get; set; }

        /// <summary>
        /// Gets or sets the critical strike chance modifier provided by this buff. 
        /// </summary>
        public double Crit { get; set; }

        /// <summary>
        /// Gets or sets the movement speed modifier of this buff. 
        /// </summary>
        public double MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the movement speed percentage modifier of this buff. 
        /// </summary>
        public int MoveSpeedPercentage
        {
            get { return _moveSpeed; }
            set { _moveSpeed = Math.Max(-100, Math.Min(100, value)); }
        }

        /// <summary>
        /// Gets or sets the attack speed percentage modifier of this buff. 
        /// </summary>
        public int AttackSpeedPercentage { get; set; }

        /// <summary>
        /// Gets or sets the mnimum damage modifier of this buff. 
        /// </summary>
        public double MinDamage { get; set; }

        /// <summary>
        /// Gets or sets the maximum damage modifier of this buff. 
        /// </summary>
        public double MaxDamage { get; set; }

        /// <summary>
        /// Gets or sets the strength modifier of this buff. 
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        /// Gets or sets the vitality modifier of this buff. 
        /// </summary>
        public double Vitality { get; set; }

        /// <summary>
        /// Gets or sets the agility modifier of this buff. 
        /// </summary>
        public double Agility { get; set; }

        /// <summary>
        /// Gets or sets the intellect modifier of this buff. 
        /// </summary>
        public double Intellect { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the buff, in miliseconds. 
        /// If this value is nonpositive the buff lasts indefinitely. 
        /// </summary>
        public int FullDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of stacks of this buff 
        /// that can applied to a single target by a given unit. 
        /// If set to 0 the buff can be stacked infinitely. 
        /// </summary>
        public int MaxStacks { get; set; } = 0;


        /// <summary>
        /// Gets or sets the type of this buff. 
        /// </summary>
        public BuffType Type { get; set; } = BuffType.NonStacking;

        /// <summary>
        /// Gets or sets whether this buff has an icon and shows in the default buff bar. 
        /// </summary>
        public bool HasIcon { get; set; } = true;

        /// <summary>
        /// Gets or sets the unit states that are applied to units affected by this buff. 
        /// </summary>
        public UnitFlags UnitStates { get; set; }


        /// <summary>
        /// Gets whether this buff is timed or indefinite. 
        /// </summary>
        public bool IsTimed => FullDuration > 0;


        /// <summary>
        /// Creates a new buff with default values. 
        /// </summary>
        public Buff()
        {
        }


        public Buff(Buff @base)
        {
            CloningPlant.ShallowCopy(@base, this);
        }


        /// <summary>
        /// The method executed whenever a buff expires from a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnExpired(BuffInstance buff) { }

        /// <summary>
        /// The method executed on every update frame for which a buff is on a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnUpdate(BuffInstance buff) { }

        /// <summary>
        /// The method executed whenever a buff is refreshed on a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnRefresh(BuffInstance buff) { }

        /// <summary>
        /// The method executed whenever a buff is initially applied to a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnApplied(BuffInstance buff) { }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => (int)Id;

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is Buff) && ((Buff)obj).Id == Id;
        }
    }
}
