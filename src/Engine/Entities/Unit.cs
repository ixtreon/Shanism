using Shanism.Common;
using Shanism.Common.Entities;
using Shanism.Common.Objects;
using Shanism.Engine.Objects.Abilities;
using Shanism.Engine.Objects.Orders;
using Shanism.Engine.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// Represents an in-game unit. This includes NPCs, heroes, buildings. 
    /// </summary>
    public partial class Unit : Entity, IUnit
    {
        /// <summary>
        /// Gets the object type of this unit. 
        /// Always has a value of <see cref="ObjectType.Unit"/>. 
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Unit;

        const int DefaultReturnRange = 30;

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

        /// <summary>
        /// Gets or sets the mana percentage of this unit
        /// as a number between 0 and 1. 
        /// </summary>
        public float ManaPercentage { get; set; } = 1;


        /// <summary>
        /// Gets the base states of the unit. 
        /// </summary>
        public StateFlags BaseStates { get; set; } = StateFlags.None;

        /// <summary>
        /// Gets the enumeration of states currently affecting the unit. 
        /// </summary>
        public StateFlags StateFlags { get; protected internal set; }

        

        #region Subsystems

        readonly List<UnitSystem> Systems = new List<UnitSystem>();

        internal readonly AbilitySystem abilities;
        internal readonly MovementSystem movement;
        internal readonly UnitRangeSystem range;
        readonly InventorySystem inventory;
        readonly BuffSystem buffs;
        readonly VisionSystem vision;
        readonly DecaySystem decay;
        readonly OrderSystem behaviour;
        readonly StatsSystem combat;

        #endregion



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
        /// Gets whether this unit has collision. 
        /// Determined by <see cref="StateFlags"/>
        /// unless the unit is dead. 
        /// </summary>
        public override bool HasCollision
            => !IsDead
            && (StateFlags & StateFlags.NoCollision) != 0;

        /// <summary>
        /// Gets whether the unit attacks using projectiles. 
        /// Determined by <see cref="StateFlags"/>.
        /// </summary>
        public bool HasRangedAttack
        {
            get => (StateFlags & StateFlags.RangedAttack) != 0;
            set
            {
                if (value)
                    StateFlags |= StateFlags.RangedAttack;
                else
                    StateFlags &= ~StateFlags.RangedAttack;
            }
        }

        public Ability CastingAbility => abilities.CastingAbility;

        /// <summary>
        /// Gets the ID of the owner of the unit.
        /// </summary>
        uint IUnit.OwnerId => Owner.Id;

        /// <summary>
        /// Gets the ability this unit is currently casting.
        /// </summary>
        uint IUnit.CastingAbilityId => abilities.CastingAbility?.Id ?? 0;

        /// <summary>
        /// Gets the progress of the ability 
        /// the unit is currently casting 
        /// or null if no ability is being cast. 
        /// </summary>
        public int CastingProgress => abilities.CastingProgress;

        /// <summary>
        /// Gets the total cast time of the ability 
        /// the unit is currently casting 
        /// or null if no ability is being cast. 
        /// </summary>
        public int TotalCastingTime => abilities.CastingAbility?.CastTime ?? 0;


        IReadOnlyCollection<IAbility> IUnit.Abilities => abilities;

        IReadOnlyCollection<IBuffInstance> IUnit.Buffs => buffs;

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
        /// Creates a new <see cref="Unit" />.
        /// Note that the unit has to be manually added to the game map.
        /// </summary>
        /// <param name="owner">The owner of the unit. Default is <see cref="Player.Aggressive"/>. </param>
        /// <param name="position">The position of the newly created unit. Default is (0, 0). </param>
        /// <param name="level">The level of the unit. Default is 1. </param>
        public Unit(Player owner = null, Vector2? position = null, int level = 1)
        {
            Systems.Add(buffs = new BuffSystem(this));
            Systems.Add(inventory = new InventorySystem(this));
            Systems.Add(decay = new DecaySystem(this));
            Systems.Add(abilities = new AbilitySystem(this));
            Systems.Add(movement = new MovementSystem(this));
            Systems.Add(range = new UnitRangeSystem(this));
            Systems.Add(vision = new VisionSystem(this));
            Systems.Add(behaviour = new OrderSystem(this));
            Systems.Add(combat = new StatsSystem(this));

            DefaultOrder = new Guard(this, DefaultReturnRange);
            initStats();

            Level = level;
            Owner = owner ?? Player.Aggressive;
            Position = position ?? Vector2.Zero;
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
                //update generic subsystems if alive
                for(int i = 0; i < Systems.Count; i++)
                {
                    var sys = Systems[i];
                    UnitSystemPerfCounter.Start(sys.GetType().Name);
                    sys.Update(msElapsed);
                }

                UnitSystemPerfCounter.End();
            }

            base.Update(msElapsed);
        }

    }

}
