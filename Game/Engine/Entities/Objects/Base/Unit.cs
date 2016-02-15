using Engine.Common;
using Engine.Events;
using Engine.Entities.Objects;
using Engine.Systems;
using Engine.Systems.Abilities;
using Engine.Systems.Buffs;
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

namespace Engine.Entities
{
    /// <summary>
    /// Represents an in-game unit. This includes NPCs, heroes, buildings. 
    /// </summary>
    public abstract partial class Unit : GameObject, IUnit
    {

        #region Base Values
        /// <summary>
        /// Gets or sets the base hit points (life) of the unit. 
        /// </summary>
        public double BaseLife { get; protected set; }

        /// <summary>
        /// Gets or sets the base mana of the unit. 
        /// </summary>
        public double BaseMana { get; protected set; }

        /// <summary>
        /// Gets or sets the base dodge chance of the unit. 
        /// </summary>
        public double BaseDodgeChance { get; protected set; }

        /// <summary>
        /// Gets or sets the base chance of dealing a critical strike for this unit. 
        /// </summary>
        public double BaseCritChance { get; protected set; }

        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public double BaseDefense { get; protected set; }

        /// <summary>
        /// Gets or sets the base magic damage of the unit. 
        /// </summary>
        public double BaseMagicDamage { get; protected set; }

        /// <summary>
        /// Gets or sets the base movement speed of the units in squares per second. 
        /// TODO: add buff modifier
        /// </summary>
        public double BaseMoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the base minimum damage inflicted by the unit. 
        /// </summary>
        public double BaseMinDamage { get; protected set; }

        /// <summary>
        /// Gets or sets the base maximum damage inflicted by the unit. 
        /// </summary>
        public double BaseMaxDamage { get; protected set; }

        /// <summary>
        /// Gets or sets the base rate of attack of the unit measured in attacks per second. 
        /// </summary>
        public double BaseAttacksPerSecond { get; protected set; }
        #endregion


        #region Stats
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
        /// Gets the current defense of the unit. 
        /// </summary>
        public double Defense { get; protected internal set; }

        /// <summary>
        /// Gets the unit's chance to dodge an attack, in the range 0 to 100. 
        /// </summary>
        public double DodgeChance { get; protected internal set; }

        /// <summary>
        /// Gets the unit's chance to dodge an attack, in the range 0 to 100. 
        /// </summary>
        public double CritChance { get; protected internal set; }

        /// <summary>
        /// Gets the base bonus magic damage of the unit. 
        /// </summary>
        public double MagicDamage { get; protected internal set; }



        /// <summary>
        /// Gets or sets the type of attack of zhe unit. 
        /// </summary>
        public bool HasRangedAttack { get; internal set; }

        /// <summary>
        /// Gets or sets he attack range of the unit. 
        /// </summary>
        public double AttackRange { get; internal set; }

        /// <summary>
        /// Gets or sets the rato of the current life of the unit to its max life 
        /// as a number between 0 and 1. 
        /// </summary>
        public double LifePercentage { get; set; }

        /// <summary>
        /// Gets or sets the rato of the current mana of the unit to its max mana 
        /// as a number between 0 and 1. 
        /// </summary>
        public double ManaPercentage { get; set; }
        #endregion


        /// <summary>
        /// Gets or sets whether the unit is invulnerable. 
        /// </summary>
        public bool Invulnerable { get; set; }


        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        public int Level { get; protected set; }

        /// <summary>
        /// Gets the owner of this unit. 
        /// </summary>
        public Player Owner { get; private set; }


        internal SortedSet<UnitRangeEvent> RangeEvents { get; } = new SortedSet<UnitRangeEvent>();


        /// <summary>
        /// Gets a collection of all abilities owned by unit. 
        /// </summary>
        public IUnitAbilities Abilities { get { return abilities; } }

        /// <summary>
        /// Gets a collection of all buffs currently affecting this unit. 
        /// </summary>
        public IUnitBuffs Buffs { get { return buffs; } }

        #region Subsystems
        internal AbilitySystem abilities { get; }
        internal MovementSystem movement { get; }
        BuffSystem buffs { get; }
        VisionSystem vision { get; }
        UnitRangeSystem range { get; }
        OrdersSystem orders { get; }
        BehaviourSystem behaviour { get; }
        CombatSystem combat { get; }


        List<UnitSystem> Systems { get; } = new List<UnitSystem>();
        #endregion


        /// <summary>
        /// Gets whether the unit is dead. 
        /// </summary>
        public bool IsDead { get; private set; }


        public override ObjectType Type { get { return ObjectType.Unit; } }


        /// <summary>
        /// Gets or sets the current life points of the unit. 
        /// </summary>
        public double Life
        {
            get { return LifePercentage * MaxLife;
            }
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

        IEnumerable<IBuffInstance> IUnit.Buffs
        {
            get { return Buffs; }
        }

        //IPlayer IUnit.Owner
        //{
        //    get { return Owner; }
        //}

        IEnumerable<IAbility> IUnit.Abilities { get { return abilities; } }

        public int CastingProgress { get { return (Order as CastOrder?)?.Progress ?? -1; } }

        public int TotalCastingTime {  get { return (Order as CastOrder?)?.Ability?.CastTime ?? -1; } }
        #endregion

        public override bool HasCollision {  get { return !IsDead && !StateFlags.HasFlag(UnitFlags.NoCollision); } }

        protected Unit()
        {
            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;

            Owner = Player.NeutralAggressive;
        }

        protected Unit(Player owner, int level = 1)
        {
            Owner = owner;
            Level = level;

            Scale = 2;
            BaseAttacksPerSecond = 0.6;
            BaseMinDamage = 0;
            BaseMaxDamage = 2;
            BaseMoveSpeed = 20;
            BaseDefense = 0;
            BaseDodgeChance = 5;
            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;

            VisionRange = 5;


            Systems.Add(buffs = new BuffSystem(this));
            Systems.Add(abilities = new AbilitySystem(this));
            Systems.Add(movement = new MovementSystem(this));
            Systems.Add(range = new UnitRangeSystem(this));
            Systems.Add(vision = new VisionSystem(this));
            Systems.Add(orders = new OrdersSystem(this));
            Systems.Add(behaviour = new BehaviourSystem(this));
            Systems.Add(combat = new CombatSystem(this));

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
                    //var sw = Stopwatch.StartNew();

                    sys.Update(msElapsed);

                    //sw.Stop();
                    //PerfCounter.Log(sys.GetType().Name, sw.ElapsedTicks);
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
            Dying?.Invoke(args);
        }

    }

}
