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
        internal readonly UnitStats baseStats = new UnitStats();
        internal readonly UnitStats stats = new UnitStats();
        int _baseDodge, _baseCrit;


        IUnitStats IUnit.BaseStats => baseStats;
        IUnitStats IUnit.Stats => stats;


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
            get { return baseStats[UnitStat.MaxLife]; }
            set { baseStats[UnitStat.MaxLife] = value; }
        }
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public float BaseLifeRegen
        {
            get { return baseStats[UnitStat.LifeRegen]; }
            set { baseStats[UnitStat.LifeRegen] = value; }
        }
        /// <summary>
        /// Gets or sets the base mana of the unit. 
        /// </summary>
        public float BaseMaxMana
        {
            get { return baseStats[UnitStat.MaxMana]; }
            set { baseStats[UnitStat.MaxMana] = value; }
        }
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public float BaseManaRegen
        {
            get { return baseStats[UnitStat.ManaRegen]; }
            set { baseStats[UnitStat.ManaRegen] = value; }
        }

        /// <summary>
        /// Gets or sets the base minimum damage inflicted by the unit. 
        /// </summary>
        public float BaseMinDamage
        {
            get { return baseStats[UnitStat.MinDamage]; }
            set { baseStats[UnitStat.MinDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the base maximum damage inflicted by the unit. 
        /// </summary>
        public float BaseMaxDamage
        {
            get { return baseStats[UnitStat.MaxDamage]; }
            set { baseStats[UnitStat.MaxDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public float BaseMagicDamage
        {
            get { return baseStats[UnitStat.MagicDamage]; }
            set { baseStats[UnitStat.MagicDamage] = value; }
        }
        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public float BaseDefense
        {
            get { return baseStats[UnitStat.Defense]; }
            set { baseStats[UnitStat.Defense] = value; }
        }

        /// <summary>
        /// Gets or sets the base movement speed of the unit in squares per second. 
        /// </summary>
        public float BaseMoveSpeed
        {
            get { return baseStats[UnitStat.MoveSpeed]; }
            set { baseStats[UnitStat.MoveSpeed] = value; }
        }
        /// <summary>
        /// Gets or sets the base rate of attack of the unit measured in attacks per second. 
        /// </summary>
        public float BaseAttacksPerSecond
        {
            get { return baseStats[UnitStat.AttacksPerSecond]; }
            set { baseStats[UnitStat.AttacksPerSecond] = value; }
        }
        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public float BaseAttackRange
        {
            get { return baseStats[UnitStat.AttackRange]; }
            set { baseStats[UnitStat.AttackRange] = value; }
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
        public float MaxLife => stats[UnitStat.MaxLife];
        /// <summary>
        /// Gets the current life regeneration rate of the unit, in life points per second.  
        /// </summary>
        public float LifeRegen => stats[UnitStat.LifeRegen];
        /// <summary>
        /// Gets the maximum mana of the unit. 
        /// </summary>
        public float MaxMana => stats[UnitStat.MaxMana];
        /// <summary>
        /// Gets the current mana regeneration rate of the unit, in mana points per second.  
        /// </summary>
        public float ManaRegen => stats[UnitStat.ManaRegen];

        /// <summary>
        /// Gets the minimum damage of the unit's attack. 
        /// </summary>
        public float MinDamage => stats[UnitStat.MinDamage];
        /// <summary>
        /// Gets the maximum damage of the unit's attack. 
        /// </summary>
        public float MaxDamage => stats[UnitStat.MaxDamage];
        /// <summary>
        /// Gets the base bonus magic damage of the unit. 
        /// </summary>
        public float MagicDamage => stats[UnitStat.MagicDamage];
        /// <summary>
        /// Gets the current defense of the unit which provides reduction
        /// against physical damage. 
        /// </summary>
        public float Defense => stats[UnitStat.Defense];

        /// <summary>
        /// Gets the current movement speed of the unit. 
        /// </summary>
        public float MoveSpeed => stats[UnitStat.MoveSpeed];
        /// <summary>
        /// Gets the time this unit takes between successive attacks. 
        /// </summary>
        public float AttacksPerSecond => stats[UnitStat.AttacksPerSecond];
        /// <summary>
        /// Gets the current attack range of the unit. 
        /// </summary>
        public float AttackRange => stats[UnitStat.AttackRange];


        public bool CanMove => MoveSpeed > 0;

        #endregion


        void initStats()
        {
            baseStats[UnitStat.MaxLife] = 10;
            baseStats[UnitStat.MaxMana] = 0;

            baseStats[UnitStat.LifeRegen] = 0.1f;
            baseStats[UnitStat.ManaRegen] = 0;
            baseStats[UnitStat.MagicDamage] = 0;

            baseStats[UnitStat.MinDamage] = 1;
            baseStats[UnitStat.MaxDamage] = 2;
            baseStats[UnitStat.Defense] = 0;

            baseStats[UnitStat.MoveSpeed] = 10;
            baseStats[UnitStat.AttacksPerSecond] = 0.6f;
            baseStats[UnitStat.AttackRange] = 2.5f;
        }

        internal void refreshStats()
        {
            stats.Set(baseStats);
            foreach (var b in Buffs)
                stats.Add(b.Prototype.unitStats);
        }
    }
}

