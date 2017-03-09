using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Buffs;
using Shanism.Engine.Objects.Range;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Abilities
{

    [Flags]
    public enum AuraTargetType
    {
        /// <summary>
        /// An aura that targets friendly units. 
        /// </summary>
        Friendly = 1 << 0,
        /// <summary>
        /// An aura that targets neutral units. 
        /// </summary>
        Neutral = 1 << 1,
        /// <summary>
        /// An aura that targets hostile (enemy) units. 
        /// </summary>
        Hostile = 1 << 2,


        /// <summary>
        /// An aura that targets non-friendly (i.e. neutral or hostile) units. 
        /// </summary>
        NonFriendly = Neutral | Hostile,

        /// <summary>
        /// An aura that targets non-hostile (i.e. neutral or friendly) units. 
        /// </summary>
        NonHostile = Friendly | Neutral,

        /// <summary>
        /// An aura that targets all units in its range. 
        /// </summary>
        All = Friendly | Neutral | Hostile,
    }

    public class Aura : Ability
    {
        public static readonly double DefaultRange = 10;


        readonly Dictionary<Unit, BuffInstance> infectedUnits = new Dictionary<Unit, BuffInstance>();

        double _auraRange;
        RangeEvent rangeEvent;

        /// <summary>
        /// Gets or sets the buff which is applied as the effect of the aura.
        /// </summary>
        public Buff AuraEffect { get; set; }

        /// <summary>
        /// Gets or sets the range of the aura.
        /// </summary>
        public double AuraRange
        {
            get { return _auraRange; }
            set
            {
                if (!_auraRange.Equals(value))
                {
                    if (Owner != null)
                    {
                        Owner.range.RemoveEvent(rangeEvent);
                        rangeEvent = new RangeEvent(value, null, somethingAround);
                        Owner.range.AddEvent(rangeEvent);
                    }
                    _auraRange = value;
                }

            }
        }

        /// <summary>
        /// Gets or sets the types of targets for this aura.
        /// </summary>
        public AuraTargetType AuraTargets { get; set; } = AuraTargetType.Friendly;

        static bool checkTargets(Unit a, Unit b, AuraTargetType tty)
        {
            var enemy = ((tty & AuraTargetType.Hostile) != 0) && a.Owner.IsEnemyOf(b);
            var friend = ((tty & AuraTargetType.Friendly) != 0) && !a.Owner.IsEnemyOf(b);
            var neutral = ((tty & AuraTargetType.Neutral) != 0) && !a.Owner.IsEnemyOf(b);

            return enemy || friend || neutral;
        }

        public Aura()
        {
            TargetType = AbilityTargetType.Passive;
            _auraRange = DefaultRange;
            rangeEvent = new RangeEvent(DefaultRange, null, somethingAround);
        }

        void somethingAround(Entity e, RangeEventTriggerType tty)
        {
            if (!(e is Unit))
                return;
            var u = (Unit)e;

            if (!checkTargets(Owner, u, AuraTargets))
                return;

            if (tty == RangeEventTriggerType.Enter)
            {
                BuffInstance buff;
                if (!infectedUnits.TryGetValue(u, out buff))
                {
                    buff = u.Buffs.Apply(Owner, AuraEffect);
                    infectedUnits.Add(u, buff);
                }
            }
            else
            {
                BuffInstance buff;
                if (infectedUnits.TryGetValue(u, out buff))
                {
                    u.Buffs.Remove(buff);
                    infectedUnits.Remove(u);
                }
            }
        }

        protected override void OnLearned()
        {
            Owner.range.AddEvent(rangeEvent);
            base.OnLearned();
        }

        protected override void OnUnlearned()
        {
            Owner.range.RemoveEvent(rangeEvent);
            base.OnUnlearned();
        }
    }
}
