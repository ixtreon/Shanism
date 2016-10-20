using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    /// <summary>
    /// Provides a value between 0 and 1 that is continuous in time and changes at a rate defined by <see cref="Period"/>. 
    /// Must be updated every frame. The default ticker should be updated somewhere in the engine. 
    /// </summary>
    class Counter
    {
        public static Counter Default = new Counter();


        readonly Stopwatch ticker = Stopwatch.StartNew();

        /// <summary>
        /// The period of one tick/swing/direction in milliseconds. 2000 by default. 
        /// </summary>
        public readonly double Period;

        public Counter(int period = 2000)
        {
            Period = period;
        }

        public double GetValue() => (ticker.ElapsedMilliseconds % (Period * 2)) / Period;

        public double GetValue(double max) => GetValue() * max;

        public double GetValue(double min, double max) => min + GetValue(max - min);
    }
}
