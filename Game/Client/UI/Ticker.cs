using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    /// <summary>
    /// Provides a value between 0 and 1 that is continuous in time and changes at a rate defined by <see cref="Period"/>. 
    /// Must be updated every frame. The default ticker should be updated somewhere in the engine. 
    /// </summary>
    class Ticker
    {
        public static Ticker Default = new Ticker();

        //a value between 0 and 2. 
        double _rawValue;

        /// <summary>
        /// The period of one tick/swing/direction in milliseconds. 2000 by default. 
        /// </summary>
        public int Period { get; set; } = 2000;

        /// <summary>
        /// Gets a value between 0 and 1 that is continuous in time. 
        /// </summary>
        public double Value { get { return Math.Abs(1 - _rawValue); } }


        public void Update(int msElapsed)
        {
            _rawValue += 2.0 * msElapsed / Period;
            _rawValue %= 2;
        }

        public double GetValue(double min, double max)
        {
            return min + Value * (max - min);
        }
    }
}
