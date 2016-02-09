using Engine.Entities;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;

namespace Engine.Systems.Range
{
    /// <summary>
    /// A constraint between an in-game location and all or one objects. 
    /// </summary>
    class PointRangeEvent : RangeEvent
    {
        /// <summary>
        /// The in-game location of this constraint. 
        /// </summary>
        public Vector Origin { get; }

        /// <summary>
        /// Creates a new constraint from the given in-game location raised whenever an object crosses it. 
        /// </summary>
        /// <param name="gameLocation">The in-game location. </param>
        /// <param name="range">The distance at which this constraint is triggered. </param>
        public PointRangeEvent(Vector gameLocation, double range)
            : base(range)
        {
            Origin = gameLocation;
        }
        /// <summary>
        /// Creates a new constraint from the given in-game location raised whenever the specified object crosses it. 
        /// </summary>
        /// <param name="gameLocation">The in-game location. </param>
        /// <param name="target">The object that triggers the event. </param>
        /// <param name="range">The distance at which this constraint is triggered. </param>
        public PointRangeEvent(Vector gameLocation, GameObject target, double range)
            : base(range, target)
        {
            Origin = gameLocation;
        }

        protected override bool DoCheck(GameObject target, double newDistSq, double oldDistSq)
        {
            return (newDistSq < RangeSquared)
                && (oldDistSq >= RangeSquared || double.IsNaN(oldDistSq));
        }
    }
}
