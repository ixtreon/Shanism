﻿using Engine.Objects.Entities;
using Engine.Systems.Abilities;
using Engine.Systems.Behaviours;
using Engine.Systems.Orders;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects.Entities
{
    /// <summary>
    /// A simple type of unit that has prepopulated values for all attributes 
    /// and can be instantiated by providing values for its level and rarity. 
    /// </summary>
    public class Monster : Unit
    {
        public bool IsElite { get; }

        /// <summary>
        /// Creates a new level 1 monster. 
        /// </summary>
        public Monster()
            : this(1, false)
        {

        }


        /// <summary>
        /// Creates a new monster of the specified level. 
        /// </summary>
        /// <param name="level">The level of the monster. </param>
        /// <param name="isElite">Whether the monster gets bonus life n damage and is considered elite. </param>
        public Monster(int level, bool isElite = false)
            : base(Player.NeutralAggressive, level)
        {
            ModelName = "units/devilkin";

            IsElite = isElite;
            if(isElite)
            {
                BaseLife = 160 + 90 * level;
                BaseMinDamage = 5 + 5 * level;
                BaseMaxDamage = BaseMinDamage + 7;

                BaseDefense = 2 + 0.3 * level;
            }
            else
            {
                BaseLife = 60 + 30 * level;
                BaseMinDamage = 2 + 3 * level;
                BaseMaxDamage = BaseMinDamage + 2 + level / 10;

                BaseDefense = 1 + 0.2 * level;
            }

            BaseAttacksPerSecond = 1.75;
            BaseMoveSpeed = 10;
            BaseDodgeChance = 10;

            AttackRange = 3;
            BaseStates &= (~UnitFlags.RangedAttack);

            //States.

            Abilities.Add(new Attack());
            Behaviour = new AggroBehaviour(this);
        }

        /// <summary>
        /// Creates a clone of the given monster at its current position. 
        /// </summary>
        /// <param name="prototype"></param>
        public Monster(Monster prototype)
            : this(prototype.Level, prototype.IsElite)
        {
            Position = prototype.Position;
            ModelName = prototype.ModelName;
        }
    }
}