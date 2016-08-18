using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Drawing
{
    /// <summary>
    /// Contains a listing of common fonts. 
    /// </summary>
    class FontCache
    {

        /// <summary>
        /// A fancy-looking, large font. 
        /// </summary>
        public TextureFont FancyFont { get; private set; }

        /// <summary>
        /// A normal-sized standard font. 
        /// </summary>
        public TextureFont NormalFont { get; private set; }

        /// <summary>
        /// A small-sized standard font. 
        /// </summary>
        public TextureFont SmallFont { get; private set; }

        /// <summary>
        /// A large-sized standard font. 
        /// </summary>
        public TextureFont LargeFont { get; private set; }

        public SpriteFont ShanoFont { get; private set; }

        /// <summary>
        /// Loads all fonts to memory using the provided <see cref="ContentManager"/>. 
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            content.RootDirectory = "Fonts/";

            var fancy = content.Load<SpriteFont>("psicopatologia");
            var main = content.Load<SpriteFont>("helvetica");

            //load default fonts
            FancyFont = new TextureFont(fancy, 0.6);
            NormalFont = new TextureFont(main, 0.6);

            SmallFont = new TextureFont(main, 0.5);
            LargeFont = new TextureFont(main, 0.72);
        }
    }
}
