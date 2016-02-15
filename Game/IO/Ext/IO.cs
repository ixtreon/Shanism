using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public static class IOExt
    {
        public static string GetRelativePath(this string absolutePath, string folder)
        {
            absolutePath = Path.GetFullPath(absolutePath);
            folder = Path.GetFullPath(folder);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.InvariantCulture))
                folder += Path.DirectorySeparatorChar;

            var pathUri = new Uri(absolutePath);
            var folderUri = new Uri(folder);

            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri)
                .ToString()
                .Replace('/', Path.DirectorySeparatorChar));
        }

        public static string RemoveFileExtension(this string absolutePath)
        {
            var id = absolutePath.IndexOf('.');
            if (id < 0)
                return absolutePath;
            return absolutePath.Substring(0, id);
        }
    }
}
