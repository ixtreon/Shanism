﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#pragma warning disable RECS0108 // Warns about static fields in generic types

namespace Shanism.Common.Util
{
    /// <summary>
    /// Generates IDs of type <see cref="uint"/> unique to the supplied type. 
    /// </summary>
    public static class GenericId<T>
    {
        /// <summary>
        /// The GUID that will not be assigned to any object. 
        /// </summary>
        public const int None = 0;

        static int guidCount;

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
