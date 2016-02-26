using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems.Item
{
    public struct InventoryType
    {

        #region Static Members

        /// <summary>
        /// An inventory with 6 backpack slots and no equip slots. 
        /// </summary>
        public static readonly InventoryType SimpleBackpack = new InventoryType(6, InventorySlots.None);

        /// <summary>
        /// An extensive RPG inventory with 30 backpack slots and all equip slots. 
        /// </summary>
        public static readonly InventoryType FullBlownRpg = new InventoryType(30, InventorySlots.All);

        /// <summary>
        /// A simple RPG-like inventory with 20 backpack slots and 4 equip slots: 
        /// head, torso, legs, feet, and main hand. 
        /// </summary>
        public static readonly InventoryType SimpleRpg = new InventoryType(20, 
            InventorySlots.Head | InventorySlots.Torso | InventorySlots.Legs | InventorySlots.Feet | InventorySlots.MainHand);

        #endregion


        /// <summary>
        /// Gets the number of backpack slots of this inventory type. 
        /// </summary>
        public readonly int BackpackSlots;

        /// <summary>
        /// Gets a bitmask comprising of the equip slots available for this inventory type. 
        /// </summary>
        public readonly InventorySlots EquipSlots;

        public InventoryType(int backpackSlots, InventorySlots slots)
        {
            BackpackSlots = backpackSlots;
            EquipSlots = slots;
        }
    }
}
