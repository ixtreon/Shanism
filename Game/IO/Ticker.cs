using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// Provides for periodic events. 
    /// </summary>
    public class Ticker
    {
        public int Value { get; private set; } = 0;

        public int Period { get; private set; }

        public Ticker(int period)
        {
            Period = period;
        }

        /// <summary>
        /// Increments the ticker by one 
        /// and returns whether we ticked. 
        /// </summary>
        public bool Tick()
        {
            return Tick(1);
        }

        /// <summary>
        /// Increments the ticker by the specified amount 
        /// and returns whether we ticked. 
        /// </summary>
        public bool Tick(int ticks)
        {
            Value += ticks;

            if (Value < Period)
                return false;

            if(Period > 0)
                Value %= Period;
            return true;
        }

        public void Reset(int? newPeriod = null)
        {
            Value = 0;
            Period = newPeriod ?? Period;
        }
    }
}
