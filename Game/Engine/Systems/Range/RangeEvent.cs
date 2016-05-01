using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Engine.Objects;

namespace Engine.Systems.Range
{
    public class RangeEvent : IComparable<RangeEvent>
    {

        readonly double RangeSquared;

        /// <summary>
        /// The range at which this constraint is triggered. 
        /// </summary>
        public double Range { get; }


        /// <summary>
        /// Gets the single entity that triggers this constraint, or null if any entity can trigger it. 
        /// </summary>
        public Entity Target { get; }


        /// <summary>
        /// Gets whether this constraint can be triggered only by a single entity. 
        /// </summary>
        public bool HasTarget => (Target != null);


        /// <summary>
        /// The event raised whenever an entity crosses the range defined in this range boundary. 
        /// </summary>
        public event Action<Entity> Triggered;


        /// <summary>
        /// Creates a constraint that targets all entities. 
        /// </summary>
        /// <param name="range">The distance at which this cosntraint is triggered. </param>
        public RangeEvent(double range)
        {
            if (range <= 0 || range > Constants.RangeEvents.MaxRangeUnits)
                throw new ArgumentOutOfRangeException("Distance must be between 0 and {0}".F(Constants.RangeEvents.MaxRangeUnits));

            Range = range;
            RangeSquared = range * range;
        }

        /// <summary>
        /// Creates a constraint that targets a specific entity. 
        /// </summary>
        /// <param name="range">The distance at which this cosntraint is triggered. </param>
        /// <param name="target">The single entity that can trigger this constraint. </param>
        public RangeEvent(double range, Entity target)
            : this(range)
        {
            Target = target;
        }


        internal bool Check(Entity target, double newDistSq, double oldDistSq)
        {
            if ((Target == null || Target == target)
                && (newDistSq < RangeSquared)
                && (oldDistSq >= RangeSquared || double.IsNaN(oldDistSq)))
            {
                Triggered?.Invoke(target);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(RangeEvent other)
        {
            if(Target == other.Target)
                return Range.CompareTo(other.Range);
            return (Target?.Id ?? 0).CompareTo(other.Target?.Id ?? 0);
        }
    }
}
