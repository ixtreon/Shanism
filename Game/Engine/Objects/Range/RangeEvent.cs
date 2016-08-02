using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Engine.Objects;

namespace Shanism.Engine.Objects.Range
{
    public enum RangeEventTriggerType
    {
        Enter, Leave
    }


    public delegate void RangeEventCallback(Entity e, RangeEventTriggerType tty);

    public class RangeEvent : IComparable<RangeEvent>
    {

        internal readonly double RangeSquared;

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
        public event RangeEventCallback Triggered;


        /// <summary>
        /// Creates a range constraint that targets one or all entities and raises events whenever they cross a defined range boundary. 
        /// </summary>
        /// <param name="range">The distance at which this constraint is triggered. </param>
        /// <param name="target">The single entity that can trigger this constraint or null to track all entities. </param>
        /// <param name="eventHandler">The default handler for this event. </param>
        public RangeEvent(double range, Entity target = null, RangeEventCallback eventHandler = null)
        {
            if (range <= 0 || range > Constants.RangeEvents.MaxRangeUnits)
                throw new ArgumentOutOfRangeException($"Distance must be between 0 and {Constants.RangeEvents.MaxRangeUnits}");

            Range = range;
            RangeSquared = range * range;
            Target = target;

            Triggered += eventHandler;
        }

        /// <summary>
        /// Raises the <see cref="Triggered"/> event with the provided arguments. 
        /// </summary>
        internal void Raise(Entity target, RangeEventTriggerType tty)
        {
            if (Triggered != null
                && (Target == null || Target == target))
                Triggered(target, tty);
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
