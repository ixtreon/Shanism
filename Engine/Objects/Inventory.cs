using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Common;
using IO.Common;

namespace Engine.Objects
{
    public class Inventory
    {
        const int BackpackWidth = 5;
        const int BackpackHeight = 4;
        const int BackpackSize = BackpackWidth * BackpackHeight;

        private int backpackItemCount;  //numberOfItemsInBackpack

        internal Engine.Objects.Item[,] Backpack;

        Dictionary<EquipSlot, Item> EquippedItems = new Dictionary<EquipSlot, Item>();

        public Inventory()
        {
            Backpack = new Item[BackpackWidth, BackpackHeight];
        }

        public bool TryPickupItem(GameItem i)
        {
            //continue only if there is space in the backpack. 
            if(backpackItemCount == BackpackSize)
                return false;

            // are we always allowed to pick an item as the client requests?
            // what if the item is on the other side of the world?

            for (int j = 0; j < BackpackWidth; j++)
                for (int k = 0; k < BackpackHeight; k++)
                    if (Backpack[j, k] == null)
                    {
                        Backpack[j, k] = i.Item;
                        backpackItemCount++;
                        return true;
                    }
                
            return false;
        }
        public bool TryEquipItem(Point backpackSlot, EquipSlot slot)
        {
            //continue only if there is an item in this slot and it is equippable. 
            if (Backpack[backpackSlot.X, backpackSlot.Y] == null || Backpack[backpackSlot.X, backpackSlot.Y].Type == EquipSlot.None)
                return false;

            Item oldItem = null;
            EquippedItems.TryGetValue(slot, out oldItem);   //ako ima item go vzema, inache ne bara oldItem
            EquippedItems[slot] = Backpack[backpackSlot.X, backpackSlot.Y];
            Backpack[backpackSlot.X, backpackSlot.Y] = oldItem; 
            return true;
        }
        public Item DropItem(Point backpackSlot)
        {
            if (Backpack[backpackSlot.X, backpackSlot.Y] != null)
                return Backpack[backpackSlot.X, backpackSlot.Y];

            return null;
        }
        public Item DropItem(EquipSlot slot)
        {
            if (EquippedItems[slot] != null)
                return EquippedItems[slot];
            else return null;
        }
        public void MoveItem(Point from, Point to)
        {
            Item temp;
            temp = Backpack[to.X, to.Y];
            Backpack[to.X, to.Y] = Backpack[from.X, from.Y];
            Backpack[from.X, from.Y] = temp;
        }
    }
}
