using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    public static class IOExt
    {
        [Obsolete]
        public static string GetRelativePath(this string toPath, string fromPath, bool toLower = true)
        {
            return ShanoPath.GetRelativePath(fromPath, toPath);
        }
    }
}
