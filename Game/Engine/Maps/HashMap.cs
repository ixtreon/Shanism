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
using System.Collections;

namespace Engine.Maps
{
    /// <summary>
    /// Maps objects in 2D space by dividing the space into equally-sized bins contained in a hash-table. 
    /// Allows efficient concurrent range queries of a pre-specified size. 
    /// </summary>
    /// <typeparam name="T">The type of objects to track. </typeparam>
    public class HashMap<T> : IEnumerable<T>
    {
        readonly Hashtable hashTable = new Hashtable();

        public readonly Vector CellSize;

        public int Count { get; private set; }

        public IEnumerable<Hashtable> Tables
        {
            get { return hashTable.Values.Cast<Hashtable>(); }
        }

        public IEnumerable<T> Items
        {
            get { return Tables.SelectMany(bin => bin.Keys.Cast<T>().ToArray()).ToArray(); }
        }

        public HashMap(Vector cellSize)
        {
            this.CellSize = cellSize;
        }

        /// <summary>
        /// A non thread-safe Add operation. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="position"></param>
        public void Add(T item, Vector position)
        {
            var binId = getBin(position);

            Hashtable binTable = GetOrAdd(binId);

            binTable[item] = position;
            Count++;
        }

        /// <summary>
        /// A non thread-safe Remove operation. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="position"></param>
        public void Remove(T item, Vector position)
        {
            //remove from the hashmap using the item's current key to locate it. 
            var binId = getBin(position);

            if (!hashTable.ContainsKey(binId))
                throw new Exception("Provided bin {0} for GameObject {1} does not exist in the table!".Format(binId, item));
            var binTable = Get(binId);

            if(!binTable.ContainsKey(item))
            {
                var kur = this.Contains(item);
                var hui = this.hashTable.Values.SelectRaw(o => (Hashtable)o).FirstOrDefault(t => t.ContainsKey(item));
                return;
            }
            binTable.Remove(item);

            Count--;
        }

        /// <summary>
        /// Updates the recorded position of the given item in this HashMap. 
        /// Not thread-safe. Or maybe it is?
        /// </summary>
        /// <param name="item"></param>
        public void Update(T item, Vector position, Vector newPos)
        {
            if (position == newPos)
                return;

            var oldId = getBin(position);
            var newId = getBin(newPos);

            if(position.IsNan() || oldId == newId)
            {
                var ht = Get(newId);
                //lock (ht)
                    ht[item] = newPos;
            }
            else
            {
                Get(oldId).Remove(item);

                Hashtable ht = GetOrAdd(newId);
                //lock(ht)
                    ht[item] = newPos;
            }
        }

        /// <summary>
        /// Retrieves all units within the specified rectangle. 
        /// Thread-safe. 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IEnumerable<T> RangeQuery(Vector pos, Vector size)
        {
            var cellStart = getBin(pos);
            var cellEnd = getBin(pos + size);

            var bins = SelectBins(cellStart, cellEnd);

            Hashtable binTable;
            foreach (var b in bins)
            {
                binTable = TryGet(b);
                if(binTable != null)
                    foreach(DictionaryEntry kvp in binTable)
                        if (((Vector)kvp.Value).Inside(pos, size))
                            yield return (T)kvp.Key;
            }
        }

        Hashtable Get(Bin id)
        {
            return (Hashtable)hashTable[id];
        }

        Hashtable GetOrAdd(Bin id)
        {
            if (hashTable.ContainsKey(id))
                return Get(id);

            Hashtable ht = new Hashtable();
            hashTable[id] = ht;
            return ht;
        }

        Hashtable TryGet(Bin id)
        {
            if (!hashTable.ContainsKey(id))
                return null;
            return (Hashtable)hashTable[id];
        }

        IEnumerable<Bin> SelectBins(Bin start, Bin end)
        {
            for (int ix = start.X; ix <= end.X; ix++)
                for (int iy = start.Y; iy <= end.Y; iy++)
                    yield return new Bin(ix, iy);
        }

        /// <summary>
        /// Returns the bin of the given item, as determined by the bin size. 
        /// </summary>
        Bin getBin(Vector pos)
        {
            return (pos / CellSize).Floor();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
