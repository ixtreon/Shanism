using System.Collections.Generic;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Items;
using Shanism.Common.Game;

namespace Shanism.Engine.Systems
{
    public interface IUnitInventory
    {
        /// <summary>
        /// Gets the grid of items in the inventory backpack.
        /// </summary>
        IReadOnlyCollection<Item> BackpackItems { get; }

        /// <summary>
        /// Gets the collection of equipped items.
        /// </summary>
        IReadOnlyDictionary<EquipSlot, Item> EquippedItems { get; }

        /// <summary>
        /// Gets or sets the type of the inventory. 
        /// </summary>
        InventoryType Type { get; set; }

        /// <summary>
        /// Causes the unit to drop the item from the selected backpack slot. 
        /// Fails if there is no item at that slot. 
        /// </summary>
        /// <param name="slotId">The slot identifier.</param>
        /// <returns>True if an item was successfully dropped. False otherwise.</returns>
        bool DropItem(int slotId);

        /// <summary>
        /// Causes the unit to drop the item from the selected equip slot. 
        /// Fails if there is no item at that slot. 
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <returns>True if the item was successfully dropped. False otherwise.</returns>
        bool DropItem(EquipSlot slot);

        /// <summary>
        /// Causes the unit to pick-up the specified item. 
        /// Fails if the unit has no space in its inventory. 
        /// </summary>
        /// <param name="item">The item to pick up.</param>
        /// <returns>True if the item was successfully picked up. False otherwise.</returns>
        bool PickupItem(Item item);

        /// <summary>
        /// Causes the unit to pick-up the specified item from the ground.
        /// Fails if the unit has no space in its inventory. 
        /// TODO: Fail if the unit is too far away from the item.
        /// </summary>
        /// <param name="mapItem">The map item.</param>
        /// <returns>True if the item was successfully picked up. False otherwise.</returns>
        bool PickupItem(GameItem mapItem);


        void SwapItems(int backpackSlotA, int backpackSlotB);

        bool SwapItems(int backpackSlot, EquipSlot equipSlot);
    }
}