using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Engine.Objects.Buffs;
using Shanism.Common;
using Shanism.Engine.Objects.Abilities;
using Shanism.Common.Interfaces.Objects;

namespace Shanism.Engine.Objects.Items
{
    /// <summary>
    /// An item lying inside a unit's inventory. 
    /// </summary>
    /// <seealso cref="Shanism.Engine.GameObject" />
    public class Item : GameObject, IItem
    {
        /// <summary>
        /// Gets the <see cref="Shanism.Common.Game.ObjectType" /> of this game object. 
        /// Always returns <see cref="ObjectType.Item"/>. 
        /// </summary>
        public override ObjectType ObjectType { get; } = ObjectType.Item;

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the icon string of the item.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the buff obtained when carrying the item. 
        /// </summary>
        public Buff CarryBuff { get; set; }

        /// <summary>
        /// Gets or sets the buff obtained when the item is equipped. 
        /// </summary>
        public Buff EquipBuff { get; set; }

        /// <summary>
        /// Gets or sets the ability that becomes available from this item. 
        /// </summary>
        public Ability Ability { get; set; }

        IBuff IItem.CarryBuff => CarryBuff;
        IBuff IItem.EquipBuff => EquipBuff;
        IAbility IItem.Ability => Ability;

        /// <summary>
        /// Gets or sets the equip slot this item can go to. 
        /// </summary>
        public EquipSlot Slot { get; set; }

        public Item()
        {
        }
    }
}
