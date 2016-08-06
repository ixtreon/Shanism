using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
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


        Player _owner;

        /// <summary>
        /// Gets the owner of this unit. 
        /// </summary>
        public Player Owner
        {
            get { return _owner; }
            set
            {
                if (_owner != value)
                {
                    _owner?.RemoveControlledUnit(this);
                    _owner = value;
                    _owner?.AddControlledUnit(this);
                }
            }
        }

        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        public int Level { get; protected internal set; }

        /// <summary>
        /// Gets or sets the life percentage of this unit
        /// as a number between 0 and 1. 
        /// </summary>
        public float LifePercentage { get; set; } = 1;
        float manaPercentage = 1;

        /// <summary>
        /// Gets or sets the mana percentage of this unit
        /// as a number between 0 and 1. 
        /// </summary>
        public float ManaPercentage { get { return manaPercentage; } set { Debug.Assert(!float.IsNaN(value)); manaPercentage = value; } }


        /// <summary>
        /// Gets the base states of the unit. 
        /// </summary>
        public StateFlags BaseStates { get; set; } = StateFlags.None;

        /// <summary>
        /// Gets the enumeration of states currently affecting the unit. 
        /// </summary>
        public StateFlags StateFlags { get; protected internal set; }


        void updateUnitStats()
        {
            unitStats.Set(baseUnitStats);
            foreach (var b in buffs)
            {
                unitStats.Add(b.Prototype.unitStats);

            }
        }

        #region Subsystems

        readonly List<UnitSystem> Systems = new List<UnitSystem>();

        internal readonly AbilitySystem abilities;
        internal readonly MovementSystem movement;
        internal readonly RangeSystem range;
        readonly InventorySystem inventory;
        readonly BuffSystem buffs;
        readonly VisionSystem vision;
        readonly DecaySystem decay;
        readonly OrderSystem orders;
        readonly BehaviourSystem behaviour;
        readonly StatsSystem combat;

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
        public float Life
        {
            get { return LifePercentage * MaxLife; }
            set
            {
                if (value <= 0)
                    LifePercentage = 0;
                else if (value >= MaxLife)
                    LifePercentage = 1;
                else
                    LifePercentage = value / MaxLife;
            }
        }

        /// <summary>
        /// Gets or sets the current mana points of the unit. 
        /// </summary>
        public float Mana
        {
            get { return ManaPercentage * MaxMana; }
            set
            {
                if (MaxMana > 0)
                {
                    if (value <= 0)
                        ManaPercentage = 0;
                    else if (value >= MaxMana)
                        ManaPercentage = 1;
                    else
                        ManaPercentage = value / MaxMana;
                }
            }
        }


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

        /// <summary>
        /// Gets whether this unit has collision. 
        /// This is determined by the current <see cref="StateFlags"/> of the unit
        /// unless the unit is dead. 
        /// </summary>
        public override bool HasCollision
            => !IsDead && !StateFlags.HasFlag(StateFlags.NoCollision);

        /// <summary>
        /// Gets whether the unit attacks using projectiles. 
        /// </summary>
        public bool HasRangedAttack => StateFlags.HasFlag(StateFlags.RangedAttack);



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
            Systems.Add(combat = new StatsSystem(this));

            Owner = owner;
            Level = level;
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
                    UnitSystemPerfCounter.RunAndLog(sys.GetType().Name, sys.Update, msElapsed);
            }

            base.Update(msElapsed);
        }

    }

}
