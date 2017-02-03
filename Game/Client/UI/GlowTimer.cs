using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    /// <summary>
    /// Provides a value between 0 and 1 that is continuous in time (i.e. "glows") 
    /// and changes at a rate defined by its <see cref="Period"/>. 
    /// </summary>
    class GlowTimer
    {
        /// <summary>
        /// Gets the default timer with a period of 2000 ms. 
        /// </summary>
        public static GlowTimer Default { get; } = new GlowTimer(2000);

        static readonly Stopwatch ticker = Stopwatch.StartNew();


        readonly long start;

        /// <summary>
        /// The period of one tick/swing/direction in milliseconds. 
        /// </summary>
        public int Period { get; }


        public GlowTimer(int period)
        {
            Period = period;
            start = ticker.ElapsedMilliseconds;
        }


        int doublePeriod => Period << 1;

        long timeSinceStart => (ticker.ElapsedMilliseconds - start);

        long timeSinceRawZero => (timeSinceStart % doublePeriod);


        public double GetValue() => Math.Abs(Period - timeSinceRawZero) / Period;

        public double GetValue(double max) => GetValue() * max;

        public double GetValue(double min, double max) => min + GetValue(max - min);
    }
}
