using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// The type of a texture. 
    /// </summary>
    [Obsolete]
    public enum TextureType
    {
        Icon = 0,
        Model = 1,
        Ui = 2,
        Terrain = 3,
    }

    public static class TextureTypeExt
    {
        static readonly string[] dirs = 
        {
            @"icons",
            @"objects",
            @"ui",
            @"terrain",
        };

        [Obsolete]
        public static string GetDirectory(this TextureType t, string name)
        {
            return Util.ShanoPath.Combine(dirs[(int)t], name);
        }
    }
}
