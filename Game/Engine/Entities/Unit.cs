using Shanism.Common.Game;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using Shanism.Common.Util;
using Shanism.Engine.Events;
using Shanism.Engine.Objects;
using Shanism.Engine.Objects.Items;
using Shanism.Engine.Systems;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// Represents an in-game unit. This includes NPCs, heroes, buildings. 
    /// </summary>
    public abstract partial class Unit : Entity, IUnit
    {
        /// <summary>
        /// Gets the object type of this unit. 
        /// Always has a value of <see cref="ObjectType.Unit"/>. 
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Unit;


        #region Base (Combat) Stats
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public double BaseMaxLife { get; set; } = 100;

        /// <summary>
        /// Gets or sets the base mana of the unit. 
        /// </summary>
        public double BaseMaxMana { get; set; } = 0;

        /// <summary>
        /// Gets or sets the base dodge chance of the unit. 
        /// </summary>
        public double BaseDodgeChance { get; set; } = 0;

        /// <summary>
        /// Gets or sets the base chance of dealing a critical strike for this unit. 
        /// </summary>
        public double BaseCritChance { get; set; } = 0;

        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public double BaseDefense { get; set; } = 0;

        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public double BaseMagicDamage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the base movement speed of the units in squares per second. 
        /// </summary>
        public double BaseMoveSpeed { get; set; } = 10;

        /// <summary>
        /// Gets or sets the base minimum damage inflicted by the unit. 
        /// </summary>
        public double BaseMinDamage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the base maximum damage inflicted by the unit. 
        /// </summary>
        public double BaseMaxDamage { get; set; } = 2;

        /// <summary>
        /// Gets or sets the base rate of attack of the unit measured in attacks per second. 
        /// </summary>
        public double BaseAttacksPerSecond { get; set; } = 0.75;


        #endregion


        #region Current Stats

        /// <summary>
        /// Gets or sets the life percentage of this unit
        /// as a number between 0 and 1. 
        /// </summary>
        public double LifePercentage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the mana percentage of this unit
        /// as a number between 0 and 1. 
        /// </summary>
        public double ManaPercentage { get; set; } = 1;


        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        public int Level { get; protected internal set; }


        /// <summary>
        /// Gets the maximum life of the unit. 
        /// </summary>
        public double MaxLife { get; protected internal set; }

        /// <summary>
        /// Gets the current life regeneration rate of the unit, in life points per second.  
        /// </summary>
        public double LifeRegen { get; protected internal set; }

        /// <summary>
        /// Gets the maximum mana of the unit. 
        /// </summary>
        public double MaxMana { get; protected internal set; }

        /// <summary>
        /// Gets the current mana regeneration rate of the unit, in mana points per second.  
        /// </summary>
        public double ManaRegen { get; protected internal set; }

        /// <summary>
        /// Gets the time this unit takes between successive attacks. 
        /// </summary>
        public double AttacksPerSecond { get; protected internal set; }
        /// <summary>
        /// Gets the minimum damage of the unit's attack. 
        /// </summary>
        public double MinDamage { get; protected internal set; }
        /// <summary>
        /// Gets the maximum damage of the unit's attack. 
        /// </summary>
        public double MaxDamage { get; protected internal set; }

        /// <summary>
        /// Gets the current defense of the unit which provides reduction
        /// against physical damage. 
        /// </summary>
        public double Defense { get; protected internal set; }

        /// <summary>
        /// Gets the unit's chance to dodge an attack, in the range 0 to 100. 
        /// </summary>
        public double DodgeChance { get; protected internal set; }

        /// <summary>
        /// Gets the unit's chance to dodge an attack, in the range 0 to 100. 
        /// NYI
        /// </summary>
        public double CritChance { get; protected internal set; }

        /// <summary>
        /// Gets the base bonus magic damage of the unit. 
        /// </summary>
        public double MagicDamage { get; protected internal set; }

        /// <summary>
        /// Gets the current attack range of the unit. 
        /// </summary>
        public double AttackRange { get; protected internal set; }
        #endregion


        /// <summary>
        /// Gets the owner of this unit. 
        /// </summary>
        public Player Owner { get; private set; }


        #region Subsystems

        readonly List<UnitSystem> Systems = new List<UnitSystem>();

        internal readonly AbilitySystem abilities;
        internal readonly MovementSystem movement;
        internal readonly RangeSystem range;
        internal readonly VisionSystem vision;
        readonly InventorySystem inventory;
        readonly BuffSystem buffs;
        readonly DecaySystem decay;
        readonly OrderSystem orders;
        readonly BehaviourSystem behaviour;
        readonly CombatSystem combat;

        #endregion


        /// <summary>
        /// Gets a collection of all abilities owned by the unit. 
        /// </summary>
        public IUnitAbilities Abilities => abilities;

        /// <summary>
        /// Gets a collection of all buffs currently affecting this unit. 
        /// </summary>
        public IUnitBuffs Buffs => buffs;

        public IUnitInventory Inventory => inventory;


        /// <summary>
        /// Gets or sets the current life points of the unit. 
        /// </summary>
        public double Life
        {
            get { return LifePercentage * MaxLife; }
            set
            {
                if (value < 0)
                    LifePercentage = 0;
                else if (value > MaxLife)
                    LifePercentage = 1;
                else
                    LifePercentage = value / MaxLife;
            }
        }

        /// <summary>
        /// Gets or sets the current mana points of the unit. 
        /// </summary>
        public double Mana
        {
            get { return ManaPercentage * MaxMana; }
            set
            {
                if (MaxMana > 0)
                    ManaPercentage = value / MaxMana;
                else
                    ManaPercentage = 1;
            }
        }


        #region IUnit implementation
        /// <summary>
        /// Gets the ID of the owner of the unit.
        /// </summary>
        public uint OwnerId => Owner.Id;

        /// <summary>
        /// Gets the ability this unit is currently casting.
        /// </summary>
        public uint CastingAbilityId => abilities.CastingAbility?.Id ?? 0;

        /// <summary>
        /// Gets the progress of the ability the unit is currently casting or null if no ability is being cast. 
        /// </summary>
        public int CastingProgress => abilities.CastingProgress;

        /// <summary>
        /// Gets the progress of the ability the unit is currently casting or null if no ability is being cast. 
        /// </summary>
        public int TotalCastingTime => abilities.CastingAbility?.CastTime ?? 0;


        IReadOnlyCollection<IAbility> IUnit.Abilities => abilities;

        IReadOnlyCollection<IBuffInstance> IUnit.Buffs => buffs;

        //IReadOnlyCollection<IItem> IUnit.BackpackItems => inventory.BackpackItems;

        //IReadOnlyDictionary<EquipSlot, IItem> IUnit.EquipItems => inventory.EquippedItems
        //    .ToDictionary(kvp => kvp.Key, kvp => (IItem)kvp.Value);     //..type system ftw
        #endregion

        /// <summary>
        /// Gets whether this unit has collision. 
        /// This is determined by the current <see cref="States"/> of the unit
        /// unless the unit is dead. 
        /// </summary>
        public override bool HasCollision
            => !IsDead && !States.HasFlag(StateFlags.NoCollision);

        /// <summary>
        /// Gets whether the unit attacks using projectiles. 
        /// </summary>
        public bool HasRangedAttack => States.HasFlag(StateFlags.RangedAttack);



        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        protected Unit()
            : this(Player.Aggressive, 1)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="level">The level.</param>
        protected Unit(Player owner, int level = 1)
        {
            Systems.Add(buffs = new BuffSystem(this));
            Systems.Add(inventory = new InventorySystem(this));
            Systems.Add(decay = new DecaySystem(this));
            Systems.Add(abilities = new AbilitySystem(this));
            Systems.Add(movement = new MovementSystem(this));
            Systems.Add(range = new RangeSystem(this));
            Systems.Add(vision = new VisionSystem(this));
            Systems.Add(orders = new OrderSystem(this));
            Systems.Add(behaviour = new BehaviourSystem(this));
            Systems.Add(combat = new CombatSystem(this));

            Level = level;
            SetOwner(owner);
        }

        public void SetOwner(Player newOwner)
        {
            if (Owner == newOwner)
                return;

            Owner?.RemoveControlledUnit(this);
            Owner = newOwner;
            Owner?.AddControlledUnit(this);
        }

        /// <summary>
        /// Updates buffs. 
        /// <para>Overridden in derived classes to provide stats handling. </para>
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void Update(int msElapsed)
        {
            if (!IsDead)
            {
                //update generic subsystems
                foreach (var sys in Systems)
                {
                    UnitSystemPerfCounter.RunAndLog(sys.GetType().Name, sys.Update, msElapsed);
                }
            }

            base.Update(msElapsed);
        }

    }

}
