using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client;
using Shanism.Client.UI;
using Microsoft.Xna.Framework;

namespace Shanism.Client
{
    public class ShanoComponent : IShanoComponent
    {
        IShanoComponent source;

        public KeyboardInfo Keyboard => source.Keyboard;
        public MouseInfo Mouse => source.Mouse;
        public Screen Screen => source.Screen;
        public ContentList Content => source.Content;

        public ShanoComponent(IShanoComponent source)
        {
            if (source is ShanoComponent)
                this.source = ((ShanoComponent)source).source;
            else
                this.source = source;
        }
    }

    public class OldShanoComponent : IShanoComponent
    {
        public KeyboardInfo Keyboard { get; }
        public MouseInfo Mouse { get; }
        public Screen Screen { get; }
        public ContentList Content { get; }

        public OldShanoComponent(GraphicsDevice device, ContentManager contentManager)
        {
            Content = new ContentList(device, contentManager);
            Screen = new Screen(device);

            Keyboard = new KeyboardInfo();
            Mouse = new MouseInfo(Screen);
        }
    }

}
