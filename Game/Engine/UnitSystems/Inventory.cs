using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Engine.Objects.Items;
using Shanism.Engine.Maps;

namespace Shanism.Engine.Systems
{
    //NYI
    class InventorySystem : UnitSystem, IUnitInventory
    {

        static void CreateItemOnGround(MapSystem map, Vector pos, Item it)
            => map.Add(new GameItem(it, pos));



        readonly Unit Owner;

        Dictionary<EquipSlot, Item> equippedItems;

        InventoryType _type = InventoryType.None;

        Backpack backpack;



        public InventorySystem(Unit owner)
        {
            Owner = owner;

            backpack = new Backpack(0);
            equippedItems = new Dictionary<EquipSlot, Item>();
        }



        public IReadOnlyDictionary<EquipSlot, Item> EquippedItems => equippedItems;

        public IReadOnlyCollection<Item> BackpackItems => backpack.ItemGrid;

        public InventoryType Type
        {
            get { return _type; }
            set { setType(value); }
        }


        void setType(InventoryType newKind)
        {
            _type = newKind;
            var newSize = newKind.BackpackSize;

            var oldItems = backpack.Items.ToList();
            backpack = new Backpack(newSize, oldItems.Take(newSize));
            for (var i = newSize; i < oldItems.Count; i++)
                dropItem(oldItems[i]);


            var oldEquips = equippedItems.Values;
            equippedItems = new Dictionary<EquipSlot, Item>();
            foreach (var eq in oldEquips)
                if (!PickupItem(eq))
                    dropItem(eq);
        }

        bool canEquip(Item it)
            => it.Slot != EquipSlot.None && Type.EquipSlots.HasFlag(it.Slot);

        public bool PickupItem(GameItem mapItem)
        {
            return PickupItem(mapItem.Item);
        }


        public bool PickupItem(Item item)
        {
            //first try to equip it
            if (canEquip(item) && !equippedItems.ContainsKey(item.Slot))
            {
                equippedItems.Add(item.Slot, item);
                return true;
            }

            //else put in backpack
            if (backpack.Add(item))
                return true;

            //else fail
            return false;
        }

        public bool SwapItems(int backpackSlot, EquipSlot equipSlot)
        {
            var backpackItem = backpack.ItemAt(backpackSlot);
            var equipItem = equippedItems.TryGet(equipSlot);

            //can't swap if there's an item in the backpack that can't go in the slot
            if (backpackItem != null && backpackItem.Slot != equipSlot)
                return false;

            equippedItems[equipSlot] = backpackItem;
            backpack.PlaceItem(backpackSlot, equipItem);
            return true;
        }

        public void SwapItems(int backpackSlotA, int backpackSlotB)
            => backpack.Swap(backpackSlotA, backpackSlotB);

        void dropItem(Item it) => CreateItemOnGround(GameObject.currentGame.map, Owner.Position, it);

        public bool DropItem(int slotId)
        {
            Item it;
            if (backpack.TryRemove(slotId, out it))
            {
                dropItem(it);
                return true;
            }

            return false;
        }


        public bool DropItem(EquipSlot slot)
        {
            Item it;
            if (equippedItems.TryGetValue(slot, out it))
            {
                equippedItems.Remove(slot);
                dropItem(it);
                return true;
            }

            return false;
        }

        public override void Update(int msElapsed)
        {
            // update items, inventory?
        }
    }
}
