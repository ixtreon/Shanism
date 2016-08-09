using Shanism.Engine.Events;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// A compound behaviour which makes an unit engage other units that come into range.
    /// Switches between following and attacking the current target with max aggro, unless it escapes too far away.
    /// In peaceful times releases control.
    /// </summary>
    class Stand : OrderList
    {
        public const int DefaultReturnRange = 20;


        readonly Dictionary<Unit, double> aggroTable = new Dictionary<Unit, double>();

        readonly ReturnOrder ForceReturnBehaviour;     // forces the unit to the origin if it leaves its aggro zone
        readonly SpamBehaviour SpamBehaviour;              // forces the unit to continuously cast its spammable abilities, if it can
        readonly MoveToUnit FollowBehaviour;          // forces the unit to chase enemy units in its vision range
        readonly ReturnOrder FreeReturnBehaviour;      // makes the unit return to the origin if nothing else is to be done

        public Unit CurrentTarget { get; private set; }

        public double ReturnRange { get; }


        public Stand(Unit u, double returnRange = DefaultReturnRange)
            : base(u)
        {
            ReturnRange = returnRange;
            ForceReturnBehaviour = new ReturnOrder(this, u.Position, ReturnRange);
            SpamBehaviour = new SpamBehaviour(this);
            FollowBehaviour = new MoveToUnit(u);
            FreeReturnBehaviour = new ReturnOrder(this, u.Position, 0.5);

            AddRange(new Order[]
            {
                ForceReturnBehaviour,
                SpamBehaviour,
                FollowBehaviour,
                FreeReturnBehaviour,
            });

            Owner.ObjectSeen += onEntitySeen;
            Owner.DamageReceived += onDamageReceived;

            ForceReturnBehaviour.OnReturnStarted += ReturnBehaviour_OnReturnStarted;
            ForceReturnBehaviour.OnReturnFinished += ReturnBehaviour_OnReturnFinished;
        }

        public override bool TakeControl()
        {

            //keep track of most aggressive targets
            var newTarget = updateTarget();
            if (newTarget != CurrentTarget)
            {
                SpamBehaviour.TargetUnit = newTarget;
                FollowBehaviour.Target = newTarget;
                if(newTarget != null)
                    FollowBehaviour.MinDistance = (float)(Owner.Scale + newTarget.Scale) / 2;

                CurrentTarget = newTarget;
            }

            //update return locations in peaceful times
            if (CurrentTarget == null
                && CurrentBehaviour == null
                && ReturnPosition.DistanceTo(Owner.Position) > 2 * FreeReturnBehaviour.MaxDistance)
            {
                ReturnPosition = Owner.Position;
            }


            return base.TakeControl();
        }

        public override void Update(int msElapsed)
        {

            base.Update(msElapsed);
        }

        private void ReturnBehaviour_OnReturnFinished()
        {
            //re-add all visible entities to the aggro table
            foreach (var o in Owner.VisibleEntities)
                onEntitySeen(o);

            //reset life?!
            Owner.Life = Owner.MaxLife;
        }

        private void ReturnBehaviour_OnReturnStarted()
        {
            //clear the aggro table
            aggroTable.Clear();
        }

        readonly List<Unit> toRemove = new List<Unit>();


        /// <summary>
        /// Re-targets the hero with the most aggro. 
        /// </summary>
        /// <returns></returns>
        Unit updateTarget()
        {
            if (!aggroTable.Any())
                return null;

            Unit maxAggroGuy = null;
            double maxAggro = double.MinValue;
            double maxAggroDist = double.MaxValue;

            foreach (var kvp in aggroTable)
            {
                var u = kvp.Key;
                var uAggro = kvp.Value;
                var d = Owner.Position.DistanceTo(u.Position);

                if (u.IsDead || d > ReturnRange)
                {
                    toRemove.Add(u);
                    continue;
                }

                if (uAggro < maxAggro)
                    continue;

                if (uAggro.Equals(maxAggro) && d > maxAggroDist)
                    continue;

                maxAggroGuy = u;
                maxAggro = uAggro;
                maxAggroDist = d;
            }

            // remove dead targets. 
            foreach (var u in toRemove)
                aggroTable.Remove(u);
            toRemove.Clear();

            return maxAggroGuy;
        }

        /// <summary>
        /// Updates the aggro table whenever damage is received. 
        /// </summary>
        /// <param name="args"></param>
        void onDamageReceived(UnitDamagedArgs args)
        {
            var damageSource = args.DamagingUnit;
            var dmgAmount = args.FinalDamage;

            double curAggro;
            if (aggroTable.TryGetValue(damageSource, out curAggro))
                curAggro = 0;

            aggroTable[damageSource] = curAggro + dmgAmount;
        }

        /// <summary>
        /// Adds to the aggro table units that come into range. 
        /// </summary>
        void onEntitySeen(Entity e)
        {
            var u = e as Unit;
            if (u == null)
                return;


            if (ForceReturnBehaviour.Returning)
                return;

            //add it to the aggro table if it's a hero
            if (u.Owner.IsEnemyOf(Owner))
                if(!aggroTable.ContainsKey(u))
                    aggroTable.Add(u, 0);
        }

        Vector ReturnPosition
        {
            get { return FreeReturnBehaviour.OriginPosition; }
            set
            {
                ForceReturnBehaviour.OriginPosition =
                FreeReturnBehaviour.OriginPosition = value;
            }
        }
    }
}
