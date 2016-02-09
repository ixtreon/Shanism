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
using Engine.Entities.Objects;
using Engine.Systems.Orders;
using IO.Message.Client;
using IO.Objects;
using Engine.Systems.Abilities;

namespace Engine.Entities.Objects
{
    /// <summary>
    /// A type of unit that can gather experience points and has a number of attributes 
    /// that affect its main statistics (life, mana etc). 
    /// </summary>
    public class Hero : Unit, IHero
    {
        //multiplied by _level to obtain the Xp needed to level up. 
        const int XpPerLevel = 100;




        public override ObjectType Type {  get { return ObjectType.Hero; } }


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
        public double Strength { get; internal set; }
        /// <summary>
        /// Gets the current vitality of this hero. 
        /// </summary>
        public double Vitality { get; internal set; }
        /// <summary>
        /// Gets the current intellect of this hero. 
        /// </summary>
        public double Intellect { get; internal set; }
        /// <summary>
        /// Gets the current agility of this hero. 
        /// </summary>
        public double Agility { get; internal set; }



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
            BaseLife = 100;
            BaseMana = 5;
        }


        public Hero(Player owner)
            : base(owner)
        {
            ModelName = "hero";

            Scale = 0.6;

            BaseMoveSpeed = 5;

            BaseStrength = 10;
            BaseVitality = 10;
            BaseIntellect = 10;
            BaseAgility = 10;
        }


        /// <summary>
        /// Fired whenever the player requests the activation of some action. 
        /// </summary>
        internal virtual void OnAction(ActionMessage args)
        {
            var ability = Abilities.TryGet(args.AbilityId);
            if (ability == null)
                return;

            CastAbility(ability, args.TargetLocation);
        }
    }
}
