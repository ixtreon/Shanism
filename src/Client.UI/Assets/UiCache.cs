using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Sprites;

namespace Shanism.Client.Assets
{
    public class UiCache
    {
        public StaticSprite MenuClose { get; }
        public StaticSprite IconBorder { get; }
        public StaticSprite IconBorderHover { get; }

        public UiCache(ContentList assets)
        {
            MenuClose = new StaticSprite(assets, "ui/close");
            IconBorder = new StaticSprite(assets, "ui/border");
            IconBorderHover = new StaticSprite(assets, "ui/border-hover");
        }
    }
}
