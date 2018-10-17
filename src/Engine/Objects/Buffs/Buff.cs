using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Shanism.Common.Objects;

namespace Shanism.Engine.Objects.Buffs
{

    /// <summary>
    /// Represents any in-game effect which has a temporary or permanent effect on a unit's statistics. 
    /// </summary>
    public class Buff : GameObject, IBuff, IEquatable<Buff>
    {

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Buff;


        int _moveSpeedPerc;
        string _rawDescription;

        internal readonly UnitStats unitStats = new UnitStats(0);
        internal readonly HeroAttributes heroStats = new HeroAttributes(0);

        IUnitStats IBuff.Stats => unitStats;
        IHeroAttributes IBuff.Attributes => heroStats;

        /// <summary>
        /// Gets or sets whether this buff has an icon and shows in the default buff bar. 
        /// </summary>
        public bool HasIcon { get; set; } = true;

        /// <summary>
        /// Gets or sets icon of the buff. 
        /// </summary>
        public string Icon { get; set; } = Shanism.Common.Constants.Content.DefaultValues.Icon;

        public Color IconTint { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the name of the buff. 
        /// </summary>
        public string Name { get; set; } = string.Empty;

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
                Description = (_rawDescription = value).FormatWith(this);
            }
        }

        /// <summary>
        /// Gets the formatted description of this buff. 
        /// </summary>
        public string Description { get; private set; }

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
        public int MaxStacks { get; set; } = 1;


        /// <summary>
        /// Gets or sets the type of this buff. 
        /// </summary>
        public BuffStackType StackType { get; set; }

        /// <summary>
        /// Gets or sets the unit states that are applied to units affected by this buff. 
        /// </summary>
        public StateFlags StateFlags { get; set; }


        /// <summary>
        /// Gets whether this buff is timed or indefinite. 
        /// </summary>
        public bool IsTimed => FullDuration > 0;


        #region UnitStats
        /// <summary>
        /// Gets or sets the life modifier of this buff. 
        /// </summary>
        public ref float MaxLife => ref unitStats.Get(UnitField.MaxLife);
        /// <summary>
        /// Gets or sets the mana modifier of this buff. 
        /// </summary>
        public ref float MaxMana => ref unitStats.Get(UnitField.MaxMana);
        /// <summary>
        /// Gets or sets the life regen modifier of this buff. 
        /// </summary>
        public ref float LifeRegen => ref unitStats.Get(UnitField.LifeRegen);
        /// <summary>
        /// Gets or sets the mana regen modifier of this buff. 
        /// </summary>
        public ref float ManaRegen => ref unitStats.Get(UnitField.ManaRegen);
        /// <summary>
        /// Gets or sets the mnimum damage modifier of this buff. 
        /// </summary>
        public ref float MinDamage => ref unitStats.Get(UnitField.MinDamage);
        /// <summary>
        /// Gets or sets the maximum damage modifier of this buff. 
        /// </summary>
        public ref float MaxDamage => ref unitStats.Get(UnitField.MaxDamage);
        /// <summary>
        /// Gets or sets the defense provided by this buff. 
        /// </summary>
        public ref float Defense => ref unitStats.Get(UnitField.Defense);

        /// <summary>
        /// Gets or sets the movement speed modifier of this buff. 
        /// </summary>
        public ref float MoveSpeed => ref unitStats.Get(UnitField.MoveSpeed);
        /// <summary>
        /// Gets or sets the movement speed percentage modifier of this buff. 
        /// </summary>
        public int MoveSpeedPercentage
        {
            get { return _moveSpeedPerc; }
            set { _moveSpeedPerc = Math.Max(-100, Math.Min(100, value)); }
        }

        /// <summary>
        /// Gets or sets the attack speed percentage modifier of this buff. 
        /// </summary>
        public int AttackSpeedPercentage { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the dodge (evasion) modifier provided by this buff. 
        /// </summary>
        public int Dodge { get; set; }

        /// <summary>
        /// Gets or sets the critical strike chance modifier provided by this buff. 
        /// </summary>
        public int Crit { get; set; }


        #region HeroStats

        /// <summary>
        /// Gets or sets the strength modifier of this buff.
        /// </summary>
        public ref float Strength => ref heroStats.Get(HeroAttribute.Strength);

        /// <summary>
        /// Gets or sets the vitality modifier of this buff.
        /// </summary>
        public ref float Vitality => ref heroStats.Get(HeroAttribute.Vitality);

        /// <summary>
        /// Gets or sets the agility modifier of this buff.
        /// </summary>
        public ref float Agility => ref heroStats.Get(HeroAttribute.Agility);

        /// <summary>
        /// Gets or sets the intellect modifier of this buff.
        /// </summary>
        public ref float Intellect => ref heroStats.Get(HeroAttribute.Intellect);

        #endregion


        /// <summary>
        /// Creates a new buff with default values. 
        /// </summary>
        public Buff()
        {
        }



        #region Virtual Methods

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

        #endregion


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// Checks whether the buffs have the same id. 
        /// </summary>
        public override bool Equals(object obj) => (obj is Buff b) && Equals(b);

        /// <summary>
        /// Checks whether the buffs have the same id. 
        /// </summary>
        public bool Equals(Buff other) => Id == other.Id;
    }
}
