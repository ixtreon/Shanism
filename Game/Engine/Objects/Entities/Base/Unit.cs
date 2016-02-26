using Engine.Common;
using Engine.Events;
using Engine.Objects.Entities;
using Engine.Systems;
using Engine.Systems.Abilities;
using Engine.Objects.Buffs;
using Engine.Systems.Orders;
using Engine.Systems.Range;
using IO;
using IO.Common;
using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using IO.Util;
using System.Reflection;
using Engine.Systems.Buffs;

namespace Engine.Objects
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


        #region Base Stats
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public double BaseLife { get; set; }

        /// <summary>
        /// Gets or sets the base mana of the unit. 
        /// </summary>
        public double BaseMana { get; set; }

        /// <summary>
        /// Gets or sets the base dodge chance of the unit. 
        /// </summary>
        public double BaseDodgeChance { get; set; }

        /// <summary>
        /// Gets or sets the base chance of dealing a critical strike for this unit. 
        /// </summary>
        public double BaseCritChance { get; set; }

        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public double BaseDefense { get; set; }

        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public double BaseMagicDamage { get; set; }

        /// <summary>
        /// Gets or sets the base movement speed of the units in squares per second. 
        /// TODO: add buff modifier
        /// </summary>
        public double BaseMoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the base minimum damage inflicted by the unit. 
        /// </summary>
        public double BaseMinDamage { get; set; }

        /// <summary>
        /// Gets or sets the base maximum damage inflicted by the unit. 
        /// </summary>
        public double BaseMaxDamage { get; set; }

        /// <summary>
        /// Gets or sets the base rate of attack of the unit measured in attacks per second. 
        /// </summary>
        public double BaseAttacksPerSecond { get; set; }


        #endregion


        #region Current Stats

        /// <summary>
        /// Gets or sets the life percentage of this unit
        /// as a number between 0 and 1. 
        /// </summary>
        public double LifePercentage { get; set; }

        /// <summary>
        /// Gets or sets the mana percentage of this unit
        /// as a number between 0 and 1. 
        /// </summary>
        public double ManaPercentage { get; set; }


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

        /// <summary>
        /// Gets whether the unit is dead. 
        /// </summary>
        public bool IsDead { get; private set; }


        internal SortedSet<RangeEvent> RangeEvents { get; } = new SortedSet<RangeEvent>();


        #region Subsystems

        List<UnitSystem> Systems { get; } = new List<UnitSystem>();

        internal AbilitySystem abilities { get; }
        internal MovementSystem movement { get; }
        BuffSystem buffs { get; }
        VisionSystem vision { get; }
        UnitRangeSystem range { get; }
        OrdersSystem orders { get; }
        BehaviourSystem behaviour { get; }
        CombatSystem combat { get; }

        #endregion


        /// <summary>
        /// Gets a collection of all abilities owned by the unit. 
        /// </summary>
        public IUnitAbilities Abilities => abilities;

        /// <summary>
        /// Gets a collection of all buffs currently affecting this unit. 
        /// </summary>
        public IUnitBuffs Buffs => buffs;


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
        /// Gets the IDs of all abilities of this unit.
        /// </summary>
        public IEnumerable<uint> AbilityIds => abilities.Select(a => a.Id).ToList();

        /// <summary>
        /// Gets the IDs of all buffs currently affecting the unit.
        /// </summary>
        public IEnumerable<uint> BuffIds => Buffs.Select(bi => bi.Id).ToList();

        /// <summary>
        /// Gets the ability this unit is currently casting or null otherwise.
        /// </summary>
        public uint CastingAbilityId => (Order as CastOrder?)?.Ability?.Id ?? 0;

        /// <summary>
        /// Gets the progress of the ability the unit is currently casting.
        /// </summary>
        public int CastingProgress => (Order as CastOrder?)?.Progress ?? -1;

        /// <summary>
        /// Gets the progress of the ability the unit is currently casting.
        /// </summary>
        public int TotalCastingTime => (Order as CastOrder?)?.Ability?.CastTime ?? -1;

        /// <summary>
        /// Gets whether this unit is invulnerable.
        /// </summary>
        public bool Invulnerable => (States & UnitFlags.Invulnerable) != 0;

        IEnumerable<IAbility> IUnit.Abilities => abilities;

        IEnumerable<IBuffInstance> IUnit.Buffs => buffs;
        #endregion

        /// <summary>
        /// Gets whether this unit has collision. 
        /// This is determined by the current <see cref="States"/> of the unit
        /// unless the unit is dead. 
        /// </summary>
        public override bool HasCollision
            => !IsDead && !States.HasFlag(UnitFlags.NoCollision);

        /// <summary>
        /// Gets whether the unit attacks using projectiles. 
        /// </summary>
        public bool HasRangedAttack => States.HasFlag(UnitFlags.RangedAttack);


        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        protected Unit()
        {
            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;

            Systems.Add(buffs = new BuffSystem(this));
            Systems.Add(abilities = new AbilitySystem(this));
            Systems.Add(movement = new MovementSystem(this));
            Systems.Add(range = new UnitRangeSystem(this));
            Systems.Add(vision = new VisionSystem(this));
            Systems.Add(orders = new OrdersSystem(this));
            Systems.Add(behaviour = new BehaviourSystem(this));
            Systems.Add(combat = new CombatSystem(this));

            Owner = Player.NeutralAggressive;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="level">The level.</param>
        protected Unit(Player owner, int level = 1)
            : this()
        {
            Owner = owner;
            Level = level;

            Scale = Constants.Units.DefaultUnitSize;
            BaseAttacksPerSecond = 0.6;
            BaseMinDamage = 0;
            BaseMaxDamage = 2;
            BaseMoveSpeed = 10;
            BaseDefense = 0;
            BaseDodgeChance = 5;

            VisionRange = 20;

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
                    var sw = Stopwatch.StartNew();

                    sys.Update(msElapsed);

                    sw.Stop();
                    PerfCounter.Log(sys.GetType().Name, sw.ElapsedTicks);
                }
            }

            base.Update(msElapsed);
        }

        /// <summary>
        /// Instantly kills the unit. 
        /// </summary>
        /// <param name="killer"></param>
        public void Kill(Unit killer = null)
        {
            if (IsDead)
                return;

            // update unit state
            Life = 0;
            IsDead = true;
            Buffs.Clear();


            // give out rewards
            if (killer != null)
            {
                // gold reward!

                if (killer is Hero)
                    ((Hero)killer).Experience += GetExperienceReward();
            }

            // raise the event
            var args = new UnitDyingArgs(this, killer ?? this);
            Death?.Invoke(args);

            //run the scripts
            Scenario.RunScripts(s => s.OnUnitDeath(this));
        }

    }

}
