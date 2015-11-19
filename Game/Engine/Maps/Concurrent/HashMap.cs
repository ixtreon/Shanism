using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO;
using IO.Common;
using Bin = IO.Common.Point;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections.Concurrent;

namespace Engine.Maps.Concurrent
{
    /// <summary>
    /// Maps objects in 2D space by dividing the space into equally-sized bins contained in a hash-table. 
    /// Allows for efficient concurrent range queries of limited size. 
    /// </summary>
    /// <typeparam name="T">The type of objects to track. </typeparam>
    [Obsolete]
    public class HashMap<T>
    {

         ConcurrentDictionary<Vector, ConcurrentDictionary<T, Vector>> hashTable 
            = new ConcurrentDictionary<Vector, ConcurrentDictionary<T, Vector>>();

        public readonly Vector CellSize;

        public int Count { get; private set; }

        public IEnumerable<T> Items
        {
            get { return hashTable.SelectMany(t => t.Value.Select(it => it.Key)); }
        }

        public HashMap(Vector cellSize)
        {
            this.CellSize = cellSize;
        }

        public void Add(T item, Vector position)
        {
            var binId = getBin(position);

            var bin = hashTable.GetOrAdd(binId, (v) => new ConcurrentDictionary<T, Vector>());

            if (bin.TryAdd(item, position))
                Count++;
        }

        public bool TryRemove(T item, Vector position)
        {
            //remove from the hashmap using the item's current key to locate it. 
            var binId = getBin(position);
            var removed = hashTable[binId].TryRemove(item, out position);
            if (removed)
                Count--;
            return removed;
        }

        /// <summary>
        /// Updates the recorded position of the given item in this HashMap. 
        /// !!! NOT THREAD SAFE WHILE IT SHOULD BE !!! 
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(T item, Vector oldPos, Vector newPos)
        {
            var oldId = getBin(oldPos);
            var newId = getBin(newPos);
            if(oldId == newId)
            {
                hashTable[newId][item] = newPos;
                return;
            }
            else
            {
                if(TryRemove(item, oldPos)) // should be made thread-safe
                    Add(item, newPos);
            }
        }

        /// <summary>
        /// Retrieves all units within the specified rectangle. 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<T> RangeQuery(Vector pos, Vector size)
        {
            var cellStart = getBin(pos);
            var cellEnd = getBin(pos + size);

            // get the distinct bin IDs of all bins inbetween (and including)
            // the start and end points, then get their entries in the table. 
            for(int ix = cellStart.X; ix <= cellEnd.X; ix++)
                for(int iy = cellStart.Y; iy <= cellEnd.Y; iy++)
                {
                    var binId = new Bin(ix, iy);
                    ConcurrentDictionary<T, Vector> bin;
                    if (hashTable.TryGetValue(binId, out bin))
                        foreach (var u in bin)
                            if (u.Value.Inside(pos, size))
                                yield return u.Key;
                }
        }

        /// <summary>
        /// Returns the bin of the given item, as determined by the bin size. 
        /// </summary>
        private Bin getBin(Vector pos)
        {
            return (pos / CellSize).Floor();
        }
    }

}
