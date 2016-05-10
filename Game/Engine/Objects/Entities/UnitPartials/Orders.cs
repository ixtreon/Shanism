using Shanism.Engine.Objects.Entities;
using Shanism.Engine.Systems.Abilities;
using Shanism.Engine.Systems.Behaviours;
using Shanism.Engine.Systems.Orders;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects
{
    //The part of the unit class which deals with order handling, such as moving and attacking. 
    partial class Unit
    {

        /// <summary>
        /// The event executed whenever any unit's order is changed. 
        /// </summary>
        public static event Action<Unit, IOrder> AnyOrderChanged;

        /// <summary>
        /// The event executed whenever the unit's order is changed. 
        /// </summary>
        public event Action<IOrder> OrderChanged;


        /// <summary>
        /// Gets the type of the unit's current order. 
        /// </summary>
        public virtual OrderType OrderType => Order?.Type ?? OrderType.Stand;

        /// <summary>
        /// Gets or sets the current behaviour of this unit. 
        /// </summary>
        public Behaviour Behaviour { get; set; }

        /// <summary>
        /// Gets the current order of the unit. 
        /// </summary>
        public IOrder Order { get; private set; }

        /// <summary>
        /// Gets whether this unit is currently performing a custom order,
        /// i.e. one not part of its <see cref="Behaviour"/>
        /// </summary>
        public bool CustomOrder { get; private set; }

        /// <summary>
        /// Gets whether this unit is currently moving. 
        /// </summary>
        public bool IsMoving => Order is IMoveOrder;

        /// <summary>
        /// Gets the direction in which this unit is moving, or <see cref="double.NaN"/> if it is standing. 
        /// </summary>
        public double MoveDirection => (Order as IMoveOrder)?.Direction ?? double.NaN;

        internal void SetOrder(IOrder ord, bool isCustomOrder = true)
        {
            Order = ord;
            CustomOrder = isCustomOrder;

            //raise the order changed event
            OrderChanged?.Invoke(Order);
            AnyOrderChanged?.Invoke(this, Order);
        }

        /// <summary>
        /// Tries to cast the given ability on the provided target. 
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="target"></param>
        public void CastAbility(Ability ability, object target)
        {
            var newOrder = new CastOrder(ability, target);

            if(!newOrder.Equals(Order))
                Order = newOrder;
        }

        internal void TryCastAbility(ActionMessage msg)
        {
            //check if we got the ability
            var ability = abilities.TryGet(msg.AbilityId);
            if (ability == null)
                return;

            object target = null;
            switch(ability.TargetType)
            {
                case AbilityTargetType.PointTarget:
                    target = msg.TargetLocation;
                    break;

                case AbilityTargetType.UnitTarget:
                    target = Map.GetByGuid(msg.TargetGuid);
                    if (target == null)
                        return;
                    break;

                case AbilityTargetType.PointOrUnitTarget:
                    target = (object)Map.GetByGuid(msg.TargetGuid) ?? msg.TargetLocation;
                    break;

                case AbilityTargetType.NoTarget:

                    break;
            }
            CastAbility(ability, target);
        }


        //TODO: implement these
        #region Custom Orders

        /// <summary>
        /// Orders the unit to stop moving and stand in one place. 
        /// </summary>
        public void Stand() => SetOrder(new Stand(), true); 

        /// <summary>
        /// Clears the current order of the unit. 
        /// </summary>
        public void Clear() => SetOrder(new Stand(), false);

        //public void OrderMove(Vector target)
        //{
        //    SetOrder(new MoveLocation(target));
        //}

        //public void OrderMove(Unit target, bool follow = true)
        //{
        //    SetOrder(new MoveUnit(target, keepFollowing: follow));
        //}

        //public void OrderAttack(Unit target)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OrderPatrol(Vector target)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OrderMoveAttack(Vector target)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}
