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
    [Obsolete]
    public class HashMap<T> : IEnumerable<T>
    {
        /// <summary>
        /// The main table of tables. 
        /// </summary>
        readonly Hashtable hashTable = new Hashtable();

        /// <summary>
        /// The span of a sub-table. 
        /// </summary>
        public readonly Vector CellSize;

        /// <summary>
        /// The total amount of objects in the HashMap
        /// </summary>
        public int Count { get; private set; }


        public IEnumerable<T> Items
        {
            get { return hashTable.Values.Cast<Hashtable>().SelectMany(bin => bin.Keys.Cast<T>().ToArray()).ToArray(); }
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
        public bool Remove(T item, Vector position)
        {
            //get bin
            var binId = getBin(position);
            var binTable = TryGet(binId);
            if (binTable == null)
                return false;

            //get item
            if (!binTable.ContainsKey(item))
                return false;

            //remove it
            binTable.Remove(item);
            Count--;
            return true;
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

            if (position.IsNan() || oldId == newId)
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
        /// <param name="pos">The lower left point of the rectangle. </param>
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
                if ((binTable = TryGet(b)) != null)
                    foreach (DictionaryEntry kvp in binTable)
                        if (((Vector)kvp.Value).Inside(pos, size))
                            yield return (T)kvp.Key;
            }
        }

        public IEnumerable<T> RawQuery(Vector pos, Vector size)
        {
            var cellStart = getBin(pos);
            var cellEnd = getBin(pos + size);
            var bins = SelectBins(cellStart, cellEnd);

            Hashtable binTable;
            foreach (var b in bins)
                if ((binTable = TryGet(b)) != null)
                    foreach (DictionaryEntry kvp in binTable)
                        yield return (T)kvp.Key;
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
