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

namespace Client.Textures
{
    class TextureCache
    {
        Dictionary<string, Texture2D> textures { get; }  = new Dictionary<string, Texture2D>();

        public Texture2D Blank { get; private set; }


        public TextureCache()
        {
        }

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
        }


        string getName(TextureDef tex)
        {
            return new string(tex.Name
                    .Take(tex.Name.LastIndexOf("."))   // remove extension
                    .Skip(tex.Name.IndexOf("\\") + 1)
                    .Select(c => char.ToLowerInvariant(c))
                    .ToArray());
        }

        void loadTexture(ContentManager content, TextureDef texDef)
        {
            var name = getName(texDef);
            var tex = content.Load<Texture2D>(name);
            textures[name] = tex;
        }

        public Texture2D this[TextureDef file]
        {
            get
            {
                return textures[getName(file)];
            }
        }

        internal Texture2D TryGetRaw(string name)
        {
            return textures.TryGet(name.ToLowerInvariant());
        }

        internal Texture2D TryGetIcon(string iconName)
        {
            return TryGetRaw(TextureType.Icon.GetDirectory(iconName));
        }

        internal Texture2D TryGetObject(string objName)
        {
            return TryGetRaw(TextureType.Model.GetDirectory(objName));
        }

        internal Texture2D TryGetUI(string name)
        {
            return TryGetRaw(TextureType.Ui.GetDirectory(name));
        }
    }
}
