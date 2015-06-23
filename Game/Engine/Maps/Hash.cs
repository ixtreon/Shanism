using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    /// <summary>
    /// Contains extension methods for hashing integers and object types. 
    /// </summary>
    public static class Hash
    {

        #region Hash ints and objects to ints
        /// <summary>
        /// Hashes the given integer, returning another positive integer. 
        /// </summary>
        /// <param name="i">The integer to hash</param>
        /// <returns></returns>
        public static int GetInt(this int i)
        {
            int c2 = 0x27d4eb2d; // a prime or an odd constant
            i = (i ^ 61) ^ (i >> 16);
            i = i + (i << 3);
            i = i ^ (i >> 4);
            i = i * c2;
            i = i ^ (i >> 15);
            return i;
        }

        public static int TurnToRange(this int i, int range)
        {
            return (int)(i.GetDouble() * range);
        }

        public static int GetInt(this int i, int j)
        {
            return (i.GetInt() ^ j.GetInt()).GetInt();
        }

        /// <summary>
        /// Hashes the given sequence of objects and returns a positive integer
        /// </summary>
        /// <param name="o">The sequence of objects to hash</param>
        /// <returns></returns>
        public static int GetInt(this object o, params object[] os)
        {
            return os.Aggregate(o.GetHashCode().GetInt(), (prev, nxt) => (prev ^ nxt.GetHashCode().GetInt()).GetInt());
        }
        #endregion

        #region Convert ints and objects to double
        /// <summary>
        /// Hashes the given integer, returning a double between 0 and 1
        /// </summary>
        /// <param name="i">The integer to hash</param>
        /// <returns></returns>
        public static double GetDouble(this int i)
        {
            return (double)GetInt(i) / int.MaxValue;
        }

        /// <summary>
        /// Hashes the given sequence of objects and returns a double value between 0 and 1
        /// </summary>
        /// <param name="o">The sequence of objects to hash</param>
        /// <returns></returns>
        public static double GetDouble(params object[] o)
        {
            return Hash.GetInt(o).GetDouble();
        }
        #endregion

        
    }
}
