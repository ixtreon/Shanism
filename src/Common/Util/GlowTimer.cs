using System;
using System.Diagnostics;

namespace Shanism.Common
{
    /// <summary>
    /// Provides a value between 0 and 1 that is continuous in time (i.e. "glows") 
    /// and changes at a rate defined by its <see cref="Period"/>. 
    /// </summary>
    public class GlowTimer
    {

        static long Timestamp() => Stopwatch.GetTimestamp() * 1000 / Stopwatch.Frequency;


        /// <summary>
        /// The period of one tick/swing/direction in milliseconds. 
        /// </summary>
        public int Period { get; }

        /// <summary>
        /// Creates a glow timer with a given period, in milliseconds, per direction.
        /// </summary>
        public GlowTimer(int period)
        {
            Period = period;
        }

        /// <summary>
        /// Creates a glow timer with a given period.
        /// </summary>
        public GlowTimer(TimeSpan period)
        {
            Period = (int)period.TotalMilliseconds;
        }

        long RawValue => (Timestamp() % (Period * 2));

        public double GetValue() => (double)Math.Abs(Period - RawValue) / Period;

        public double GetValue(double max) => GetValue() * max;

        public double GetValue(double min, double max) => min + GetValue(max - min);
    }
}
