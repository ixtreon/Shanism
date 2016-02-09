using Engine.Events;
using Engine.Entities;
using Engine.Entities.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using IO;
using System.Collections.Concurrent;

namespace Engine.Systems.Behaviours
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

        protected FollowBehaviour FollowBehaviour;
        protected SpamBehaviour SpamBehaviour;
        protected ReturnBehaviour ReturnBehaviour;

        public Unit Target { get; internal set; }

        public AggroBehaviour(Unit u)
            : base(u)
        {
            FollowBehaviour = new FollowBehaviour(this);
            SpamBehaviour = new SpamBehaviour(this);
            ReturnBehaviour = new ReturnBehaviour(this, u.Position, u.VisionRange * 2);

            AddRange(new Behaviour[]
            {
                ReturnBehaviour,
                SpamBehaviour,
                FollowBehaviour,
            });

            ReturnBehaviour.OnReturnStarted += ReturnBehaviour_OnReturnStarted;
            ReturnBehaviour.OnReturnFinished += ReturnBehaviour_OnReturnFinished;
        }

        private void ReturnBehaviour_OnReturnFinished()
        {

        }

        private void ReturnBehaviour_OnReturnStarted()
        {
            //clear the aggro table
            aggroTable.Clear();
        }


        /// <summary>
        /// Re-targets the hero with the most aggro. 
        /// </summary>
        /// <returns></returns>
        Unit pickTarget()
        {
            // remove stale entries
            var toRemove = aggroTable.Where(kvp => kvp.Key == null || kvp.Key.IsDead);
            double outVal;
            foreach (var kvp in toRemove)
                aggroTable.TryRemove(kvp.Key, out outVal);

            if (!aggroTable.Any())
                return null;

            // target the max-aggro guy
            var maxAggroGuy = aggroTable
                .ArgMaxList(a => a.Value)
                .ArgMaxList(a => -a.Key.Position.DistanceToSquared(Owner.Position))
                .FirstOrDefault().Key;
            return maxAggroGuy;
        }

        /// <summary>
        /// Updates the aggro table whenever damage is received. 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnDamageReceived(UnitDamagedArgs args)
        {
            if (ReturnBehaviour.Returning)
                return;
            //get the source
            var damageSource = args.DamagingUnit;

            //update his aggro in the table
            double aggro = 0;
            aggroTable.TryGetValue(damageSource, out aggro);
            aggroTable[damageSource] = aggro + args.FinalDamage;
        }

        /// <summary>
        /// Adds to the aggro table units that come into range. 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnUnitInVisionRange(Unit unit)
        {
            if (ReturnBehaviour.Returning)
                return;


            //add it to the aggro table if it's a hero
            if (unit.Owner.IsEnemyOf(Owner) && !aggroTable.ContainsKey(unit))
                aggroTable[unit] = 0;
        }

        /// <summary>
        /// The behaviour takes control only if there's a valid enemy target around. 
        /// </summary>
        /// <param name="msElapsed"></param>
        /// <returns></returns>
        public override bool TakeControl(int msElapsed)
        {
            return pickTarget() != null;
        }

        public override void Update(int msElapsed)
        {
            FollowBehaviour.Distance = SpamBehaviour.Ability?.CastRange ?? 1;
            var newTarget = pickTarget();


            if (newTarget != Target)
            {
                //change target
                SpamBehaviour.TargetUnit = newTarget;
                FollowBehaviour.Target = newTarget;

                // if there was no target before, save where we currently are
                if (Target == null)
                    ReturnBehaviour.OriginPosition = Unit.Position;

                Target = newTarget;
            }

            base.Update(msElapsed);
        }
    }
}
