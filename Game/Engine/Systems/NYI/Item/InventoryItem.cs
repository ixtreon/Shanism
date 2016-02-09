using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Common;
using Engine.Systems.Buffs;
using Engine.Systems.Abilities;
using Engine.Systems.Item;

namespace Engine.Entities
{
    //NYI
    public class InventoryItem : ScenarioObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public Buff Bonuses { get; set; }

        public Ability Ability { get; set; }

        public InventorySlots Slot { get; set; }

        public InventoryItem()
        {
        }
    }
}
