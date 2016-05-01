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
using IO.Common;
using ScenarioLib;
using System.Text.RegularExpressions;

namespace Client.Textures
{
    public class TextureCache
    {
        readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public Texture2D Blank { get; private set; }

        public Texture2D DefaultIcon { get; private set; }



        public void LoadContent(ContentManager content, string contentDir, IEnumerable<TextureDef> texList)
        {
            content.RootDirectory = contentDir;

            foreach (var tex in texList)
            {
                try
                {
                    loadTexture(content, tex);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error opening resource {0}: {1}", tex.Name, e.Message);
                }
            }

            Blank = TryGetRaw("1");
            DefaultIcon = TryGetIcon("default");
        }


        void loadTexture(ContentManager content, TextureDef texDef)
        {
            var name = texDef.Name.RemoveFileExtension().ToLowerInvariant();

            var tex = content.Load<Texture2D>(name);
            textures[name] = tex;
        }

        public Texture2D this[TextureDef file]
        {
            get
            {
                return textures[file.Name.RemoveFileExtension().ToLowerInvariant()];
            }
        }

        /// <summary>
        /// Tries to get the texture with the given full (raw) name. 
        /// </summary>
        /// <param name="name">The full name of the texture. </param>
        /// <returns>The texture with that name, or null if no such texture was found. </returns>
        public Texture2D TryGetRaw(string name)
        {
            return textures.TryGet(name.ToLowerInvariant());
        }

        /// <summary>
        /// Tries to get the texture with the given name relative to the Icons folder. See <see cref="TextureType"/>. 
        /// </summary>
        /// <param name="iconName">The name of the texture relative to the Icons folder. </param>
        /// <returns>The texture with that name, or null if no such texture was found. </returns>
        public Texture2D TryGetIcon(string iconName)
        {
            return TryGetRaw(TextureType.Icon.GetDirectory(iconName));
        }

        /// <summary>
        /// Tries to get the texture with the given name relative to the UI folder. See <see cref="TextureType"/>. 
        /// </summary>
        /// <param name="name">The name of the texture relative to the UI folder. </param>
        /// <returns>The texture with that name, or null if no such texture was found. </returns>
        public Texture2D TryGetUI(string name)
        {
            return TryGetRaw(TextureType.Ui.GetDirectory(name));
        }
    }
}
