using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Engine.Objects.Buffs;
using Shanism.Engine.Systems.Abilities;
using Shanism.Engine.Systems.Item;
using Shanism.Common.Game;

namespace Shanism.Engine.Objects
{
    //NYI
    class InventoryItem : GameObject
    {
        public override ObjectType ObjectType {  get { return ObjectType.Item; } }


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
