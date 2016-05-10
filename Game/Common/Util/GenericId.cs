using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Generates unsigned IDs unique to the supplied generic type. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class GenericId<T>
    {
        static int guidCount = 0;

        /// <summary>
        /// Generates a new, unique id for the current type <typeparamref name="T"/>. 
        /// </summary>
        /// <returns>A new, nonnegative id. </returns>
        public static uint GetNew()
        {
            return (uint)Interlocked.Increment(ref guidCount);
        }
    }
}
