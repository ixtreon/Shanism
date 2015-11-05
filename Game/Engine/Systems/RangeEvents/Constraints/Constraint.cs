using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Engine.Objects;

namespace Engine.Systems.RangeEvents
{
    public abstract class Constraint : IComparable<Constraint>
    {
        protected double RangeSquared { get; }

        internal int LastChecked { get; private set; }

        /// <summary>
        /// The range at which this constraint is triggered. 
        /// </summary>
        public double Range { get; }


        /// <summary>
        /// Gets the object that triggers this constraint, or null if any unit can trigger it. 
        /// </summary>
        public GameObject Target { get; }

        /// <summary>
        /// Gets whether this constraint can be triggered only by a given object. 
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
        public Constraint(double range)
        {
            if (range < 0 || range > Constants.RangeConstraints.MaxRange)
                throw new ArgumentOutOfRangeException("Distance must be between 0 and {0}".F(Constants.RangeConstraints.MaxRange));
            Range = range;
            RangeSquared = range * range;
        }

        /// <summary>
        /// Creates a constraint that targets a specific game object. 
        /// </summary>
        /// <param name="range">The distance at which this cosntraint is triggered. </param>
        /// <param name="target">The object that can trigger this constraint. </param>
        public Constraint(double range, GameObject target)
            : this(range)
        {
            Target = target;
        }

        internal bool Check(GameObject target, int currentFrame)
        {
            if (LastChecked >= currentFrame)
                return false;
            LastChecked = currentFrame;

            if (Target != target)
                return false;

            var isRaised = DoCheck(target);

            if (isRaised)
                Triggered?.Invoke(target);

            return isRaised;
        }

        protected abstract bool DoCheck(GameObject target);

        public int CompareTo(Constraint other)
        {
            return Range.CompareTo(other.Range);
        }
    }
}
