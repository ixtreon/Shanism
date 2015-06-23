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

namespace Engine.Maps
{
    /// <summary>
    /// Maps objects in 2D space by dividing the space into equally-sized bins contained in a hash-table. 
    /// Allows for efficient concurrent range queries of limited size. 
    /// </summary>
    /// <typeparam name="T">The type of objects to track. </typeparam>
    public class HashMap2<T>
    {
        readonly Dictionary<Vector, Dictionary<T, Vector>> hashTable = new Dictionary<Vector, Dictionary<T, Vector>>();

        public readonly Vector CellSize;

        public int Count { get; private set; }

        public IEnumerable<T> Items
        {
            get { return hashTable.SelectMany(t => t.Value.Select(it => it.Key)); }
        }

        public HashMap2(Vector cellSize)
        {
            this.CellSize = cellSize;
        }

        public void Add(T item, Vector position)
        {
            var binId = getBin(position);
            hashTable[binId].Add(item, position);
            Count++;
        }

        public bool Remove(T item, Vector position)
        {
            //remove from the hashmap using the item's current key to locate it. 
            var binId = getBin(position);
            var removed = hashTable[binId].Remove(item);

            Debug.Assert(removed);
            Count--;

            return removed;
        }

        /// <summary>
        /// Updates the recorded position of the given item in this HashMap. 
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(T item, Vector position, Vector newPos)
        {
            var oldId = getBin(position);
            var newId = getBin(newPos);
            if(oldId == newId)
            {
                hashTable[newId][item] = newPos;
                return;
            }
            else
            {
                hashTable[oldId].Remove(item);
                hashTable[newId].Add(item, newPos);
                if(Remove(item, position))
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

            return SelectBins(cellStart, cellEnd)
                .AsParallel()
                .SelectMany(b =>
                {
                    Dictionary<T, Vector> t;
                    if (hashTable.TryGetValue(b, out t))
                        return t.Where(u => u.Value.Inside(pos, size)).Select(u => u.Key);
                    else
                        return Enumerable.Empty<T>();
                });

        }

        private IEnumerable<Bin> SelectBins(Bin start, Bin end)
        {
            for (int ix = start.X; ix <= end.X; ix++)
                for (int iy = start.Y; iy <= end.Y; iy++)
                    yield return new Bin(ix, iy);
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
