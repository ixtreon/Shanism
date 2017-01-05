using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable RECS0108 // Warns about static fields in generic types

namespace Shanism.Common.Util
{
    /// <summary>
    /// Generates IDs of type <see cref="uint"/> unique to the supplied type. 
    /// </summary>
    public static class Allocator<T>
    {
        static readonly List<uint> freeIds = new List<uint>();

        static uint maxId = 0;

        public static uint Allocate()
        {
            //if (freeIds.Count > 0)
            //    return freeIds.Pop();

            return maxId++;
        }

        
        public static void Deallocate(uint i)
        {
            Debug.Assert(maxId > i);
            Debug.Assert(!freeIds.Contains(i));

            freeIds.Add(i);
        }
    }
}
