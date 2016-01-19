using Engine.Objects.Game;
using Engine.Systems.Abilities;
using Engine.Systems.Behaviours;
using Engine.Systems.Orders;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects.Game
{
    /// <summary>
    /// A simple type of unit that has prepopulated values for all attributes 
    /// and can be instantiated by providing values for its level and rarity. 
    /// </summary>
    public class Monster : Unit
    {
        public Monster(Vector location)
            : this(location, 1, false)
        {

        }

        /// <summary>
        /// Creates a new monster of the specified level. 
        /// </summary>
        /// <param name="model">The model of the monster. </param>
        /// <param name="location">The position of the monster. </param>
        /// <param name="level">The level of the monster. </param>
        /// <param name="isElite">Whether the monster gets bonus life n damage and is considered elite. </param>
        public Monster(Vector location, int level = 1, bool isElite = false)
            : base(Player.NeutralAggressive, location, level)
        {
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
            BaseMoveSpeed = 3;
            BaseDodge = 10;

            AttackRange = 1;
            HasRangedAttack = false;

            Abilities.Add(new Attack());
            Behaviour = new AggroBehaviour(this);
        }

        /// <summary>
        /// Creates a clone of the given monster at its current position. 
        /// </summary>
        /// <param name="prototype"></param>
        public Monster(Monster prototype)
            : this(prototype.Position, prototype.Level)
        {
            ModelName = prototype.ModelName;
        }
    }
}
