using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Engine.Entities;

namespace Engine.Systems.Range
{
    public abstract class RangeEvent : IComparable<RangeEvent>
    {

        readonly Dictionary<GameObject, double> targetDistances = new Dictionary<GameObject, double>();


        protected double RangeSquared { get; }

        /// <summary>
        /// The range at which this constraint is triggered. 
        /// </summary>
        public double Range { get; }


        /// <summary>
        /// Gets the object that triggers this constraint, or null if any unit can trigger it. 
        /// </summary>
        public GameObject Target { get; }

        /// <summary>
        /// Gets whether this constraint can be triggered only by a single object. 
        /// </summary>
        public bool HasTarget
        {
            get { return Target != null; }
        }

        public event Action<GameObject> Triggered;

        /// <summary>
        /// Creates a constraint that targets all game objects. 
        /// </summary>
        /// <param name="range">The distance at which this cosntraint is triggered. </param>
        public RangeEvent(double range)
        {
            if (range < 0 || range > Constants.RangeEvents.MaxRangeUnits)
                throw new ArgumentOutOfRangeException("Distance must be between 0 and {0}".F(Constants.RangeEvents.MaxRangeUnits));
            Range = range;
            RangeSquared = range * range;
        }

        /// <summary>
        /// Creates a constraint that targets a specific game object. 
        /// </summary>
        /// <param name="range">The distance at which this cosntraint is triggered. </param>
        /// <param name="target">The object that can trigger this constraint. </param>
        public RangeEvent(double range, GameObject target)
            : this(range)
        {
            Target = target;
        }

        internal bool Check(GameObject target, double distSquared)
        {

            //check frame
            if(Target != null && Target != target)
                return false;

            //get old, set new distance
            var oldDistSquared = targetDistances.TryGetVal(target) ?? double.NaN;
            targetDistances[target] = distSquared;

            //do the check, invoke event if necessary
            var isRaised = DoCheck(target, distSquared, oldDistSquared);
            if (isRaised)
                Triggered?.Invoke(target);

            return isRaised;
        }

        protected abstract bool DoCheck(GameObject target, double newDistSq, double oldDistSq);

        public int CompareTo(RangeEvent other)
        {
            return Range.CompareTo(other.Range);
        }
    }
}
