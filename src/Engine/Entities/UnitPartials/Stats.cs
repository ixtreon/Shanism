using Ix.Math;
using Shanism.Common;
using Shanism.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {
        internal readonly UnitStats baseStats = new UnitStats(0);
        internal readonly UnitStats stats = new UnitStats(0);
        int _baseDodge, _baseCrit;


        IUnitStats IUnit.BaseStats => baseStats;
        IUnitStats IUnit.Stats => stats;


        #region Base Stats

        /// <summary>
        /// Gets or sets the base dodge chance of the unit. 
        /// This value is always in the range [0; 100].
        /// </summary>
        public int BaseDodgeChance
        {
            get { return _baseDodge; }
            set { _baseDodge = value.Clamp(0, 100); }
        }
        /// <summary>
        /// Gets or sets the base chance of dealing a critical strike for this unit. 
        /// This value is always in the range [0; 100].
        /// </summary>
        public int BaseCritChance
        {
            get { return _baseCrit; }
            set { _baseCrit = value.Clamp(0, 100); }
        }

        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public ref float BaseMaxLife => ref baseStats.Get(UnitField.MaxLife);
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public ref float BaseLifeRegen => ref baseStats.Get(UnitField.LifeRegen);
        /// <summary>
        /// Gets or sets the base mana of the unit. 
        /// </summary>
        public ref float BaseMaxMana => ref baseStats.Get(UnitField.MaxMana);
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public ref float BaseManaRegen => ref baseStats.Get(UnitField.ManaRegen);

        /// <summary>
        /// Gets or sets the base minimum damage inflicted by the unit. 
        /// </summary>
        public ref float BaseMinDamage => ref baseStats.Get(UnitField.MinDamage);
        /// <summary>
        /// Gets or sets the base maximum damage inflicted by the unit. 
        /// </summary>
        public ref float BaseMaxDamage => ref baseStats.Get(UnitField.MaxDamage);
        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public ref float BaseMagicDamage => ref baseStats.Get(UnitField.MagicDamage);
        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public ref float BaseDefense => ref baseStats.Get(UnitField.Defense);

        /// <summary>
        /// Gets or sets the base movement speed of the unit in squares per second. 
        /// </summary>
        public ref float BaseMoveSpeed => ref baseStats.Get(UnitField.MoveSpeed);
        /// <summary>
        /// Gets or sets the base rate of attack of the unit measured in attacks per second. 
        /// </summary>
        public ref float BaseAttacksPerSecond => ref baseStats.Get(UnitField.AttacksPerSecond);
        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public ref float BaseAttackRange => ref baseStats.Get(UnitField.AttackRange);
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
        public float MaxLife => stats[UnitField.MaxLife];
        /// <summary>
        /// Gets the current life regeneration rate of the unit, in life points per second.  
        /// </summary>
        public float LifeRegen => stats[UnitField.LifeRegen];
        /// <summary>
        /// Gets the maximum mana of the unit. 
        /// </summary>
        public float MaxMana => stats[UnitField.MaxMana];
        /// <summary>
        /// Gets the current mana regeneration rate of the unit, in mana points per second.  
        /// </summary>
        public float ManaRegen => stats[UnitField.ManaRegen];

        /// <summary>
        /// Gets the minimum damage of the unit's attack. 
        /// </summary>
        public float MinDamage => stats[UnitField.MinDamage];
        /// <summary>
        /// Gets the maximum damage of the unit's attack. 
        /// </summary>
        public float MaxDamage => stats[UnitField.MaxDamage];
        /// <summary>
        /// Gets the base bonus magic damage of the unit. 
        /// </summary>
        public float MagicDamage => stats[UnitField.MagicDamage];
        /// <summary>
        /// Gets the current defense of the unit which provides reduction
        /// against physical damage. 
        /// </summary>
        public float Defense => stats[UnitField.Defense];

        /// <summary>
        /// Gets the current movement speed of the unit. 
        /// </summary>
        public float MoveSpeed => stats[UnitField.MoveSpeed];
        /// <summary>
        /// Gets the time this unit takes between successive attacks. 
        /// </summary>
        public float AttacksPerSecond => stats[UnitField.AttacksPerSecond];
        /// <summary>
        /// Gets the current attack range of the unit. 
        /// </summary>
        public float AttackRange => stats[UnitField.AttackRange];

        #endregion


        void initStats()
        {
            BaseMaxLife = 10;
            BaseMaxMana = 0;

            BaseLifeRegen = 0.1f;
            BaseManaRegen = 0.1f;
            BaseMagicDamage = 0;

            BaseMinDamage = 1;
            BaseMaxDamage = 2;
            BaseDefense = 0;

            BaseMoveSpeed = 10;
            BaseAttacksPerSecond = 2f / 3;
            BaseAttackRange = 2.5f;
        }

        internal void refreshStats()
        {
            stats.Set(baseStats);
            for(int i = Buffs.Count - 1; i >= 0; i--)
                stats.Add(Buffs[i].Prototype.unitStats);
        }
    }
}

