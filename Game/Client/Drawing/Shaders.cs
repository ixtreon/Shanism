using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    class ShaderContainer
    {
        readonly ContentManager content;

        public Effect FogOfWar { get; private set; }


        public Effect ObjectShadows { get; private set; }


        public ShaderContainer(ContentManager content)
        {
            this.content = content;

            Load();
        }

        /// <summary>
        /// Loads or reloads all shaders.
        /// </summary>
        public void Load()
        {
            content.RootDirectory = "Shaders";

            FogOfWar = tryLoadShader("shader");
            ObjectShadows = tryLoadShader("shadows");
        }

        Effect tryLoadShader(string name)
        {
            try
            {
                return content.Load<Effect>(name);
            }
            catch
            {
                return null;
            }
        }
    }
}
