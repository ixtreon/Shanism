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
    /// Contains a listing of common fonts of type <see cref="TextureFont"/>. 
    /// </summary>
    class FontCache
    {

        /// <summary>
        /// A fancy-looking, large font. 
        /// </summary>
        public TextureFont FancyFont { get; }

        /// <summary>
        /// A normal-sized standard font. 
        /// </summary>
        public TextureFont NormalFont { get; }

        /// <summary>
        /// A small-sized standard font. 
        /// </summary>
        public TextureFont SmallFont { get; }

        /// <summary>
        /// A large-sized standard font. 
        /// </summary>
        public TextureFont LargeFont { get; }

        public TextureFont ShanoFont { get; }

        /// <summary>
        /// Loads all fonts to memory using the provided <see cref="ContentManager"/>. 
        /// </summary>
        /// <param name="content"></param>
        public FontCache(ContentManager content)
        {
            content.RootDirectory = "Fonts/";

            var fancy = content.Load<SpriteFont>("psicopatologia");
            var main = content.Load<SpriteFont>("helvetica");

            //load default fonts
            FancyFont = new TextureFont(fancy, 0.07);
            ShanoFont = new TextureFont(fancy, 0.16);


            NormalFont = new TextureFont(main, 0.05);
            SmallFont = new TextureFont(main, 0.03);
            LargeFont = new TextureFont(main, 0.07);
        }
    }
}
