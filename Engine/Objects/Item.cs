using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Common;

namespace Engine.Objects
{
    public class Item
    {
        public string Name;
        public string Icon;
        public Buff Bonuses;
        public EquipSlot Type;

        public Item(string itemName, string itemIcon, Buff itemBonuses, EquipSlot itemType)
        {
            this.Name = itemName;
            this.Icon = itemIcon;
            this.Bonuses = itemBonuses;
            this.Type = itemType;
        }
    }
}
