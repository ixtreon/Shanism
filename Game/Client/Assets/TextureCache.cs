using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Util;

namespace Shanism.Client.Drawing
{
    public class TextureCache
    {
        readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, TextureDef> textureDefs = new Dictionary<string, TextureDef>();


        public Texture2D Blank { get; private set; }

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
    }
}
