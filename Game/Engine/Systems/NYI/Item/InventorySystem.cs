using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Common;
using IO.Common;
using Engine.Objects;
using Engine.Objects.Entities;
using IO;

namespace Engine.Systems.Item
{
    //NYI
    class InventorySystem : UnitSystem
    {
        readonly object _lock = new object();

        const int BackpackWidth = 5;
        const int BackpackHeight = 4;
        const int BackpackSize = BackpackWidth * BackpackHeight;


        readonly InventoryType Type;

        readonly InventoryItem[] BackpackItems;

        readonly Dictionary<InventorySlots, InventoryItem> EquippedItems;


        int backpackItemCount;  //numberOfItemsInBackpack


        public InventorySystem(InventoryType invType)
        {
            Type = invType;

            BackpackItems = new InventoryItem[invType.BackpackSlots];
            EquippedItems = new Dictionary<InventorySlots, InventoryItem>();
        }

        public bool TryPickupItem(GameItem i)
        {
            lock (_lock)
            {
                //continue only if there is space in the backpack. 
                if (backpackItemCount == BackpackSize)
                    return false;

                var freeSlotId = 0;
                while (freeSlotId < BackpackItems.Length && BackpackItems[freeSlotId] != null)
                    freeSlotId++;

                if (freeSlotId == BackpackItems.Length)  //no free slot, can't pick it up
                    return false;

                BackpackItems[freeSlotId] = i.Item;
                backpackItemCount++;
                return true;
            }
        }
        public bool TryEquipItem(int fromBackpackSlot, InventorySlots slot)
        {
            lock (_lock)
            {
                //get the item to be moved in the slot
                var newItem = BackpackItems[fromBackpackSlot];

                if (newItem == null || newItem.Slot != slot)
                    return false;

                //get the item that was in the slot
                var oldItem = EquippedItems.TryGet(slot);

                //swap the items
                EquippedItems[slot] = BackpackItems[fromBackpackSlot];
                BackpackItems[fromBackpackSlot] = oldItem;

                if (oldItem == null)
                    backpackItemCount--;

                return true;
            }
        }

        public InventoryItem DropItem(int slotId)
        {
            lock (_lock)
            {
                if (BackpackItems[slotId] != null)
                {
                    var item = BackpackItems[slotId];
                    BackpackItems[slotId] = null;
                    backpackItemCount--;

                    return item;
                }

                return null;
            }
        }


        public InventoryItem DropItem(InventorySlots slot)
        {
            lock (_lock)
            {
                if (EquippedItems[slot] != null)
                {
                    var item = EquippedItems[slot];
                    EquippedItems[slot] = null;
                    return item;
                }

                return null;
            }
        }


        public void MoveItem(int from, int to)
        {
            lock (_lock)
            {
                var temp = BackpackItems[to];

                BackpackItems[to] = BackpackItems[from];
                BackpackItems[from] = temp;
            }
        }

        internal override void Update(int msElapsed)
        {
            // pass
        }
    }
}
