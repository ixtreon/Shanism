using Shanism.Engine.Systems;
using Shanism.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shanism.Common.Game;
using Shanism.Engine.Common;
using Shanism.Engine.Objects.Entities;
using Shanism.Engine.Systems.Orders;
using Shanism.Common.Message.Client;
using Shanism.Common.Objects;
using Shanism.Engine.Systems.Abilities;
using Shanism.Common.Util;

namespace Shanism.Engine.Objects.Entities
{
    /// <summary>
    /// A type of unit that can gather experience points and has a number of attributes 
    /// that affect its main statistics (life, mana etc). 
    /// </summary>
    public class Hero : Unit, IHero
    {
        //multiplied by _level to obtain the Xp needed to level up. 
        const int XpPerLevel = 100;

        /// <summary>
        /// Gets the object type of this hero. 
        /// Always has a value of <see cref="ObjectType.Hero"/>. 
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Hero;


        /// <summary>
        /// The total experience of the unit. 
        /// </summary>
        int _experience;

        /// <summary>
        /// Gets the experience needed to reach the next level. 
        /// </summary>
        public int ExperienceNeeded
            => XpPerLevel * Level;

        /// <summary>
        /// The current experience of this hero.
        /// </summary>
        public int Experience
        {
            get { return _experience; }
            set
            {
                if (value < _experience)
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
        public double BaseStrength { get; set; } = 10;
        /// <summary>
        /// Gets or sets the base vitality of this hero. 
        /// </summary>
        public double BaseVitality { get; set; } = 10;
        /// <summary>
        /// Gets or sets the base intellect of this hero. 
        /// </summary>
        public double BaseIntellect { get; set; } = 10;
        /// <summary>
        /// Gets or sets the base agility of this hero. 
        /// </summary>
        public double BaseAgility { get; set; } = 10;



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
            => (Order as PlayerMoveOrder?)?.State ?? MovementState.Stand;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        /// <param name="owner">The owner of the hero.</param>
        public Hero(Player owner)
            : base(owner)
        {
            ModelName = "units/hero";

            BaseMoveSpeed = 12;

            BaseLife = 100;
            BaseMana = 5;
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
