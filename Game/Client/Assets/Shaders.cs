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
    static class Shaders
    {
        public static Effect effect { get; private set; }

        public static void Load(GraphicsDevice graphics, ContentManager content)
        {
            content.RootDirectory = "Shaders";

            try
            {
                effect = content.Load<Effect>("shader");
            }
            catch
            {
                effect = null;
            }
        }
    }
}
