using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;
using Bin = IO.Common.Point;

namespace Engine.Maps
{
    /// <summary>
    /// Maps objects in 2D space by dividing the space into equally-sized bins contained in a hash-table. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HashMap<T>
    {
        const int AvgItemsPerBin = 10;

        List<T>[] hashTable;

        readonly Func<T, Vector> fGetItemPosition;

        readonly Vector CellSize;

        readonly Dictionary<T, Vector> currentPos = new Dictionary<T, Vector>();

        public IEnumerable<T> Items
        {
            get { return hashTable.SelectMany(x => x); }
        }

        public int TableSize
        {
            get { return hashTable.Length; }
        }

        public HashMap(Func<T, Vector> extractFunc, Vector cellSize, int tableSize = 10)
        {
            this.fGetItemPosition = extractFunc;
            this.CellSize = cellSize;

            resizeTable(tableSize);
        }

        private void resizeTable(int newSize)
        {
            // init the new table
            var newTable = new List<T>[newSize];

            for (int i = 0; i < newSize; i++)
                newTable[i] = new List<T>();

            // move all items from the previous table
            if (hashTable != null)  // (if any)
            {
                foreach (var it in Items)
                {
                    var pos = currentPos[it];
                    var id = getId(pos, newSize);
                    newTable[id].Add(it);
                }
            }

            //set the new table. 
            this.hashTable = newTable;
        }

        /// <summary>
        /// Returns the bin Id of the given item, as determined by the bin size. 
        /// </summary>
        private Bin getBin(Vector pos)
        {
            return (pos / CellSize).ToPoint();
        }

        /// <summary>
        /// Gets the bin Id of the given cell in a table with a specified size (bins). 
        /// </summary>
        private int getBinId(Bin b, int tableSize)
        {
            return ((b.X.GetHashCode() | b.Y.GetHashCode()) % tableSize + tableSize) % tableSize;
        }

        /// <summary>
        /// Gets the table entry (id) of the given vector in the current table. 
        /// </summary>
        private int getId(Vector pos)
        {
            return getId(pos, TableSize);
        }

        /// <summary>
        /// Gets the table entry (id) of the given vector in a table of size tableSize. 
        /// </summary>
        private int getId(Vector pos, int tableSize)
        {
            var binId = getBin(pos);
            var binHash = getBinId(binId, tableSize);
            return binHash;
        }

        public void Add(T item)
        {
            if (currentPos.ContainsKey(item))
                throw new ArgumentException("Cannot add an object which is already on the map. ");

            //add to the hash table. 
            var itPos = fGetItemPosition(item);
            var binId = getId(itPos);
            hashTable[binId].Add(item);

            //add to the sanity dictionary. 
            currentPos.Add(item, itPos);

            //resize the hashtable, if necessary
            if (currentPos.Count > AvgItemsPerBin * TableSize)
                resizeTable(TableSize * 2);
        }

        public void Remove(T item)
        {
            if (!currentPos.ContainsKey(item))
                throw new ArgumentException("Cannot remove an object which is not on the map. ");

            //remove from the hashmap using the item's current key to locate it. 
            var itPos = currentPos[item];
            var binId = getId(itPos);
            var removed = hashTable[binId].Remove(item);
            Debug.Assert(removed);

            //remove from the sanity dictionary
            currentPos.Remove(item);
           
            //size back?
        }


        public bool Contains(T item)
        {
            return currentPos.ContainsKey(item);
        }

        /// <summary>
        /// Updates the recorded position of the given item in this HashMap. 
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(T item)
        {
            if (!currentPos.ContainsKey(item))
                throw new ArgumentException("Cannot update an item which is not on the map. ");

            //get the old position of the item using the sanity dictionary. 
            var oldPos = currentPos[item];
            var oldId = getId(oldPos);
            
            //get the new position of the item
            var newPos = fGetItemPosition(item);
            var newId = getId(newPos);

            //update position in the sanity dictionary. 
            currentPos[item] = newPos;

            //continue only if we need to change the entry for this item. 
            if (oldId == newId)
                return;

            //add n remove entry
            var removed = hashTable[oldId].Remove(item);
            Debug.Assert(removed);
            hashTable[newId].Add(item);
        }

        public IEnumerable<T> RangeQuery(Vector pos, Vector size)
        {
            var cellStart = getBin(pos);
            var cellEnd = getBin(pos + size);

            // get the distinct bin IDs of all bins inbetween (and including)
            // the start and end points, then get their entries in the table. 
            var bins = cellStart.IterateToInclusive(cellEnd)
                .Select(p => getBinId(p, TableSize))            
                .Distinct()
                .Select(binId => hashTable[binId]);

            // go thru the collected bin entries 
            // and check whether each unit is inside the rect
            return bins.SelectMany(bin => bin.Where(u => currentPos[u].Inside(pos, size)));
        }
    }
}
