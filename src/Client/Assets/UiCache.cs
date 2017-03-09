using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Drawing
{
    class UiCache
    {
        public IconSprite MenuClose { get; }

        public UiCache(ContentList assets)
        {
            MenuClose = new IconSprite(assets, "ui/close");
        }
    }
}
