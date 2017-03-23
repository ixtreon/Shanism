using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Util;
using System.IO;

namespace Shanism.Client.Assets
{
    public class TextureCache
    {
        readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        readonly Dictionary<string, TextureDef> textureDefs = new Dictionary<string, TextureDef>();

        readonly GraphicsDevice device;

        public Texture2D Blank { get; private set; }

        public TextureCache(GraphicsDevice device)
        {
            this.device = device;
        }

        public void Load(string texPath, IEnumerable<TextureDef> texList)
        {
            foreach (var texDef in texList)
            {
                try
                {
                    var name = texDef.Name;
                    var normName = normalize(name);

                    Texture2D tex;
                    using (var ms = File.OpenRead(texPath + "/" + name))
                        tex = Texture2D.FromStream(device, ms);

                    textureDefs[normName] = texDef;
                    textures[normName] = tex;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error opening resource {texDef.Name}: {e.Message}");
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
        public bool TryGetValue(string name, out Texture2D tex) => textures.TryGetValue(normalize(name), out tex);
        public bool TryGetValue(string name, out TextureDef tex) => textureDefs.TryGetValue(normalize(name), out tex);
        public bool Contains(string name) => textures.ContainsKey(normalize(name));
    }
}
