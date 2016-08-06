using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Engine.Systems;
using Shanism.Common.StubObjects;
using Shanism.Common.Game;
using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Common.Interfaces.Objects;
using Shanism.Engine.Entities;

namespace Shanism.Engine.Objects.Buffs
{

    /// <summary>
    /// Represents any in-game effect which has a temporary or permanent effect on a unit's statistics. 
    /// </summary>
    public class Buff : GameObject, IBuff
    {

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Buff;


        int _moveSpeedPerc;
        string _rawDescription;

        internal readonly UnitStats unitStats = new UnitStats();
        internal readonly HeroAttributes heroStats = new HeroAttributes();

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
        public int MaxStacks { get; set; } = 0;


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
        public float MaxLife
        {
            get { return unitStats[UnitStat.MaxLife]; }
            set { unitStats[UnitStat.MaxLife] = value; }
        }
        /// <summary>
        /// Gets or sets the mana modifier of this buff. 
        /// </summary>
        public float MaxMana
        {
            get { return unitStats[UnitStat.MaxMana]; }
            set { unitStats[UnitStat.MaxMana] = value; }
        }
        /// <summary>
        /// Gets or sets the life regen modifier of this buff. 
        /// </summary>
        public float LifeRegen
        {
            get { return unitStats[UnitStat.LifeRegen]; }
            set { unitStats[UnitStat.LifeRegen] = value; }
        }
        /// <summary>
        /// Gets or sets the mana regen modifier of this buff. 
        /// </summary>
        public float ManaRegen
        {
            get { return unitStats[UnitStat.ManaRegen]; }
            set { unitStats[UnitStat.ManaRegen] = value; }
        }
        /// <summary>
        /// Gets or sets the mnimum damage modifier of this buff. 
        /// </summary>
        public float MinDamage
        {
            get { return unitStats[UnitStat.MinDamage]; }
            set { unitStats[UnitStat.MinDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the maximum damage modifier of this buff. 
        /// </summary>
        public float MaxDamage
        {
            get { return unitStats[UnitStat.MaxDamage]; }
            set { unitStats[UnitStat.MaxDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the defense provided by this buff. 
        /// </summary>
        public float Defense
        {
            get { return unitStats[UnitStat.Defense]; }
            set { unitStats[UnitStat.Defense] = value; }
        }

        /// <summary>
        /// Gets or sets the movement speed modifier of this buff. 
        /// </summary>
        public float MoveSpeed
        {
            get { return unitStats[UnitStat.MoveSpeed]; }
            set { unitStats[UnitStat.MoveSpeed] = value; }
        }
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
        public float Strength
        {
            get { return heroStats[HeroAttribute.Strength]; }
            set { heroStats[HeroAttribute.Strength] = value; }
        }

        /// <summary>
        /// Gets or sets the vitality modifier of this buff.
        /// </summary>
        public float Vitality
        {
            get { return heroStats[HeroAttribute.Vitality]; }
            set { heroStats[HeroAttribute.Vitality] = value; }
        }

        /// <summary>
        /// Gets or sets the agility modifier of this buff.
        /// </summary>
        public float Agility
        {
            get { return heroStats[HeroAttribute.Agility]; }
            set { heroStats[HeroAttribute.Agility] = value; }
        }

        /// <summary>
        /// Gets or sets the intellect modifier of this buff.
        /// </summary>
        public float Intellect
        {
            get { return heroStats[HeroAttribute.Intellect]; }
            set { heroStats[HeroAttribute.Intellect] = value; }
        }

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
        public override int GetHashCode() => (int)Id;

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => (obj is Buff)
            && ((Buff)obj).Id == Id;

    }
}
