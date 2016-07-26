using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Game;
using Shanism.ScenarioLib;
using System.Text.RegularExpressions;
using Shanism.Common.Util;

namespace Shanism.Client.Drawing
{
    public class TextureCache
    {
        readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, TextureDef> textureDefs = new Dictionary<string, TextureDef>();


        public Texture2D Blank { get; private set; }

        public Texture2D DefaultIcon { get; private set; }

        public Texture2D IconPack { get; private set; }

        public Texture2D IconBorder { get; private set; }

        public Texture2D UiCloseButton { get; private set; }

        public void Load(ContentManager content, string contentDir, IEnumerable<TextureDef> texList)
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

            Blank = this["1"];

            DefaultIcon = this["icons/default"];
            IconBorder = this["icons/border_hover"];
            UiCloseButton = this["ui/close"];
        }

        static string normalize(string n) => 
            ShanoPath.RemoveExtension(n)
            .ToLowerInvariant();

        void loadTexture(ContentManager content, TextureDef texDef)
        {
            var name = normalize(texDef.Name);
            var tex = content.Load<Texture2D>(name);

            textureDefs[name] = texDef;
            textures[name] = tex;
        }


        public Texture2D this[string name] => textures[normalize(name)];
        public bool TryGet(string name, out Texture2D tex) => textures.TryGetValue(normalize(name), out tex);
        public bool TryGet(string name, out TextureDef tex) => textureDefs.TryGetValue(normalize(name), out tex);
        public bool Contains(string name) => textures.ContainsKey(normalize(name));


        /// <summary>
        /// Tries to get the texture with the given name relative to the Icons folder. See <see cref="TextureType"/>. 
        /// </summary>
        /// <param name="iconName">The name of the texture relative to the Icons folder. </param>
        /// <returns>The texture with that name, or null if no such texture was found. </returns>
        public Texture2D TryGetIcon(string iconName)
        {
            return textures.TryGet(normalize(TextureType.Icon.GetDirectory(iconName)));
        }
    }
}
