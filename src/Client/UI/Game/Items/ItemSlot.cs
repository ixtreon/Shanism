using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Sprites;

namespace Shanism.Client.UI.Game
{
    class ItemSlot : Control
    {
        public EquipSlot SlotType { get; set; } = EquipSlot.None;

        public IItem CurrentItem { get; set; }

        StaticSprite currentIcon;

        public ItemSlot()
        {

        }

        protected override void OnUpdate(int msElapsed)
        {
            if (CurrentItem == null)
                currentIcon = Content.Icons.Default;
            else
                currentIcon = Content.Icons.TryGet(CurrentItem.Icon) ?? Content.Icons.Default;
        }

        public override void OnDraw(Canvas g)
        {
            if(currentIcon != null)
                g.Draw(currentIcon, Vector.Zero, Size);
        }

    }
}
