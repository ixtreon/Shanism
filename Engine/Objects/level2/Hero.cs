using Engine.Systems;
using IO;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IO.Common;
using IO.Commands;
using Engine.Common;

namespace Engine.Objects
{
    [ProtoContract]
    public class Hero : Unit, IHero
    {
        //multiply by _level to obtain the Xp needed to level up. 
        private const int XpPerLevel = 100;

        private int _experience;
        private int _level;

        /// <summary>
        /// Gets the experience needed to reach the next level. 
        /// </summary>
        public int ExperienceNeeded
        {
            get
            {
                return XpPerLevel * _level;
            }
        }

        /// <summary>
        /// Gets the current level of this hero. 
        /// </summary>
        /// <returns></returns>
        public override int Level
        {
            get { return _level; }
        }

        /// <summary>
        /// The current experience of this hero.
        /// </summary>
        [ProtoMember(1)]
        public int Experience
        {
            get { return _experience; }
            set
            {
                if (value < 0 || value < _experience)
                    throw new Exception("You must supply an experience value higher than the previous one!");
                _experience = value;
                while (_experience > ExperienceNeeded)
                {
                    _experience -= ExperienceNeeded;
                    _level++;
                }
            }
        }

        /// <summary>
        /// Gets or sets the base strength of this hero. 
        /// </summary>
        [ProtoMember(2)]
        public double BaseStrength;
        /// <summary>
        /// Gets or sets the base vitality of this hero. 
        /// </summary>
        [ProtoMember(3)]
        public double BaseVitality;
        /// <summary>
        /// Gets or sets the base intellect of this hero. 
        /// </summary>
        [ProtoMember(4)]
        public double BaseIntellect;
        /// <summary>
        /// Gets or sets the base agility of this hero. 
        /// </summary>
        [ProtoMember(5)]
        public double BaseAgility;


        private Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();

        /// <summary>
        /// Gets the current strength of this hero. 
        /// </summary>
        /// <returns></returns>
        public double Strength { get; protected set; }
        /// <summary>
        /// Gets the current vitality of this hero. 
        /// </summary>
        public double Vitality { get; protected set; }
        /// <summary>
        /// Gets the current intellect of this hero. 
        /// </summary>
        public double Intellect { get; protected set; }
        /// <summary>
        /// Gets the current agility of this hero. 
        /// </summary>
        public double Agility { get; protected set; }

        /// <summary>
        /// Gets a list of all the abilities of this hero. 
        /// </summary>
        public IEnumerable<IAbility> Abilities
        {
            get { return abilities.Values; }
        }

        /// <summary>
        /// Gets the movement state of this hero. 
        /// </summary>
        public MovementState MovementState { get; internal set; }

        internal Hero()
            : base()
        {
            this.BaseMana = 5;
            this.MaxMana = 5;
        }

        /// <summary>
        /// Adds the given ability to the spellbook of this hero. 
        /// </summary>
        /// <param name="a"></param>
        public void AddAbility(Ability a)
        {
            if (a.Hero != null)
            {
                //fuck this shit...
                throw new Exception(":(");
            }
            a.Hero = this;
            abilities.Add(a.Name, a);
        }

        public Hero(string name)
            : base(name)
        {
            this.BaseMoveSpeed = 5;
            this.Size = 0.6;
            this.Model = "hero";

            this.BaseStrength = 5;
            this.BaseVitality= 5;
            this.BaseIntellect = 5;
            this.BaseAgility = 5;

            Life = BaseLife = 100;
        }

        protected override void UpdateBuffs(double secondsElapsed)
        {
            //Vika Entity.UpdateBuffs,koeto update-va life,mana,ala bala.
            base.UpdateBuffs(secondsElapsed);

            Strength = BaseStrength + Buffs.Sum(b => b.Strength);
            Vitality = BaseVitality + Buffs.Sum(b => b.Vitality);
            Intellect = BaseIntellect + Buffs.Sum(b => b.Intellect);
            Agility = BaseAgility + Buffs.Sum(b => b.Agility);

            MinDamage += Strength * Constants.DamagePerStrength;
            MaxDamage += Strength * Constants.DamagePerStrength;
            Defense += Strength * Constants.DefensePerStrength;

            Defense += Agility * Constants.AtkSpeedPerAgility;
            Defense += Agility * Constants.DodgePerAgility;

            MaxLife += Vitality * Constants.LifePerVitality;
            LifeRegen += Vitality * Constants.LifeRegPerVitality;
            MaxMana += Vitality * Constants.ManaPerVitality;
            ManaRegen += Vitality * Constants.ManaRegPerVitality;

            MagicDamage += Intellect * Constants.MagicDamagePerInt;
            ManaRegen += Intellect * Constants.ManaRegPerInt;
        }


        /// <summary>
        /// Uses <see cref="MovementState"/> to update the position of the hero. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void UpdateLocation(int msElapsed)
        {
            var dx = Math.Sign(MovementState.XDirection);   // make sure its [-1; 1]
            var dy = Math.Sign(MovementState.YDirection);

            var dist = this.MoveSpeed * msElapsed / 1000;

            if (dx != 0 || dy != 0)
            {
                if (dx * dy != 0)                   // if we are moving on a diagonal
                    dist = dist / Math.Sqrt(2);     // scale the x/y components
                this.Location += new Vector(dx, dy) * dist;
            }
        }

        public double DamageRoll()
        {
            return Rnd.NextDouble(MinDamage, MaxDamage);
        }

        public void Save(string fileName)
        {
            using(var fs = File.Create(fileName))
            {
                Serializer.Serialize(fs, this);
            }
        }

        public static Hero Load(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            {
                var h = Serializer.Deserialize<Hero>(fs);
                return h;
            }
        }

        public static void Delete(string fileName)
        {
            File.Delete(fileName);
        }

        public virtual void OnAction(ActionArgs args)
        {
            Ability ability;
            abilities.TryGetValue(args.AbilityId, out ability);

            var manaCost = ability.ManaCost;

            //check cooldown
            if (ability.CurrentCooldown > 0)
                return;

            //check life, mana
            if (this.Mana < manaCost)
                return;

            var t = args.TargetLocation;

            var result = ability.Cast(t);

            if (result.Success)
            {
                ability.CurrentCooldown = ability.Cooldown;
                this.Mana -= manaCost;
            }
        }

        /// <summary>
        /// Updates hero's abilities 
        /// in addition to the default buff handling provided by <see cref="Unit"/>
        /// </summary>
        internal override void UpdateEffects(int msElapsed)
        {
            foreach (var a in abilities.Values)
                a.Update(msElapsed);

            base.UpdateEffects(msElapsed);
        }
    }

}
