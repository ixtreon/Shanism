using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Common;
using Engine.Systems.Buffs;
using Engine.Systems.Abilities;

namespace Engine.Objects
{
    //NYI
    public class Item : ScenarioObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public Buff Bonuses { get; set; }

        public Ability Ability { get; set; }

        public EquipSlot Type { get; set; }

        public Item(string itemName, string itemIcon, Buff itemBonuses, EquipSlot itemType)
        {
            this.Name = itemName;
            this.Icon = itemIcon;
            this.Bonuses = itemBonuses;
            this.Type = itemType;
        }
    }
}
