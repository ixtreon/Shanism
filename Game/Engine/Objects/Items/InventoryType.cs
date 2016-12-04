using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Items
{
    /// <summary>
    /// Defines the types and amount of items a unit can carry.  
    /// </summary>
    public struct InventoryType
    {

        #region Static Members

        /// <summary>
        /// No inventory at all. 
        /// </summary>
        public static readonly InventoryType None = new InventoryType(0, EquipSlot.None);

        /// <summary>
        /// An inventory with 6 backpack slots and no equip slots. 
        /// </summary>
        public static readonly InventoryType SimpleBackpack = new InventoryType(6, EquipSlot.None);

        /// <summary>
        /// A simple RPG-like inventory with 20 backpack slots and 4 equip slots: 
        /// head, torso, legs, feet, and main hand. 
        /// </summary>
        public static readonly InventoryType SimpleRpg = new InventoryType(20,
            EquipSlot.Head | EquipSlot.Torso | EquipSlot.Legs | EquipSlot.Feet | EquipSlot.MainHand);

        /// <summary>
        /// An extensive RPG inventory with 30 backpack slots and all equip slots. 
        /// </summary>
        public static readonly InventoryType FullBlownRpg = new InventoryType(30, EquipSlot.All);

        #endregion


        /// <summary>
        /// Gets the number of backpack slots of this inventory type. 
        /// </summary>
        public readonly byte BackpackSize;

        /// <summary>
        /// Gets a bitmask comprising of the equip slots available for this inventory type. 
        /// </summary>
        public readonly EquipSlot EquipSlots;


        public InventoryType(byte backpackSlots, EquipSlot slots)
        {
            BackpackSize = backpackSlots;
            EquipSlots = slots;
        }
    }
}
