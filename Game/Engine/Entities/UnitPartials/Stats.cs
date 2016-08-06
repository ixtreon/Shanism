using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {
        internal readonly UnitStats baseUnitStats = new UnitStats();
        internal readonly UnitStats unitStats = new UnitStats();
        int _baseDodge, _baseCrit;


        IUnitStats IUnit.BaseStats => baseUnitStats;
        IUnitStats IUnit.Stats => unitStats;


        #region Base Stats
        /// <summary>
        /// Gets or sets the base dodge chance of the unit. 
        /// </summary>
        public int BaseDodgeChance
        {
            get { return _baseDodge; }
            set { _baseDodge = value.Clamp(0, 100); }
        }
        /// <summary>
        /// Gets or sets the base chance of dealing a critical strike for this unit. 
        /// </summary>
        public int BaseCritChance
        {
            get { return _baseCrit; }
            set { _baseCrit = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public float BaseMaxLife
        {
            get { return baseUnitStats[UnitStat.MaxLife]; }
            set { baseUnitStats[UnitStat.MaxLife] = value; }
        }
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public float BaseLifeRegen
        {
            get { return baseUnitStats[UnitStat.LifeRegen]; }
            set { baseUnitStats[UnitStat.LifeRegen] = value; }
        }
        /// <summary>
        /// Gets or sets the base mana of the unit. 
        /// </summary>
        public float BaseMaxMana
        {
            get { return baseUnitStats[UnitStat.MaxMana]; }
            set { baseUnitStats[UnitStat.MaxMana] = value; }
        }
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public float BaseManaRegen
        {
            get { return baseUnitStats[UnitStat.ManaRegen]; }
            set { baseUnitStats[UnitStat.ManaRegen] = value; }
        }

        /// <summary>
        /// Gets or sets the base minimum damage inflicted by the unit. 
        /// </summary>
        public float BaseMinDamage
        {
            get { return baseUnitStats[UnitStat.MinDamage]; }
            set { baseUnitStats[UnitStat.MinDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the base maximum damage inflicted by the unit. 
        /// </summary>
        public float BaseMaxDamage
        {
            get { return baseUnitStats[UnitStat.MaxDamage]; }
            set { baseUnitStats[UnitStat.MaxDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public float BaseMagicDamage
        {
            get { return baseUnitStats[UnitStat.MagicDamage]; }
            set { baseUnitStats[UnitStat.MagicDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public float BaseDefense
        {
            get { return baseUnitStats[UnitStat.Defense]; }
            set { baseUnitStats[UnitStat.Defense] = value; }
        }

        /// <summary>
        /// Gets or sets the base movement speed of the unit in squares per second. 
        /// </summary>
        public float BaseMoveSpeed
        {
            get { return baseUnitStats[UnitStat.MoveSpeed]; }
            set { baseUnitStats[UnitStat.MoveSpeed] = value; }
        }
        /// <summary>
        /// Gets or sets the base rate of attack of the unit measured in attacks per second. 
        /// </summary>
        public float BaseAttacksPerSecond
        {
            get { return baseUnitStats[UnitStat.AttacksPerSecond]; }
            set { baseUnitStats[UnitStat.AttacksPerSecond] = value; }
        }
        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public float BaseAttackRange
        {
            get { return baseUnitStats[UnitStat.AttackRange]; }
            set { baseUnitStats[UnitStat.AttackRange] = value; }
        }
        #endregion


        #region Current Stats
        /// <summary>
        /// Gets the unit's chance to dodge an attack, in the range 0 to 100. 
        /// </summary>
        public int DodgeChance { get; internal set; }

        /// <summary>
        /// Gets the unit's chance to dodge an attack, in the range 0 to 100. 
        /// NYI
        /// </summary>
        public int CritChance { get; internal set; }

        /// <summary>
        /// Gets the maximum life of the unit. 
        /// </summary>
        public float MaxLife => unitStats[UnitStat.MaxLife];
        /// <summary>
        /// Gets the current life regeneration rate of the unit, in life points per second.  
        /// </summary>
        public float LifeRegen => unitStats[UnitStat.LifeRegen];
        /// <summary>
        /// Gets the maximum mana of the unit. 
        /// </summary>
        public float MaxMana => unitStats[UnitStat.MaxMana];
        /// <summary>
        /// Gets the current mana regeneration rate of the unit, in mana points per second.  
        /// </summary>
        public float ManaRegen => unitStats[UnitStat.ManaRegen];

        /// <summary>
        /// Gets the minimum damage of the unit's attack. 
        /// </summary>
        public float MinDamage => unitStats[UnitStat.MinDamage];
        /// <summary>
        /// Gets the maximum damage of the unit's attack. 
        /// </summary>
        public float MaxDamage => unitStats[UnitStat.MaxDamage];
        /// <summary>
        /// Gets the base bonus magic damage of the unit. 
        /// </summary>
        public float MagicDamage => unitStats[UnitStat.MagicDamage];
        /// <summary>
        /// Gets the current defense of the unit which provides reduction
        /// against physical damage. 
        /// </summary>
        public float Defense => unitStats[UnitStat.Defense];

        /// <summary>
        /// Gets the current movement speed of the unit. 
        /// </summary>
        public float MoveSpeed => unitStats[UnitStat.MoveSpeed];
        /// <summary>
        /// Gets the time this unit takes between successive attacks. 
        /// </summary>
        public float AttacksPerSecond => unitStats[UnitStat.AttacksPerSecond];
        /// <summary>
        /// Gets the current attack range of the unit. 
        /// </summary>
        public float AttackRange => unitStats[UnitStat.AttackRange];

        #endregion


        internal void refreshStats()
        {
            unitStats.Set(baseUnitStats);
            foreach (var b in Buffs)
                unitStats.Add(b.Prototype.unitStats);
        }
    }
}

