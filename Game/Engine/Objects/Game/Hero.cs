using Engine.Systems;
using IO;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IO.Common;
using Engine.Common;
using Engine.Objects.Game;
using Engine.Systems.Orders;
using IO.Message.Client;

namespace Engine.Objects.Game
{
    [ProtoContract]
    public class Hero : Unit, IHero
    {
        //multiply by _level to obtain the Xp needed to level up. 
        const int XpPerLevel = 100;

        int _experience;

        /// <summary>
        /// Gets the experience needed to reach the next level. 
        /// </summary>
        public int ExperienceNeeded
        {
            get
            {
                return XpPerLevel * Level;
            }
        }

        public override ObjectType ObjectType
        {
            get { return ObjectType.Hero; }
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
                    Level++;
                }
            }
        }

        /// <summary>
        /// Gets or sets the base strength of this hero. 
        /// </summary>
        [ProtoMember(2)]
        public double BaseStrength { get; set; }
        /// <summary>
        /// Gets or sets the base vitality of this hero. 
        /// </summary>
        [ProtoMember(3)]
        public double BaseVitality { get; set; }
        /// <summary>
        /// Gets or sets the base intellect of this hero. 
        /// </summary>
        [ProtoMember(4)]
        public double BaseIntellect { get; set; }
        /// <summary>
        /// Gets or sets the base agility of this hero. 
        /// </summary>
        [ProtoMember(5)]
        public double BaseAgility { get; set; }



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
        /// Gets the movement state of this hero, if it is moving. 
        /// </summary>
        public MovementState MoveState
        {
            get { return (OrderType == OrderType.Move) ? ((PlayerMoveOrder)Order).State : MovementState.Stand; }
        }

        internal Hero()
            : base()
        {
            this.BaseMana = 5;
            this.MaxMana = 5;
        }


        public Hero(Player owner, Vector location)
            : base("hero", owner, location)
        {
            this.BaseMoveSpeed = 5;
            this.Size = 0.6;

            this.BaseStrength = 5;
            this.BaseVitality= 5;
            this.BaseIntellect = 5;
            this.BaseAgility = 5;
            
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

        /// <summary>
        /// Fired whenever the hero requests the activation of some action. 
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnAction(ActionMessage args)
        {
            Ability ability;
            if(!abilities.TryGetValue(args.AbilityId, out ability))
                return;

            CastAbility(ability, args.TargetLocation);
        }

        internal override void UpdateStats(int msElapsed)
        {
            base.UpdateStats(msElapsed);
            Strength = BaseStrength + Buffs.Sum(b => b.Strength);
            Vitality = BaseVitality + Buffs.Sum(b => b.Vitality);
            Intellect = BaseIntellect + Buffs.Sum(b => b.Intellect);
            Agility = BaseAgility + Buffs.Sum(b => b.Agility);

            MinDamage += Strength * Constants.Attributes.DamagePerStrength;
            MaxDamage += Strength * Constants.Attributes.DamagePerStrength;
            Defense += Strength * Constants.Attributes.DefensePerStrength;

            Defense += Agility * Constants.Attributes.AtkSpeedPerAgility;
            Defense += Agility * Constants.Attributes.DodgePerAgility;

            MaxLife += Vitality * Constants.Attributes.LifePerVitality;
            LifeRegen += Vitality * Constants.Attributes.LifeRegPerVitality;
            MaxMana += Vitality * Constants.Attributes.ManaPerVitality;
            ManaRegen += Vitality * Constants.Attributes.ManaRegPerVitality;

            MagicDamage += Intellect * Constants.Attributes.MagicDamagePerInt;
            ManaRegen += Intellect * Constants.Attributes.ManaRegPerInt;
        }

        ///// <summary>
        ///// Updates hero's abilities (custom handlers, cooldown)
        ///// in addition to the default buff handling provided by <see cref="Unit"/>
        ///// </summary>
        //internal override void Update(int msElapsed)
        //{
        //    base.Update(msElapsed);
        //}
    }

}
