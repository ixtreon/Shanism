using Microsoft.Xna.Framework;
using ShanoRpgWinGl.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRpgWinGl.UI
{
    class Window : UserControl
    {
        public readonly Button CloseButton = new Button()
        {
            Size = new Vector2(0.12f, 0.12f),
            RelativePosition = new Vector2(0.85f, 0.03f),
            Texture = TextureCache.Get(@"UI\close"),
        };

        public Window()
        {
            Add(CloseButton);
        }
    }
}
