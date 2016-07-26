using Shanism.Engine.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Items
{
    /// <summary>
    /// The backpack of a unit. Contains a bunch of items. 
    /// </summary>
    class Backpack
    {
        readonly Item[] itemGrid;

        int _count;


        public Backpack(int maxSz, IEnumerable<Item> items)
            : this(maxSz)
        {
            foreach (var it in items?.Take(maxSz))
                if(it != null)
                    itemGrid[_count++] = it;
        }

        public Backpack(int maxSz)
        {
            itemGrid = new Item[maxSz];
            _count = 0;
        }


        public int Size => itemGrid.Length;

        public int Count => _count;

        public int FreeSlots => Size - Count;

        public IEnumerable<Item> Items => itemGrid.Where(it => it != null);
        public IReadOnlyCollection<Item> ItemGrid => itemGrid;

        public Item ItemAt(int slotId) => itemGrid[slotId];

        /// <summary>
        /// Places the specified item at the specified slot. 
        /// Removes any item in that slot and returns it. 
        /// </summary>
        public Item PlaceItem(int slotId, Item newItem)
        {
            var oldItem = itemGrid[slotId];
            if (oldItem != null)
                _count--;

            itemGrid[slotId] = newItem;
            if (newItem != null)
                _count++;

            return oldItem;
        }

        public bool Add(Item it)
        {
            if (it == null) return true;

            if (FreeSlots <= 0) return false;

            for (var i = 0; i < Size; i++)
                if (itemGrid[i] == null)
                {
                    itemGrid[i] = it;
                    _count++;
                    return true;
                }

            throw new Exception("free slots out of sync! :|");
        }

        public bool TryRemove(int id, out Item it)
        {
            if ((it = itemGrid[id]) == null)
                return false;

            itemGrid[id] = null;
            _count--;
            return true;
        }

        public bool Remove(Item it)
        {
            var id = find(it);
            if (id == null)
                return false;

            itemGrid[id.Value] = null;
            _count--;
            return true;
        }

        public void Swap(int slotIdA, int slotIdB)
        {
            var fstIt = itemGrid[slotIdA];
            var sndIt = itemGrid[slotIdB];

            itemGrid[slotIdA] = sndIt;
            itemGrid[slotIdB] = fstIt;
        }

        int? find(Item it)
        {
            for (var i = 0; i < Size; i++)
                if (itemGrid[i] == it)
                    return i;
            return null;
        }
    }
}
