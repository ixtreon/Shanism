using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Entities
{
    /// <summary>
    /// Provides static methods for the creation of unique identifiers for game objects. 
    /// </summary>
    static class ObjectGuid
    {
        static int guidCount = 0;

        /// <summary>
        /// Generates a new, unique id for the given type <typeparamref name="T"/>. 
        /// </summary>
        /// <returns>A new, nonnegative id. </returns>
        public static uint GetNew()
        {
            return (uint)Interlocked.Increment(ref guidCount);
        }
    }
}
