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
using IO.Objects;

namespace Engine.Objects.Game
{
    public class Hero : Unit, IHero
    {
        //multiplied by _level to obtain the Xp needed to level up. 
        const int XpPerLevel = 100;

        /// <summary>
        /// The total experience of the unit. 
        /// </summary>
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
        public double BaseStrength { get; set; }
        /// <summary>
        /// Gets or sets the base vitality of this hero. 
        /// </summary>
        public double BaseVitality { get; set; }
        /// <summary>
        /// Gets or sets the base intellect of this hero. 
        /// </summary>
        public double BaseIntellect { get; set; }
        /// <summary>
        /// Gets or sets the base agility of this hero. 
        /// </summary>
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


        /// <summary>
        /// Fired whenever the hero requests the activation of some action. 
        /// </summary>
        /// <param name="args"></param>
        internal virtual void OnAction(ActionMessage args)
        {
            Ability ability;
            if(!abilities.TryGetValue(args.AbilityId, out ability))
                return;

            CastAbility(ability, args.TargetLocation);
        }

        internal override void UpdateBuffs(int msElapsed)
        {
            base.UpdateBuffs(msElapsed);
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
    }

}
