using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Engine.Events;
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
        public readonly AbilityTargetType TargetType;
        public readonly Entity TargetEntity;
        public readonly Vector TargetLocation;

        int progress = 0;
        public int Progress => progress;


        public CastingData(Ability ab, Vector target)
        {
            Ability = ab;
            TargetType = AbilityTargetType.PointTarget;
            TargetEntity = null;
            TargetLocation = target;
        }

        public CastingData(Ability ab, Entity target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            Ability = ab;
            TargetType = AbilityTargetType.UnitTarget;
            TargetEntity = target;
            TargetLocation = target.Position;
        }


        /// <summary>
        /// Advances the progress of the spell and returns true if casting is finished.
        /// </summary>
        internal bool UpdateProgress(int msElapsed)
            => Interlocked.Add(ref progress, msElapsed) > Ability.CastTime;


        public override bool Equals(object obj)
        {
            var other = obj as CastingData;
            if (other == null)
                return false;

            return Ability.Equals(other.Ability)
                && TargetLocation.Equals(other.TargetLocation)
                && TargetEntity == other.TargetEntity;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Ability.GetHashCode();
                hash = hash * 23 + TargetLocation.GetHashCode();
                hash = hash * 23 + TargetEntity?.GetHashCode() ?? 0;
                return hash;
            }
        }

        public bool Invoke() => Ability.Invoke(this);
    }
}
