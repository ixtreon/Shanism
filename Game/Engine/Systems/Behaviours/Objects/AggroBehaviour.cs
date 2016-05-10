using Shanism.Engine.Events;
using Shanism.Engine.Objects;
using Shanism.Engine.Objects.Entities;
using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using System.Collections.Concurrent;

namespace Shanism.Engine.Systems.Behaviours
{
    /// <summary>
    /// A compound aggro behaviour which keeps an aggro table for a unit 
    /// and switches between following and attacking the current target with max aggro. 
    /// 
    /// Returns the unit to the starting position if the main target runs too far away. 
    /// </summary>
    class AggroBehaviour : BehaviourList
    {

        ConcurrentDictionary<Unit, double> aggroTable = new ConcurrentDictionary<Unit, double>();

        protected ReturnBehaviour ForceReturnBehaviour;     // forces the unit to the origin if it leaves its aggro zone
        protected SpamBehaviour SpamBehaviour;              // forces the unit to continuously cast its spammable abilities, if it can
        protected FollowBehaviour FollowBehaviour;          // forces the unit to chase enemy units in its vision range
        protected ReturnBehaviour FreeReturnBehaviour;      // makes the unit return to the origin if nothing else is to be done

        public Unit CurrentTarget { get; internal set; }

        public AggroBehaviour(Unit u)
            : base(u)
        {
            ForceReturnBehaviour = new ReturnBehaviour(this, u.Position, u.VisionRange * 2);
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


        /// <summary>
        /// Re-targets the hero with the most aggro. 
        /// </summary>
        /// <returns></returns>
        Unit pickTarget()
        {
            if (!aggroTable.Any())
                return null;

            // remove dead or non-existing targets. 
            var toRemove = aggroTable.Where(kvp => kvp.Key == null || kvp.Key.IsDead);
            double outVal;
            foreach (var kvp in toRemove)
                aggroTable.TryRemove(kvp.Key, out outVal);

            // target the max-aggro guy
            var maxAggroGuy = aggroTable
                .ArgMaxList(kvp => kvp.Value)   // get unit(s) with the most aggro
                .Select(kvp => kvp.Key)
                .ArgMaxList(u => -u.Position.DistanceToSquared(Owner.Position)) //if more than 1 such unit, target the closest one
                .FirstOrDefault();

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

            aggroTable.AddOrUpdate(damageSource, dmgAmount, (_, aggro) => aggro + dmgAmount);
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
                aggroTable.TryAdd(unit, 0);
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

            //update return locations in peaceful times
            if (CurrentTarget == null 
                && CurrentBehaviour == null 
                && ReturnPosition.DistanceTo(Owner.Position) > 2 * FreeReturnBehaviour.MaxDistance)
            {
                ForceReturnBehaviour.OriginPosition =
                FreeReturnBehaviour.OriginPosition = Owner.Position;
            }

            //keep track of most aggressive targets
            var newTarget = pickTarget();
            if (newTarget != CurrentTarget)
            {
                SpamBehaviour.TargetUnit = newTarget;
                FollowBehaviour.Target = newTarget;

                CurrentTarget = newTarget;
            }

            base.Update(msElapsed);
        }
    }
}
