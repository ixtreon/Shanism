using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// A simple counter that resets at a given max value. 
    /// Is not thread-safe. 
    /// </summary>
    public class Counter
    {
        /// <summary>
        /// The current value of the counter. Zero by default. 
        /// </summary>
        public int Value { get; private set; } = 0;

        /// <summary>
        /// Gets the max value of the counter. 
        /// </summary>
        public int MaxValue { get; private set; } 

        /// <summary>
        /// Creates a new counter with the given maximum value. 
        /// </summary>
        /// <param name="maxValue">The maximum value this counter can reach. </param>
        public Counter(int maxValue)
        {
            MaxValue = Math.Max(1, maxValue);
        }

        /// <summary>
        /// Increments the counter by one 
        /// and returns whether the max value was reached. 
        /// </summary>
        public bool Tick()
        {
            return Tick(1);
        }

        /// <summary>
        /// Increments the counter by the specified amount 
        /// and returns whether the max value was reached. 
        /// </summary>
        public bool Tick(int ticks)
        {
            Value += ticks;

            if (Value < MaxValue)
                return false;

            Value %= MaxValue;
            return true;
        }

        /// <summary>
        /// Resets the counter and optionally sets a new max value. 
        /// </summary>
        /// <param name="newMax">The new maximum this counter can reach. </param>
        public void Reset(int? newMax = null)
        {
            if (newMax < 0) throw new ArgumentException($"The value of '{nameof(newMax)}' ({newMax}) must be a non-negative integer!");

            Value = 0;
            MaxValue = Math.Max(1, newMax ?? MaxValue);
        }
    }
}
