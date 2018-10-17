using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Content;
using Shanism.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Shanism.Client.Assets
{
    public class TextureDict : IEnumerable<ShanoTexture>
    {

        readonly Dictionary<string, ShanoTexture> textures = new Dictionary<string, ShanoTexture>();


        public TextureDict(IEnumerable<ShanoTexture> textures)
        {
            this.textures = new Dictionary<string, ShanoTexture>();
            foreach (var tex in textures)
                this.textures[tex.Name] = tex;
        }

        public TextureDict(GraphicsDevice device, string basePath, IEnumerable<TextureDef> textureDefinitions)
        {
            IEnumerable<ShanoTexture> LoadTextures()
            {
                foreach (var texDef in textureDefinitions)
                    if (TryLoadTexture(device, basePath, texDef, out var tex))
                        yield return tex;
                    else
                        ClientLog.Instance.Warning($"Unable to load texture `{texDef.Name}`.");
            }

            textures = LoadTextures().ToDictionary(x => x.Name, x => x);
        }

        static bool TryLoadTexture(GraphicsDevice device, string path, TextureDef texDef, out ShanoTexture tex)
        {
            var pathToTexture = Path.Combine(path, "Textures", texDef.Name);
            try
            {
                Texture2D tex2d;
                using (var ms = File.OpenRead(pathToTexture))
                    tex2d = Texture2D.FromStream(device, ms);

                tex = new ShanoTexture(tex2d, texDef);
                return true;
            }
            catch
            {
                tex = null;
                return false;
            }
        }


        public bool TryGet(string name, out ShanoTexture tex)
        {
            Debug.Assert(name == ShanoPath.NormalizeTexture(name));
            return textures.TryGetValue(name, out tex);
        }

        public bool Contains(string name)
        {
            Debug.Assert(name == ShanoPath.NormalizeTexture(name));
            return textures.ContainsKey(name);
        }

        public IEnumerator<ShanoTexture> GetEnumerator() => textures.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => textures.Values.GetEnumerator();
    }

    public static class TextureDictExt
    {
        public static TextureDict ToTextureDict(this IEnumerable<ShanoTexture> source)
            => new TextureDict(source);
    }

}
