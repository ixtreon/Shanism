using Engine.Common;
using Engine.Events;
using Engine.Objects.Game;
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
        public double BaseLife { get; set; }

        /// <summary>
        /// Gets or sets the base mana of the unit. 
        /// </summary>
        public double BaseMana { get; set; }

        /// <summary>
        /// Gets or sets the base dodge chance of the unit. 
        /// </summary>
        public double BaseDodge { get; set; }

        /// <summary>
        /// Gets or sets the base defense of the unit. 
        /// </summary>
        public double BaseDefense { get; set; }

        /// <summary>
        /// Gets or sets the base movement speed of the units in squares per second. 
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
        /// Gets the unit's chance to dodge an attack. 
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

        public bool RangedAttack { get; internal set; }

        public double AttackRange { get; internal set; }

        public double ActionDelay { get; set; }

        public double LifePercentage { get; set; }

        public double ManaPercentage { get; set; }
        #endregion


        /// <summary>
        /// Gets or sets whether the unit is invulnerable. 
        /// </summary>
        public bool Invulnerable { get; set; }

        public Unit Target { get; set; }


        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        public int Level { get; protected set; }


        public Player Owner { get; private set; }

        public BuffSystem Buffs { get; private set; }

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
                ManaPercentage = value / MaxMana;
            }
        }

        IEnumerable<IBuffInstance> IUnit.Buffs
        {
            get { return Buffs; }
        }

        IPlayer IUnit.Owner
        {
            get { return Owner; }
        }

        protected Unit()
        {
            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;
            Owner = Player.NeutralAggressive;
        }

        public Unit(string model, Player owner, Vector location, int level = 1)
            : base(model, location)
        {
            this.Owner = owner;
            Owner.AddControlledUnit(this);

            this.VisionRange = 5;
            this.Level = level;
            this.Size = 0.4;

            this.Buffs = new BuffSystem(this);

            this.BaseAttacksPerSecond = 0.6;
            this.BaseMinDamage = 0;
            this.BaseMaxDamage = 2;
            this.BaseMoveSpeed = 5;
            this.BaseDefense = 0;
            this.BaseDodge = 5;

            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;
        }

        /// <summary>
        /// Handles changes to base stats based on current buffs and life and mana regeneration. 
        /// </summary>
        /// <param name="secondsElapsed">the time elapsed since the last update, in seconds</param>
        internal virtual void UpdateStats(int msElapsed)
        {

            //current stat is simply the base value plus the sum of all effects on it from buffs. 
            Defense = BaseDefense + Buffs.Sum(b => b.Defense);
            Dodge = BaseDodge;
            MaxLife = BaseLife + Buffs.Sum(b => b.Life);
            MaxMana = BaseMana + Buffs.Sum(b => b.Mana);
            MinDamage = BaseMinDamage + Buffs.Sum(b => b.MinDamage);
            MaxDamage = BaseMaxDamage + Buffs.Sum(b => b.MaxDamage);
            MoveSpeed = BaseMoveSpeed * Math.Max(0, Buffs.Aggregate(1.0, (p, b) => p * (100.0 + b.MoveSpeed) / 100));
            AttacksPerSecond = BaseAttacksPerSecond * (100 + Buffs.Sum(b => b.AttackSpeed)) / 100;


            LifeRegen = Constants.BaseLifeRegen;
            ManaRegen = Constants.BaseManaRegen;
            MagicDamage = Constants.BaseMagicDamage;

            MoveSpeed = 3;

        }

        /// <summary>
        /// Updates buffs. 
        /// <para>Overridden in derived classes to provide stats handling. </para>
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void Update(int msElapsed)
        {
            if (IsDestroyed)
                throw new Exception("Unit is destroyed!");

            if (IsDead)
                return;

            UpdateStats(msElapsed);
            regenerate(msElapsed);

            //update abilities
            foreach (var a in abilities.Values)
                a.Update(msElapsed);

            //update buffs
            Buffs.Update(msElapsed);

            //update orders
            UpdateBehaviour(msElapsed);
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
