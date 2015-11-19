using Engine.Events;
using Engine.Objects;
using Engine.Objects.Game;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems.Behaviours
{
    /// <summary>
    /// A compound aggro behaviour which keeps the aggro table of a unit and switches between following and attacking, 
    /// 
    /// Returns the unit to the starting position if the target runs too far away. 
    /// 
    /// </summary>
    class AggroBehaviour : BehaviourList
    {

        Dictionary<Unit, double> aggroTable = new Dictionary<Unit, double>();

        protected SpamBehaviour AttackBehaviour;
        protected FollowBehaviour FollowBehaviour;
        protected ReturnBehaviour ReturnBehaviour;


        public Unit Target { get; internal set; }

        public AggroBehaviour(Unit u)
            : base(u)
        {
            FollowBehaviour = new FollowBehaviour(this);
            AttackBehaviour = new SpamBehaviour(this);
            ReturnBehaviour = new ReturnBehaviour(this, u.Position, u.VisionRange * 2);

            this.AddRange(new Behaviour[]
            {
                ReturnBehaviour,
                AttackBehaviour,
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
        private Unit pickTarget()
        {
            // remove stale entries
            aggroTable = aggroTable
                .Where(kvp => kvp.Key != null && !kvp.Key.IsDead)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (!aggroTable.Any())
                return null;

            // target the max-aggro guy
            var maxAggroGuy = aggroTable.Aggregate((a, b) => (a.Value > b.Value) ? a : b).Key;
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

            var hero = unit as Hero;

            //add it to the aggro table if it's a hero
            if (hero != null && !aggroTable.ContainsKey(hero))
                aggroTable[hero] = 0;
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
            FollowBehaviour.Distance = AttackBehaviour.Ability?.CastRange ?? 1;
            var newTarget = pickTarget();


            if (newTarget != Target)
            {
                //change target
                AttackBehaviour.TargetUnit = newTarget;
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
