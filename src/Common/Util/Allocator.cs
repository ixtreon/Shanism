using System.Collections.Generic;
using System.Diagnostics;

#pragma warning disable RECS0108 // Warns about static fields in generic types

namespace Shanism.Common.Util
{
    /// <summary>
    /// Generates IDs of type <see cref="uint"/> unique to the supplied type. 
    /// </summary>
    public class Allocator
    {
        readonly List<uint> freeIds = new List<uint>();

        uint maxId;

        public void Reset()
        {
            freeIds.Clear();
            maxId = 0;
        }

        public uint New()
        {
            //if (freeIds.Count > 0)
            //    return freeIds.Pop();

            return maxId++;
        }

        public void Release(uint i)
        {
            Debug.Assert(maxId > i);
            Debug.Assert(!freeIds.Contains(i));

            freeIds.Add(i);
        }
    }
}
