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
        public Monster(string model, Vector location, int level)
            : base(model, location, level)
        {
            BaseLife = 90 + 10 * level;
            //BaseMinDamage = 5 + 3 * level;
            //BaseMaxDamage = BaseMinDamage + 5;
            BaseMinDamage = 1;
            BaseMaxDamage = 1;
            BaseAttacksPerSecond = 2.5;
            BaseDefense = 2 + 0.2 * level;
            BaseMoveSpeed = 5;
            BaseDodge = 10;

            BaseMoveSpeed = 3;
            

            AttackRange = 1;
            RangedAttack = false;

            var ab = new Attack();
            AddAbility(ab);

            Behaviour = new AggroBehaviour(this, ab);
        }

        Order lastOrder;

        internal override void Update(int msElapsed)
        {
            base.Update(msElapsed);
            if(Order != null && !Order.Equals(lastOrder))
            {
                lastOrder = Order;
            }
        }
        public Monster(Monster prototype)
            : this(prototype.Model, prototype.Location, prototype.Level)
        {

        }
    }
}
