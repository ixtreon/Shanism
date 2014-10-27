using Engine.Systems;
using IO;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Engine.Common;
using IO.Common;

namespace Engine.Objects
{
    /// <summary>
    /// Represents an in-game unit. This includes NPCs, heroes, buildings. 
    /// </summary>
    [ProtoContract]
    [ProtoInclude(1, typeof(Hero))]
    public abstract class Unit : GameObject, IUnit
    {
        //[ProtoMember(3)]
        public abstract int Level { get; }

        /// <summary>
        /// Gets the time it takes for an unit to attack as determined by <see cref="AttacksPerSecond"/> 
        /// </summary>
        /// <returns>The time for an attack in milliseconds. </returns>
        public int AttackCooldown
        {
            get { return (int)(1000 / AttacksPerSecond);  }
        }

        private double _maxLife,
            _maxMana;
        public double MaxLife
        {
            get
            {
                return _maxLife;
            }

            protected set
            {
                if (value != _maxLife)
                {
                    if(_maxLife != 0 && !IsDead)
                        Life = Life * value / _maxLife;
                    _maxLife = value;
                }
            }
        }
        public double MaxMana
        {
            get
            {
                return _maxMana;
            }

            protected set
            {
                if (value != _maxMana)
                {
                    if (_maxMana != 0 && !IsDead)
                        Life = Life * value / _maxMana;
                    _maxMana = value;
                }
            }
        }
        public double AttacksPerSecond { get; protected set; }
        public double MinDamage { get; protected set; }
        public double MaxDamage { get; protected set; }
        public double Defense { get; protected set; }
        public double Dodge { get; protected set; }
        public double MoveSpeed { get; protected set; }

        //NYI
        public event Action Dying;
        public event Action<DamageType, double> Damaged;
        public event Action<DamageType, double> Damaging;


        //the following stats have no base values (or constant ones)

        public double LifeRegen { get; protected set; }

        public double ManaRegen { get; protected set; }

        public double MagicDamage { get; protected set; }



        protected List<Buff> Buffs = new List<Buff>();


        [ProtoMember(4)]
        public double Life { get; set; }

        [ProtoMember(5)]
        public double Mana { get; set; }


        [ProtoMember(6)]
        public double BaseLife;

        [ProtoMember(7)]
        public double BaseMana;

        [ProtoMember(8)]
        public double BaseDodge;

        [ProtoMember(9)]
        public double BaseDefense; 

        [ProtoMember(10)]
        public double BaseMoveSpeed;

        [ProtoMember(11)]
        public double BaseMinDamage;

        [ProtoMember(12)]
        public double BaseMaxDamage;

        [ProtoMember(13)]
        public double BaseAttacksPerSecond;

        public bool IsDead { get; private set; }

        public bool Invulnerable { get; set; }

        //[ProtoMember(14)]
        private List<string> abilityNames = new List<string>();

        protected Unit() { }

        public Unit(string name) : base(name)
        {
            this.Size = 0.4;
            this.BaseAttacksPerSecond = 0.6;
            this.BaseMinDamage = 0;
            this.BaseMaxDamage = 2;
            this.BaseMoveSpeed = 5;
            this.BaseDefense = 0;
            this.BaseDodge = 5;

            this.BaseLife = 5;
            this.MaxLife = 5;

        }

        /// <summary>
        /// Handles unit statistics changes. 
        /// </summary>
        /// <param name="secondsElapsed">the time elapsed since the last update, in seconds</param>
        protected virtual void UpdateBuffs(double msElapsed)
        {
            //Update durations, remove stale buffs
            foreach (var b in Buffs)
                b.DurationLeft -= msElapsed;
            Buffs.RemoveAll((b) => b.DurationLeft <= 0);

            //current stat is simply the base value plus the sum of all effects on it from buffs. 
            Defense = BaseDefense + Buffs.Sum(b => b.Defense);
            Dodge = BaseDodge;
            MaxLife = BaseLife + Buffs.Sum(b => b.Life);
            MaxMana = BaseMana + Buffs.Sum(b => b.Mana);
            MinDamage = BaseMinDamage + Buffs.Sum(b => b.MinDamage);
            MaxDamage = BaseMaxDamage + Buffs.Sum(b => b.MaxDamage);
            MoveSpeed = BaseMoveSpeed * (100 + Buffs.Sum(b => b.MoveSpeedPerc)) / 100;
            AttacksPerSecond = BaseAttacksPerSecond * (100 + Buffs.Sum(b => b.AttackSpeedPerc)) / 100;

            //reset the (currently) constant stats:
            //these can be tied at any moment
            //and are also modified by heroes' stats.  
            LifeRegen = Constants.BaseLifeRegen;
            ManaRegen = Constants.BaseManaRegen;
            MagicDamage = Constants.BaseMagicDamage;
        }

        /// <summary>
        /// Updates buffs. 
        /// <para>Override in derived class to provide buff handling. </para>
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void UpdateEffects(int msElapsed)
        {
            this.UpdateBuffs(msElapsed);

            //regenerate life/mana 
            this.Mana += msElapsed * this.ManaRegen / 1000;
            this.Life += msElapsed * this.LifeRegen / 1000;

            //set m to [0; maxM] for m = life,mana
            this.Life = Math.Min(this.MaxLife, this.Life);
            this.Mana = Math.Min(this.MaxMana, this.Mana);
        }


        public bool DamageUnit(Unit u, DamageType damageType, double amount)
        {
            if (this.IsDead)
                throw new Exception("Dead guy casting!");

            if (u.IsDead || u.Invulnerable)
                return false;

            //if it's physical damage we can dodge it. 
            if (damageType == DamageType.Physical)
            {
                var dodgeRoll = Rnd.Next(0, 100);

                if (dodgeRoll < u.Dodge)     //dodge!
                    return false;
            }

            //todo: if it is magical damage, increase it. 
            //base on cooldown and manacost?

            var dmgModifier = u.getResistance(damageType);     //currently allows healing if resistance > 1
            var dmgDealt = amount * dmgModifier;

            u.Life -= dmgDealt;

            //check if other guy dies
            if (u.Life <= 0)
            {
                u.Life = 0;
                u.IsDead = true;

                //award experience? drop items?

                //fire event
                u.Game.Scenario.RunScripts(s => s.OnUnitDeath(u));
            }

            return true;
        }

        private double getResistance(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physical:
                    return 1 / (Constants.DamageReductionPerDefense * Defense + 1);
                case DamageType.Light:
                    return 0;
                case DamageType.Dark:
                    return 0;
                case DamageType.Shadow:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
