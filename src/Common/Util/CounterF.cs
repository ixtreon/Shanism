using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Common
{
    /// <summary>
    /// A simple counter that resets at a given max value. 
    /// This class is not thread-safe. 
    /// </summary>
    public class CounterF
    {
        /// <summary>
        /// The current value of the counter. Zero by default. 
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// Gets the max value of the counter. 
        /// </summary>
        public float MaxValue { get; private set; }

        /// <summary>
        /// Creates a new counter with the given maximum value. 
        /// </summary>
        /// <param name="maxValue">The maximum value this counter can reach. </param>
        public CounterF(float maxValue)
        {
            Value = 0;
            MaxValue = Math.Max(1, maxValue);
        }

        /// <summary>
        /// Increments the counter by one 
        /// and returns whether the max value was reached. 
        /// </summary>
        public bool Tick() => Tick(1);

        /// <summary>
        /// Increments the counter by the specified amount 
        /// and returns whether the max value was reached. 
        /// </summary>
        public bool Tick(float ticks)
        {
            Value += ticks;
            var hasTicked = Value >= MaxValue;
            Value %= MaxValue;
            return hasTicked;
        }

        /// <summary>
        /// Resets the counter and optionally sets a new max value. 
        /// </summary>
        /// <param name="newMax">The new maximum this counter can reach. </param>
        public void Reset(float? newMax = null)
        {
            if (newMax < 0)
                throw new ArgumentException($"The value of '{nameof(newMax)}' ({newMax}) must be a positive integer!");

            Value = 0;
            MaxValue = newMax ?? MaxValue;
        }

        public static implicit operator float(CounterF c) => c.Value;
    }
}
