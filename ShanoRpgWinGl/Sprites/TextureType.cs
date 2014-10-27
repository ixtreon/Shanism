using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRpgWinGl.Sprites
{
    /// <summary>
    /// The type (family) of a texture. 
    /// Note that textures can be re-used across different families
    /// by using the <see cref="TextureCache.Get(string)"/> method of <see cref="TextureCache"/> along with their raw names. 
    /// </summary>
    enum TextureType
    {
        Icon = 0,
        Model = 1,
        Ui = 2,
        Terrain = 3,
    }

    static class TextureTypeExt
    {
        public static readonly string[] dirs = new[]
        {
            @"Icons",
            @"Units",
            @"Ui",
            @"Terrain",
        };

        public static string GetDirectory(this TextureType t)
        {
            return dirs[(int)t];
        }
    }
}
