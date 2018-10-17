using Shanism.Common;
using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Sprites;
using System.Numerics;

namespace Shanism.Client.UI.Game
{
    class ItemSlot : Control
    {
        public EquipSlot SlotType { get; set; } = EquipSlot.None;

        public IItem Item { get; set; }

        Sprite icon;

        public ItemSlot()
        {

        }

        public override void Update(int msElapsed)
        {
            if (Item == null)
            {
                icon = Content.Icons.Default;
                return;
            }

            //refresh the item's icon
            if(Item.Icon != icon.Name)
                icon = Content.Icons.Get(Item.Icon);

        }

        public override void Draw(Canvas g)
        {
            if(icon != null)
                g.DrawSprite(icon, Vector2.Zero, Size, Color.White);
        }

    }
}
