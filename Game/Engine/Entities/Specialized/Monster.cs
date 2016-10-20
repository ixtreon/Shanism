using Shanism.Engine.Entities;
using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects.Abilities;
using Shanism.Engine.Objects.Orders;
using Shanism.Common;

namespace Shanism.Engine.Entities
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
            : base(Player.Aggressive, null, level)
        {
            Model = "units/devilkin";

            IsElite = isElite;
            if(isElite)
            {
                BaseMaxLife = 160 + 90 * level;
                BaseMinDamage = 5 + 5 * level;
                BaseMaxDamage = BaseMinDamage + 7;

                BaseDefense = 2 + 0.3f * level;
            }
            else
            {
                BaseMaxLife = 60 + 30 * level;
                BaseMinDamage = 2 + 3 * level;
                BaseMaxDamage = BaseMinDamage + 2 + level / 10;

                BaseDefense = 1 + 0.2f * level;
            }

            BaseAttacksPerSecond = 1.75f;
            BaseMoveSpeed = 10;
            BaseDodgeChance = 10;

            BaseAttackRange = 3;
            BaseStates &= (~StateFlags.RangedAttack);

            Abilities.Add(new Attack());
        }

        /// <summary>
        /// Creates a clone of the given monster at its current position. 
        /// </summary>
        /// <param name="prototype"></param>
        public Monster(Monster prototype)
            : this(prototype.Level, prototype.IsElite)
        {
            Position = prototype.Position;
            Model = prototype.Model;
        }
    }
}
