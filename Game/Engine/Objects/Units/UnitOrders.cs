using Engine.Objects.Game;
using Engine.Systems.Behaviours;
using Engine.Systems.Orders;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    //The part of the unit class which deals with order handling, such as moving and attacking. 
    partial class Unit
    {
        /// <summary>
        /// Gets the current order of the unit. 
        /// </summary>
        public virtual OrderType OrderType
        {
            get { return Order?.Type ?? OrderType.Stand; }
        }


        /// <summary>
        /// Gets or sets the active behaviour of the unit. 
        /// </summary>
        public Behaviour Behaviour { get; set; }

        /// <summary>
        /// Gets the current order of the unit. 
        /// </summary>
        public Order Order { get; private set; }

        /// <summary>
        /// Gets whether this unit is currently performing a custom order,
        /// not as part of its <see cref="Behaviour"/>
        /// </summary>
        public bool CustomOrder { get; private set; }

        /// <summary>
        /// Clears the current order of the unit. 
        /// </summary>
        public void ClearOrder()
        {
            this.Order = new Stand();
            this.CustomOrder = false;
        }

        /// <summary>
        /// Updates the current behaviour and state, 
        /// and finally runs the state handler to i.e. move or attack. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last call of this method, in milliseconds. </param>
        public void UpdateBehaviour(int msElapsed)
        {
            //dead units have no orders
            if (IsDead)
                return;

            //stunned units are useless
            if (StateFlags.HasFlag(UnitState.Stunned))
                return;

            // update the active behaviour. 
            Behaviour?.Update(msElapsed);

            // change to a new order?
            if (!CustomOrder && Behaviour != null && Behaviour.CurrentOrder != Order)
            {
                Order = Behaviour?.CurrentOrder;
                RaiseOrderChangedEvent();
            }

            //update the active order
            updateOrder(msElapsed);
        }

        /// <summary>
        /// Updates the current order. 
        /// 
        /// Currently called from <see cref="UpdateBehaviour(int)"/>
        /// </summary>
        /// <param name="msElapsed"></param>
        private void updateOrder(int msElapsed)
        {
            if (Order != null)
            {
                var cont = Order.Update(this, msElapsed);

                if (!cont)
                    Order = new Stand();
            }
        }

        /// <summary>
        /// Issues an order to the unit. 
        /// </summary>
        /// <param name="ord"></param>
        public void SetOrder(Order ord)
        {
            this.Order = ord;
            this.CustomOrder = true;
        }

        /// <summary>
        /// Tries to cast the given ability on the provided target. 
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public void CastAbility(Ability ability, object target)
        {
            var newOrder = new CastOrder(target, ability);

            if(!newOrder.Equals(Order))
            {
                Order = newOrder;
            }
        }

        public void OrderHoldPosition()
        {
            throw new NotImplementedException();
        }

        public void OrderAttack(Unit target)
        {
            throw new NotImplementedException();
        }

        public void OrderMove(Vector target)
        {
            this.Order = new MoveLocation(target);
        }

        public void OrderMove(Unit target, bool follow = true)
        {
            this.Order = new MoveUnit(target, keepFollowing: follow);
        }

        public void OrderPatrol(Vector target)
        {
            throw new NotImplementedException();
        }

        public void OrderMoveAttack(Vector target)
        {
            throw new NotImplementedException();
        }
    }
}
