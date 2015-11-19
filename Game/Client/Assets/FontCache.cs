using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Assets.Fonts
{
    class FontCache
    {

        //Fonts
        public TextureFont FancyFont { get; private set; }
        public TextureFont MediumFont { get; private set; }
        public TextureFont SmallFont { get; private set; }
        public TextureFont LargeFont { get; private set; }


        public FontCache()
        {

        }

        public void Load(ContentManager content)
        {
            content.RootDirectory = @"Content\";
            //load default fonts
            FancyFont = new TextureFont(content, "Fonts\\UI", 0.5, 4);
            MediumFont = new TextureFont(content, "Fonts\\ui-text", 0.5);
            SmallFont = new TextureFont(MediumFont, 0.8);
            LargeFont = new TextureFont(MediumFont, 1.6);
        }
    }
}
