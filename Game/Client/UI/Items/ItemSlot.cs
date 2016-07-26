using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Items
{
    class ItemSlot : Control
    {
        public EquipSlot SlotType { get; set; } = EquipSlot.None;

        public IItem CurrentItem { get; set; }

        Texture2D currentTexture;

        public ItemSlot()
        {

        }

        protected override void OnUpdate(int msElapsed)
        {
            if (CurrentItem == null)
                currentTexture = Content.Textures.DefaultIcon;
            else
                currentTexture = Content.Textures.TryGetIcon(CurrentItem.Icon) ?? Content.Textures.DefaultIcon;
        }

        public override void OnDraw(Graphics g)
        {
            if(currentTexture != null)
                g.Draw(currentTexture, Vector.Zero, Size);
        }

    }
}
