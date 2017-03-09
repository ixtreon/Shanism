using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Editor.Util
{
    class ImageUtils
    {
        public static readonly ISet<string> supportedExtensions = new HashSet<string>(new[]
        {
            ".JPG", ".JPEG", ".BMP", ".PNG"
        });


        public static bool IsValidTexture(string fileName) => supportedExtensions
                .Contains(ShanoPath.GetExtension(fileName).ToUpperInvariant());
    }
}
