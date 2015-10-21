﻿using Engine.Objects.Game;
using Engine.Systems.Behaviours;
using Engine.Systems.Orders;
using IO;
using IO.Common;
using IO.Message.Client;
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
        public virtual OrderType OrderType
        {
            get { return Order?.Type ?? OrderType.Stand; }
        }

        /// <summary>
        /// Gets or sets the behaviour that will issue this unit some orders. 
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
        public bool IsMoving
        {
            get { return (Order is IMoveOrder); }
        }

        /// <summary>
        /// Gets the direction in which this unit is moving, or <see cref="double.NaN"/> if it is standing. 
        /// </summary>
        public double MoveDirection
        {
            get { return (Order as IMoveOrder)?.Direction ?? double.NaN; }
        }

        /// <summary>
        /// Updates the current behaviour and then state, 
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

            //update the behaviour
            if (Behaviour != null)
            {
                Behaviour.Update(msElapsed);

                // change to a new order?
                if (!CustomOrder && Behaviour.CurrentOrder != Order)
                {
                    Order = Behaviour.CurrentOrder;

                    //raise the order changed event
                    OrderChanged?.Invoke(Order);
                    AnyOrderChanged?.Invoke(this, Order);
                }
            }

            //then the order
            if (Order != null)
            {
                var toContinue = Order.Update(this, msElapsed);

                if (!toContinue)
                    Order = new Stand();
            }
        }

        /// <summary>
        /// Issues an order to the unit. 
        /// </summary>
        /// <param name="ord"></param>
        public void SetOrder(IOrder ord)
        {
            this.Order = ord;
            this.CustomOrder = true;
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
            switch(msg.TargetType)
            {
                case AbilityTargetType.PointTarget:
                    target = msg.TargetLocation;
                    break;

                case AbilityTargetType.UnitTarget:
                    target = Map.GetByGuid(msg.TargetGuid);
                    if (target == null)
                        return;
                    break;
            }
            CastAbility(ability, target);
        }


        //TODO: implement these
        #region Custom Orders
        public void OrderMove(Vector target)
        {
            this.Order = new MoveLocation(target);
        }

        public void OrderMove(Unit target, bool follow = true)
        {
            this.Order = new MoveUnit(target, keepFollowing: follow);
        }


        public void OrderHoldPosition()
        {
            this.Order = new Stand();
            this.CustomOrder = true;
        }

        public void OrderAttack(Unit target)
        {
            throw new NotImplementedException();
        }

        public void OrderPatrol(Vector target)
        {
            throw new NotImplementedException();
        }

        public void OrderMoveAttack(Vector target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears the current order of the unit. 
        /// </summary>
        public void ClearOrder()
        {
            this.Order = new Stand();
            this.CustomOrder = false;
        }
        #endregion
    }
}