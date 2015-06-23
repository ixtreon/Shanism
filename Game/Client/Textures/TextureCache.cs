using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using IO;
using IO.Content;

namespace Client.Textures
{
    static class TextureCache
    {

        /// <summary>
        /// The directory where all the resources are. 
        /// </summary>
        const string ContentDir = @"Content\";

        public static TextureFont FancyFont { get; private set; }
        public static TextureFont StraightFont { get; private set; }
        public static TextureFont SmallFont { get; private set; }
        public static TextureFont HugeFont { get; private set; }


        static ContentManager content;

        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static void LoadContent(ContentManager content)
        {
            TextureCache.content = content;
            loadTextures();

            //load fonts
            FancyFont = new TextureFont(content, "Fonts\\UI", 0.5, 4);
            StraightFont = new TextureFont(content, "Fonts\\ui-text", 0.5);
            SmallFont = new TextureFont(StraightFont, 0.8);
            HugeFont = new TextureFont(StraightFont, 1.6);
        }

        static void loadTexture(string name)
        {
            var tex = content.Load<Texture2D>(name);
            textures.Add(name, tex);
        }

        /// <summary>
        /// Load the textures and bind them to locals here. 
        /// </summary>
        static void loadTextures()
        {
            //enumerate all png files in the content directory. 
            foreach (var f in System.IO.Directory.EnumerateFiles(ContentDir, "*.png", SearchOption.AllDirectories))
            {
                var fne = f
                    .Take(f.LastIndexOf("."))   // remove extension
                    .Skip(ContentDir.Length);   // remove content directory
                var fn = new string(fne.ToArray());
                try
                {
                    loadTexture(fn);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error opening resource {0}: {1}", f, e.Message);
                }
            }
        }

        /// <summary>
        /// Gets the texture with the specified raw name. 
        /// </summary>
        /// <param name="tName"></param>
        public static Texture2D Get(string tName)
        {
            return textures[tName];
        }

        public static Texture2D Get(TextureDef file)
        {
            return textures[file.Name];
        }

        /// <summary>
        /// Gets the texture with the specified name, relative for the specified <see cref="TextureType"/>. 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Texture2D Get(TextureType t, string name)
        {
            var texPath = Path.Combine(t.GetDirectory(name));
            return textures[texPath];
        }
    }
}
