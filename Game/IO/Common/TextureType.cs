﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
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

    [Obsolete]
    public static class TextureTypeExt
    {
        public static readonly string[] dirs = 
        {
            @"Icons",
            @"Objects",
            @"Ui",
            @"Terrain",
        };

        public static string GetDirectory(this TextureType t, string name)
        {
            return Path.Combine(dirs[(int)t], name);
        }
    }
}
