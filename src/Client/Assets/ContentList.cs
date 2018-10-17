using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.IO;
using Shanism.Client.Systems;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Client.Assets
{
    public class ContentList
    {

        public CircleCache Circles { get; }

        public ShaderCache Shaders { get; }
        public Text.FontCache Fonts { get; }

        public TextureDict Textures { get; }
        public AnimationDict Animations { get; }

        public IconCache Icons { get; }
        public UICache UI { get; }


        public ContentList(ScreenSystem screen,
            Text.FontCache fonts,
            GraphicsDevice device,
            ContentManager manager,
            ScenarioConfig scenario)
            : this(screen, fonts, device, manager, scenario.BaseDirectory, scenario.Content)
        {
            // empty
        }


        public ContentList(ScreenSystem screen,
            Text.FontCache fonts,
            GraphicsDevice device,
            ContentManager manager,
            string path,
            ContentConfig config
        )
        {
            Circles = new CircleCache(device, 2048, x => 8, preload: true);
            Shaders = new ShaderCache(manager, path);
            Fonts = fonts;

            Textures = new TextureDict(device, path, config.Textures);
            Animations = new AnimationDict(Textures, config.Animations);

            Icons = new IconCache(Animations, "icons/", "uncertainty");
            UI = new UICache(Animations, "ui/");
        }

        public ContentList(ContentList first, ContentList second)
        {
            Circles = first.Circles;
            Shaders = first.Shaders;
            Fonts = first.Fonts;

            Textures = new TextureDict(first.Textures.Concat(second.Textures));
            Animations = new AnimationDict(first.Animations.Concat(second.Animations));

            Icons = new IconCache(Animations, "icons/", "uncertainty");
            UI = new UICache(Animations, "ui/");
        }
    }
}
