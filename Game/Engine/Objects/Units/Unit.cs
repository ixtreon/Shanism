using Engine.Common;
using Engine.Events;
using Engine.Objects.Game;
using Engine.Systems.Abilities;
using Engine.Systems.Orders;
using IO;
using IO.Common;
using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Objects
{
    /// <summary>
    /// Represents an in-game unit. This includes NPCs, heroes, buildings. 
    /// </summary>
    [ProtoContract]
    [ProtoInclude(1, typeof(Hero))]
    public abstract partial class Unit : GameObject, IUnit
    {

        public override ObjectType ObjectType
        {
            get { return ObjectType.Unit; }
        }


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
        /// TODO: add buff modifier
        /// </summary>
        public double BaseDodge { get; protected set; }

        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public double BaseDefense { get; protected set; }

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
        /// Gets the maximum mana of the unit. 
        /// </summary>
        public double MaxMana { get; protected internal set; }
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
        public double Dodge { get; protected internal set; }

        /// <summary>
        /// Gets the current life regeneration rate of the unit, in life points per second.  
        /// </summary>
        public double LifeRegen { get; protected internal set; }

        /// <summary>
        /// Gets the current mana regeneration rate of the unit, in mana points per second.  
        /// </summary>
        public double ManaRegen { get; protected internal set; }

        /// <summary>
        /// Gets the base bonus magic damage of the unit. 
        /// </summary>
        public double MagicDamage { get; protected internal set; }

        public bool HasRangedAttack { get; internal set; }

        /// <summary>
        /// The attack range of the unit. 
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
        /// Gets the owner also the caster of the ability. 
        /// </summary>
        public Player Owner { get; private set; }


        #region Subsystems

        /// <summary>
        /// Gets a collection of all buffs currently affecting this unit. 
        /// </summary>
        public BuffSystem Buffs { get; }

        /// <summary>
        /// Gets a collection of all abilities owned by unit. 
        /// </summary>
        public AbilitySystem Abilities { get; }

        #endregion


        /// <summary>
        /// Gets whether the unit is dead. 
        /// </summary>
        public bool IsDead { get; private set; }


        /// <summary>
        /// Gets or sets the current life points of the unit. 
        /// </summary>
        public double Life
        {
            get
            {
                return LifePercentage * MaxLife;
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
            get
            {
                return ManaPercentage * MaxMana;
            }
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

        IPlayer IUnit.Owner
        {
            get { return Owner; }
        }

        IEnumerable<IAbility> IUnit.Abilities { get { return Abilities; } }

        public int CastingProgress { get { return (Order as CastOrder?)?.Progress ?? -1; } }

        public IAbility CastingAbility { get { return (Order as CastOrder?)?.Ability; } }

        #endregion

        protected Unit()
        {
            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;
            Owner = Player.NeutralAggressive;
        }

        public Unit(Player owner, Vector location, int level = 1)
            : base(location)
        {
            Owner = owner;
            Level = level;

            Owner?.AddControlledUnit(this);

            Buffs = new BuffSystem(this);
            Abilities = new AbilitySystem(this);

            Scale = 0.4;
            BaseAttacksPerSecond = 0.6;
            BaseMinDamage = 0;
            BaseMaxDamage = 2;
            BaseMoveSpeed = 5;
            BaseDefense = 0;
            BaseDodge = 5;
            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;

            VisionRange = 5;
        }

        /// <summary>
        /// Updates the unit state based on the current buffs. 
        /// </summary>
        /// <param name="secondsElapsed">the time elapsed since the last update, in seconds</param>
        internal virtual void UpdateBuffEffects(int msElapsed)
        {
            // current stat is the base value plus the sum of all effects on it from buffs. 
            // for some stats 'plus' means different things
            MaxLife = BaseLife + Buffs.Sum(b => b.Life);
            MaxMana = BaseMana + Buffs.Sum(b => b.Mana);
            Defense = BaseDefense + Buffs.Sum(b => b.Defense);
            Dodge = 100 - Buffs.Aggregate(
                100 - BaseDodge, 
                (p, b) => p * (100 - b.Dodge) / 100);
            MinDamage = BaseMinDamage + Buffs.Sum(b => b.MinDamage);
            MaxDamage = BaseMaxDamage + Buffs.Sum(b => b.MaxDamage);
            MoveSpeed = Buffs.Aggregate(
                    BaseMoveSpeed + Buffs.Sum(b => b.MoveSpeed), 
                    (p, b) => p * (100.0 + b.MoveSpeedPercentage) / 100);
            AttacksPerSecond = BaseAttacksPerSecond * (100 + Buffs.Sum(b => b.AttackSpeed)) / 100;

            LifeRegen = Constants.Units.BaseLifeRegen;
            ManaRegen = Constants.Units.BaseManaRegen;
            MagicDamage = Constants.Units.BaseMagicDamage;
        }

        /// <summary>
        /// Updates buffs. 
        /// <para>Overridden in derived classes to provide stats handling. </para>
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void Update(int msElapsed)
        {
            //update subsystems
            Abilities.Update(msElapsed);
            Buffs.Update(msElapsed);

            //update built-in systems
            UpdateBehaviour(msElapsed);
            updateVision(msElapsed);

            //update stats
            if (!IsDead)
            {
                UpdateBuffEffects(msElapsed);
                regenerate(msElapsed);
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

            Life = 0;
            IsDead = true;
            Buffs.Clear();

            if (killer is Hero)
                ((Hero)killer).Experience += this.GetExperienceReward();

            var args = new UnitDyingArgs(this, killer ?? this);
            Dying?.Invoke(args);
        }

    }

}
