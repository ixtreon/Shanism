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

        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        public int Level { get; protected set; }

        /// <summary>
        /// Gets the time it takes for an unit to attack as determined by <see cref="AttacksPerSecond"/>. 
        /// </summary>
        /// <returns>The time for an attack in milliseconds. </returns>
        public int AttackCooldown
        {
            get { return (int)(1000 / AttacksPerSecond); }
        }

        public Player Owner { get; private set; }

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
        /// Gets the current movement speed of the unit. 
        /// </summary>
        public double MoveSpeed { get; protected internal set; }

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

        /// <summary>
        /// Gets or sets whether the unit is invulnerable. 
        /// </summary>
        public bool Invulnerable { get; set; }

        /// <summary>
        /// Gets whether the unit is dead. 
        /// </summary>
        public bool IsDead { get; private set; }


        public Unit Target { get; set; }


        public double WalkSpeed { get { return MoveSpeed / 3; } }

        /// <summary>
        /// Gets a list of all the abilities of this hero. 
        /// </summary>
        public IEnumerable<IAbility> Abilities
        {
            get { return abilities.Values; }
        }

        //[ProtoMember(14)]
        private List<string> abilityNames = new List<string>();


        /// <summary>
        /// Contains mapping between ability ids (strings) and the spells. 
        /// </summary>
        protected Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();

        public UnitBuffs Buffs { get; private set; }

        /// <summary>
        /// Gets the ability being currently cast. 
        /// </summary>
        IAbility IUnit.CastingAbility
        {
            get { return OrderType == OrderType.Casting ? ((CastOrder)Order).Ability : null; }
        }

        public int CastingProgress
        {
            get { return OrderType == OrderType.Casting ? ((CastOrder)Order).Progress : -1; }
        }

        IEnumerable<IBuffInstance> IUnit.Buffs
        {
            get { return Buffs; }
        }

        protected Unit()
        {
            BaseLife = 5;
            LifePercentage = 1;
            ManaPercentage = 1;
        }

        public Unit(string model, Vector location, int level = 1)
            : base(model, location)
        {
            this.VisionRange = 5;
            this.Level = level;
            this.Size = 0.4;

            this.Buffs = new UnitBuffs(this);

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

        private void Unit_UnitInVisionRange(RangeArgs args)
        {
            var u = args.TriggerUnit;
        }


        /// <summary>
        /// Adds the given ability to the spellbook of this hero. 
        /// </summary>
        /// <param name="a"></param>
        public void AddAbility(Ability a)
        {
            if (a.Owner != null)
                throw new Exception("This ability already belongs to somebody else...");

            if (IsDead)
                return;

            a.Owner = this;
            abilities.Add(a.Name, a);
        }


        /// <summary>
        /// Gets a random value within the range of <see cref="MinDamage"/> to <see cref="MaxDamage"/>
        /// </summary>
        /// <returns></returns>
        public double DamageRoll()
        {
            return Rnd.NextDouble(MinDamage, MaxDamage + 1);
        }


        internal bool activateAbility(Ability ability, object target)
        {
            //check if we have the ability
            if (!abilities.ContainsValue(ability))
                throw new InvalidOperationException("Trying to cast a spell from another guy!");

            if (ability.Owner != this)
                throw new InvalidOperationException("Trying to cast a spell from another guy even thou we have it?!");

            //check boring stuff (mana, life?, cd)
            if (!ability.CanCast(target))
                return false;

            //execute and check custom ability handlers
            var result = ability.Cast(target);
            if (!result.Success)
                return false;

            //finally activate cooldown, remove mana
            ability.trigger();

            return true;
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
        /// Retrieves the final damage the unit will receive, if dealt a damage of the specified type and type. 
        /// </summary>
        /// <param name="amount">The amount of damage dealt by the attacker. </param>
        /// <param name="dmgType">The type of damage dealt. </param>
        /// <returns></returns>
        public double GetFinalDamage(double amount, DamageType dmgType)
        {
            return amount * getModifer(dmgType);
        }

        /// <summary>
        /// Causes this unit to damage the specified unit. 
        /// </summary>
        /// <param name="target">The unit to deal damage to. </param>
        /// <param name="dmgType">The type of damage to deal. </param>
        /// <param name="amount">The amount of damage to deal. </param>
        /// <returns></returns>
        public bool DamageUnit(Unit target, DamageType dmgType, double amount)
        {
            if (target.IsDead || target.Invulnerable)
                return false;

            //if it's physical damage we can dodge it. 
            if (dmgType == DamageType.Physical)
            {
                if (Rnd.Next(0, 100) < target.Dodge)
                    return false;
            }

            //todo: if it is magical damage, increase it. 
            //based on manacost?

            // calculate damage resistance
            // raw resistance for magics and armor multiplier for physical.  

            var dmgArgs = new UnitDamagingArgs(this, target, dmgType, amount);
            this.DamageDealt?.Invoke(dmgArgs);

            var finalDmg = target.GetFinalDamage(dmgArgs.BaseDamage, dmgArgs.DamageType);
            target.Life -= finalDmg;

            var receiveArgs = new UnitDamagedArgs(this, target, dmgType, amount, finalDmg);
            target.DamageReceived?.Invoke(receiveArgs);

            //check for death
            if (target.LifePercentage <= 0)
            {
                target.Kill(this);
                
                //drop items?

                //fire scenario event
                target.Game.Scenario.RunScripts(s => s.OnUnitDeath(target));
            }

            return true;
        }

        /// <summary>
        /// Gets the experience rewarded when the unit is killed. 
        /// </summary>
        /// <returns></returns>
        public virtual int GetExperienceReward()
        {
            return Constants.ExperienceBase + this.Level * Constants.ExperiencePerLevel;
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

        /// <summary>
        /// Updates the <see cref="Mana"/> and <see cref="Life"/> based on <see cref="ManaRegen"/> and <see cref="LifeRegen"/>, respectively. 
        /// </summary>
        /// <param name="msElapsed"></param>
        private void regenerate(int msElapsed)
        {
            if (Life < MaxLife)
            {
                var lifeDelta = LifeRegen * msElapsed / 1000;
                this.Life = Math.Min(MaxLife, Life + lifeDelta);
            }

            if (Mana < MaxMana)
            {
                var newMana = this.Mana + this.ManaRegen * msElapsed / 1000;
                this.Mana = Math.Min(this.MaxMana, newMana);
            }
        }

        /// <summary>
        /// Gets the modifier for damage of the selected type inflicted on this unit. 
        /// </summary>
        /// 
        /// <example>
        /// For example a unit with 0 armor should have 0% physical reduction 
        /// or in other words has a modifier of 1.0. 
        /// </example>
        private double getModifer(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physical:
                    return 1 / (Constants.DamageReductionPerDefense * Defense + 1);
                case DamageType.Light:
                    return 0;
                case DamageType.Shadow:
                    return 0;
                case DamageType.Dark:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }

    }

}
