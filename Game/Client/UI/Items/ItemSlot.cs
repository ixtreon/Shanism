using Shanism.Client.Drawing;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.UI.Items
{
    class ItemSlot : Control
    {
        public EquipSlot SlotType { get; set; } = EquipSlot.None;

        public IItem CurrentItem { get; set; }

        IconSprite currentIcon;

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

        public override void OnDraw(Graphics g)
        {
            if(currentIcon != null)
                g.Draw(currentIcon, Vector.Zero, Size);
        }

    }
}
