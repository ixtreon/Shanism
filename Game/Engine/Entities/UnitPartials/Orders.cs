using Shanism.Engine.Systems.Orders;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects.Behaviours;
using Shanism.Engine.Objects.Abilities;

namespace Shanism.Engine.Entities
{
    //The part of the unit class which deals with orders such as moving and attacking. 
    partial class Unit
    {
        /// <summary>
        /// The event executed whenever any unit's order is changed. 
        /// </summary>
        public static event Action<Unit, IOrder> AnyOrderChanged;



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
        /// The event executed whenever the unit's order is changed. 
        /// </summary>
        public event Action<IOrder> OrderChanged;


        #region Property Shortcuts

        /// <summary>
        /// Gets the type of the unit's current order. 
        /// </summary>
        public OrderType OrderType => Order?.Type ?? OrderType.Stand;

        /// <summary>
        /// Gets whether this unit is currently moving. 
        /// </summary>
        public bool IsMoving => Order is IMoveOrder;

        /// <summary>
        /// Gets the direction in which this unit is moving, if it is moving. 
        /// </summary>
        public float MoveDirection => (Order as IMoveOrder)?.Direction ?? 0;

        #endregion

        #region Casting

        /// <summary>
        /// Tries to cast the given ability on the provided target entity. 
        /// </summary>
        /// <param name="ability">The ability to cast.</param>
        /// <param name="target">The targeted entity.</param>
        /// <returns>Whether the unit began casting the ability.</returns>
        public bool TryCastAbility(Ability ability, Entity target)
            => abilities.BeginCasting(ability, target);

        /// <summary>
        /// Tries to cast the given ability on the provided target location. 
        /// </summary>
        /// <param name="ability">The ability to cast.</param>
        /// <param name="location">The target location.</param>
        /// <returns>Whether the unit began casting the ability.</returns>
        public bool TryCastAbility(Ability ability, Vector location)
            => abilities.BeginCasting(ability, location);

        /// <summary>
        /// Tries to cast the given ability without a target. 
        /// </summary>
        /// <param name="ability">The ability to cast.</param>
        /// <returns>Whether the unit began casting the ability.</returns>
        public bool TryCastAbility(Ability ability)
            => abilities.BeginCasting(ability);


        internal bool TryCastAbility(ClientState state)
        {
            if (state.ActionId == 0) return false;

            var ability = abilities.TryGet(state.ActionId);
            if (ability == null) return false;

            Entity targetEntity;

            switch(ability.TargetType)
            {
                case AbilityTargetType.Passive:
                    return false;

                case AbilityTargetType.NoTarget:
                    return TryCastAbility(ability);

                case AbilityTargetType.PointTarget:
                    return TryCastAbility(ability, state.ActionTargetLoc);

                case AbilityTargetType.UnitTarget:
                    targetEntity = Map.GetByGuid(state.ActionTargetId);
                    if (targetEntity != null)
                        return TryCastAbility(ability, targetEntity);

                    return false;

                case AbilityTargetType.PointOrUnitTarget:
                    targetEntity = Map.GetByGuid(state.ActionTargetId);
                    if (targetEntity != null)
                        return TryCastAbility(ability, targetEntity);

                    return TryCastAbility(ability, state.ActionTargetLoc);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Custom Orders

        internal void SetOrder(IOrder ord, bool isCustomOrder = true)
        {
            Order = ord;
            CustomOrder = isCustomOrder;

            //raise the order changed event
            OrderChanged?.Invoke(Order);
            AnyOrderChanged?.Invoke(this, Order);
        }

        /// <summary>
        /// Orders the unit to stop moving and stand in one place. 
        /// </summary>
        public void Stand() => SetOrder(new Stand(), true); 

        /// <summary>
        /// Clears the current order of the unit. 
        /// </summary>
        public void ClearOrder() => SetOrder(new Stand(), false);

        /// <summary>
        /// Orders the unit to start moving towards the target in-game position. 
        /// </summary>
        public void OrderMove(Vector target) => SetOrder(new MoveLocation(target));

        /// <summary>
        /// Orders the unit to start moving in the specified direction. 
        /// </summary>
        public void OrderMove(float direction) => SetOrder(new PlayerMoveOrder(direction));

        /// <summary>
        /// Orders the unit to go to the target unit. 
        /// Similar to <see cref="OrderMove(Vector)"/> but makes sure the target is reached
        /// even if it moves after the order was issued. 
        /// </summary>
        public void OrderMove(Unit target, bool follow = true) => SetOrder(new MoveUnit(target, keepFollowing: follow));


        public void OrderFollow(Unit target) => SetOrder(new MoveUnit(target, keepFollowing: true));

        public void OrderAttack(Unit target)
        {
            if (!Owner.IsEnemyOf(target))
                return;

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
        #endregion
    }
}
