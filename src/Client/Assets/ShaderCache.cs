using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Assets;
using System;
using System.IO;

namespace Shanism.Client.Systems
{
    public class ShaderCache
    {

        public Effect FogOfWar { get; private set; }

        public Effect ObjectShadows { get; private set; }


        public ShaderCache() { }

        public ShaderCache(ContentManager manager, string basePath)
        {
            bool TryLoadShader(string shaderName, out Effect shader)
            {
                var fullPath = Path.Combine(basePath, "Shaders", shaderName);
                try
                {
                    shader = manager.Load<Effect>(fullPath);
                    return true;
                }
                catch (Exception e)
                {
                    ClientLog.Instance.Error($"Unable to load shader '{shaderName}': {e.Message}");
                    shader = null;
                    return false;
                }
            }


            if (TryLoadShader("basicShadows", out var fogOfWar))
                FogOfWar = fogOfWar;

            if (TryLoadShader("shadows", out var shadows))
                ObjectShadows = shadows;
        }


    }
}
