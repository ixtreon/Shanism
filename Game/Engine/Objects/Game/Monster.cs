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
    /// A simple type of unit. 
    /// </summary>
    public class Monster : Unit
    {
        /// <summary>
        /// Creates a new monster of the specified level. 
        /// </summary>
        /// <param name="model">The model of the monster. </param>
        /// <param name="location">The position of the monster. </param>
        /// <param name="level">The level of the monster. </param>
        /// <param name="isElite">Whether the monster gets bonus life n damage and is considered elite. </param>
        public Monster(string model, Vector location, int level, bool isElite = false)
            : base(model, Player.NeutralAggressive, location, level)
        {
            if(isElite)
            {
                BaseLife = 210 + 90 * level;
                BaseMinDamage = 5 + 5 * level;
                BaseMaxDamage = BaseMinDamage + 7;

                BaseDefense = 3 + 0.3 * level;
            }
            else
            {
                BaseLife = 90 + 30 * level;
                BaseMinDamage = 2 + 3 * level;
                BaseMaxDamage = BaseMinDamage + 5;

                BaseDefense = 2 + 0.2 * level;
            }

            BaseAttacksPerSecond = 2.5;
            BaseMoveSpeed = 5;
            BaseDodge = 10;

            BaseMoveSpeed = 3;
            
            AttackRange = 1;
            RangedAttack = false;

            var ab = new Attack();
            AddAbility(ab);

            Behaviour = new AggroBehaviour(this, ab);
        }

        /// <summary>
        /// Creates a clone of the given monster at its current position. 
        /// </summary>
        /// <param name="prototype"></param>
        public Monster(Monster prototype)
            : this(prototype.ModelString, prototype.Position, prototype.Level)
        {

        }
    }
}
