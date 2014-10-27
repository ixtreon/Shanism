using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using IO;

namespace ShanoRpgWinGl.Sprites
{
    static class TextureCache
    {
        /// <summary>
        /// The directory where all the resources are. 
        /// </summary>
        const string ContentDir = @"Content\";

        public static TextureFont MainFont { get; private set; }
        public static TextureFont UiFont { get; private set; }


        static ContentManager content;

        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static void LoadContent(ContentManager content)
        {
            TextureCache.content = content;
            loadTextures();

            //load fonts
            MainFont = new TextureFont(content, "Fonts\\UI", 0.5, 1.5);
            UiFont = new TextureFont(content, "Fonts\\ui-text", 0.5);
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
                    Console.WriteLine("Error opening resource {0}", f);
                }
            }
        }

        /// <summary>
        /// Gets the texture with the specified raw name. 
        /// </summary>
        /// <param name="tName"></param>
        /// <returns></returns>
        public static Texture2D Get(string tName)
        {
            return textures[tName];
        }

        /// <summary>
        /// Gets the texture with the specified name, relative for the specified <see cref="TextureType"/>. 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Texture2D Get(TextureType t, string name)
        {
            var tName = Path.Combine(t.GetDirectory(), name);
            return textures[tName];
        }

        static void loadTexture(string name)
        {
            var tex = content.Load<Texture2D>(name);
            textures.Add(name, tex);
        }
    }
}
