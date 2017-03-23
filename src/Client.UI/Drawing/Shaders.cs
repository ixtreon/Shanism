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
    public class ShaderContainer
    {
        public Effect FogOfWar { get; private set; }

        public Effect ObjectShadows { get; private set; }


        public ShaderContainer(ContentManager content)
        {
            content.RootDirectory = "Shaders";

            FogOfWar = tryLoadShader(content, "basicShadows");
            ObjectShadows = tryLoadShader(content, "shadows");
        }

        Effect tryLoadShader(ContentManager content, string name)
        {
            try
            {
                return content.Load<Effect>(name);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error loading shader '{name}': {e.Message}");
                return null;
            }
        }
    }
}
