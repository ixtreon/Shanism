using Shanism.Engine.Events;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using System.Collections.Concurrent;

namespace Shanism.Engine.Objects.Behaviours
{
    /// <summary>
    /// A compound aggro behaviour which keeps an aggro table for a unit 
    /// and switches between following and attacking the current target with max aggro. 
    /// 
    /// Returns the unit to the starting position if the main target runs too far away. 
    /// </summary>
    class AggroBehaviour : BehaviourList
    {
        public const int DefaultReturnRange = 20;


        readonly Dictionary<Unit, double> aggroTable = new Dictionary<Unit, double>();

        readonly ReturnBehaviour ForceReturnBehaviour;     // forces the unit to the origin if it leaves its aggro zone
        readonly SpamBehaviour SpamBehaviour;              // forces the unit to continuously cast its spammable abilities, if it can
        readonly FollowBehaviour FollowBehaviour;          // forces the unit to chase enemy units in its vision range
        readonly ReturnBehaviour FreeReturnBehaviour;      // makes the unit return to the origin if nothing else is to be done

        public Unit CurrentTarget { get; private set; }

        public double ReturnRange { get; }


        public AggroBehaviour(Unit u, double returnRange = DefaultReturnRange)
            : base(u)
        {
            ReturnRange = returnRange;
            ForceReturnBehaviour = new ReturnBehaviour(this, u.Position, ReturnRange);
            SpamBehaviour = new SpamBehaviour(this);
            FollowBehaviour = new FollowBehaviour(this);
            FreeReturnBehaviour = new ReturnBehaviour(this, u.Position, 0.5);

            AddRange(new Behaviour[]
            {
                ForceReturnBehaviour,
                SpamBehaviour,
                FollowBehaviour,
                FreeReturnBehaviour,
            });

            ForceReturnBehaviour.OnReturnStarted += ReturnBehaviour_OnReturnStarted;
            ForceReturnBehaviour.OnReturnFinished += ReturnBehaviour_OnReturnFinished;
        }

        private void ReturnBehaviour_OnReturnFinished()
        {

        }

        private void ReturnBehaviour_OnReturnStarted()
        {
            //clear the aggro table
            aggroTable.Clear();

            Owner.Life = Owner.MaxLife;
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
            double aggroDist = double.MaxValue;

            foreach (var kvp in aggroTable)
            {
                var u = kvp.Key;
                var d = Owner.Position.DistanceToSquared(u.Position);

                if (u.IsDead || d > ReturnRange)
                {
                    toRemove.Add(u);
                    continue;
                }

                if (kvp.Value < maxAggro)
                    continue;

                if (kvp.Value.Equals(maxAggro) && d > aggroDist)
                    continue;

                maxAggroGuy = u;
                maxAggro = kvp.Value;
                aggroDist = d;
            }

            // remove dead targets. 
            foreach (var u in toRemove)
                aggroTable.Remove(u);

            return maxAggroGuy;
        }

        /// <summary>
        /// Updates the aggro table whenever damage is received. 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnDamageReceived(UnitDamagedArgs args)
        {
            //add or update the damage source's entry in the table
            var damageSource = args.DamagingUnit;
            var dmgAmount = args.FinalDamage;

            double curAggro;
            if (aggroTable.TryGetValue(damageSource, out curAggro))
                aggroTable[damageSource] = curAggro + dmgAmount;
            else
                aggroTable[damageSource] = dmgAmount;
        }

        /// <summary>
        /// Adds to the aggro table units that come into range. 
        /// </summary>
        protected override void OnUnitInVisionRange(Unit unit)
        {
            if (ForceReturnBehaviour.Returning)
                return;

            //add it to the aggro table if it's a hero
            if (unit.Owner.IsEnemyOf(Owner))
                if(!aggroTable.ContainsKey(unit))
                    aggroTable.Add(unit, 0);
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

        public override void Update(int msElapsed)
        {
            FollowBehaviour.Distance = SpamBehaviour.Ability?.CastRange ?? 1;


            //keep track of most aggressive targets
            var newTarget = updateTarget();
            if (newTarget != CurrentTarget)
            {
                SpamBehaviour.TargetUnit = newTarget;
                FollowBehaviour.Target = newTarget;

                CurrentTarget = newTarget;
            }

            //update return locations in peaceful times
            if (CurrentTarget == null
                && CurrentBehaviour == null
                && ReturnPosition.DistanceTo(Owner.Position) > 2 * FreeReturnBehaviour.MaxDistance)
            {
                ForceReturnBehaviour.OriginPosition =
                FreeReturnBehaviour.OriginPosition = Owner.Position;
            }

            base.Update(msElapsed);
        }
    }
}
