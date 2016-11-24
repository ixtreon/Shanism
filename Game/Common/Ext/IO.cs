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
        public static string GetRelativePath(this string absolutePath, string folder, bool toLower = true)
        {
            var absSegments = ShanoPath.SplitPath(absolutePath, toLower);
            var dirSegments = ShanoPath.SplitPath(folder, toLower);
            var commonPrefix = getCommonPrefix(absSegments, dirSegments);

            var sb = new StringBuilder();

            for (var i = commonPrefix; i < dirSegments.Length; i++)
            {
                sb.Append("..");
                sb.Append('/');
            }

            sb.Append(string.Join("/", absSegments, commonPrefix, absSegments.Length - commonPrefix));

            var retVal = sb.ToString();
            return retVal;
        }

        static int getCommonPrefix(string[] absSegments, string[] dirSegments)
        {
            var maxPrefix = Math.Min(absSegments.Length, dirSegments.Length);
            var commonPrefix = 0;
            while (commonPrefix < maxPrefix
                && absSegments[commonPrefix].Equals(dirSegments[commonPrefix]))
                commonPrefix++;
            return commonPrefix;
        }
    }
}
