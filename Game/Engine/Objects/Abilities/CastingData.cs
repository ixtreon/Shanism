using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Abilities
{
    class CastingData
    {
        public readonly Ability Ability;
        public readonly object Target;

        public bool IsGroundTarget => Target is Vector;
        public bool IsEntityTarget => Target is Entity;

        int progress = 0;


        public CastingData(Ability ab, Vector target)
        {
            Ability = ab;
            Target = target;
        }

        public CastingData(Ability ab, Entity target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            Ability = ab;
            Target = target;
        }

        public int Progress => progress;

        public Vector TargetLocation => (Target as Entity)?.Position ?? (Vector)Target;

        /// <summary>
        /// Advances the progress of the spell and returns true if casting is finished.
        /// </summary>
        internal bool UpdateProgress(int msElapsed)
            => Interlocked.Add(ref progress, msElapsed) > Ability.CastTime;


        public override bool Equals(object obj)
            => (obj is CastingData)
            && ((CastingData)obj).Target.Equals(Target)
            && ((CastingData)obj).Ability.Equals(Ability);
    }
}
