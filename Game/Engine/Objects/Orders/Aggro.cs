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
    /// Switches between following and attacking the current target with max aggro, until it dies.
    /// </summary>
    class Aggro : OrderList
    {
        public const int DefaultReturnRange = 20;


        readonly Dictionary<Unit, double> aggroTable = new Dictionary<Unit, double>();

        readonly SpamAbilities SpamBehaviour;              // forces the unit to continuously cast its spammable abilities, if it can
        readonly MoveToUnit FollowBehaviour;          // forces the unit to chase enemy units in its vision range

        public Unit CurrentTarget { get; private set; }


        public Aggro(Unit u)
            : base(u)
        {
            SpamBehaviour = new SpamAbilities(this);
            FollowBehaviour = new MoveToUnit(u);

            AddRange(new Order[]
            {
                SpamBehaviour,
                FollowBehaviour,
            });

            Owner.ObjectSeen += onEntitySeen;
            Owner.DamageReceived += onDamageReceived;

        }

        public override bool TakeControl()
        {
            //keep track of most aggressive targets
            var newTarget = updateTarget();
            if (newTarget != CurrentTarget)
            {
                SpamBehaviour.TargetUnit = newTarget;
                FollowBehaviour.Target = newTarget;
                if (newTarget != null)
                    FollowBehaviour.MinDistance = (float)(Owner.Scale + newTarget.Scale) / 2;

                CurrentTarget = newTarget;
            }

            return base.TakeControl();
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        readonly List<Unit> toRemove = new List<Unit>();

        /// <summary>
        /// Clears this unit's aggro table.
        /// </summary>
        public void ClearTable()
            => aggroTable.Clear();

        /// <summary>
        /// Re-adds all visible enemies to the aggro table.
        /// </summary>
        public void ResetTable()
        {
            aggroTable.Clear();
            foreach (var e in Owner.VisibleEntities)
                onEntitySeen(e);
        }

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

                if (u.IsDead)
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


            //add it to the aggro table if it's a hero
            if (u.Owner.IsEnemyOf(Owner))
                if (!aggroTable.ContainsKey(u))
                    aggroTable.Add(u, 0);
        }
    }
}
