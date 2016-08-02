using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Contains methods for handling paths inside animations and textures.
    /// Paths are Unix-like, always using a forward-slash ('/') for delimiter.  
    /// </summary>
    public static class ShanoPath
    {
        public const char PathDelimiter = '/';
        const char AltPathDelimiter = '\\';

        public static readonly char[] RecognizedDelimiters = { PathDelimiter, AltPathDelimiter };

        /// <summary>
        /// Splits the path into segments. 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string[] SplitPath(string path)
        {
            path = Normalize(path);
            if (string.IsNullOrEmpty(path))
                return new string[0];

            return path.Split(PathDelimiter);
        }

        /// <summary>
        /// Combines the specified paths using the default path delimiter. 
        /// Returns a path in normal form.
        /// </summary>
        /// <param name="paths">The paths to combine.</param>
        /// <returns>A single path that is a concatenation of all the given paths. </returns>

        public static string Combine(IEnumerable<string> paths)
        {
            var sb = new StringBuilder();
            foreach (var p in paths)
            {
                var fp = Normalize(p);

                if (fp.Length > 0)
                {
                    sb.Append(fp);
                    sb.Append(PathDelimiter);
                }
            }
            if(sb.Length > 0)
                sb.Length--;
            return sb.ToString();
        }

        /// <summary>
        /// Gets a normalized version of this path. 
        /// </summary>
        /// <param name="path">The path to normalize. </param>
        public static string Normalize(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            return path
                .ToLowerInvariant()
                .Replace(AltPathDelimiter, PathDelimiter)
                .Trim(PathDelimiter);
        }
        /// <summary>
        /// Combines the specified paths using the default path delimiter. 
        /// Returns a path in normal form.
        /// </summary>
        /// <param name="paths">The paths to combine.</param>
        /// <returns>A single path that is a concatenation of all the given paths. </returns>
        public static string Combine(params string[] paths)
            => Combine((IEnumerable<string>)paths);

        /// <summary>
        /// Returns the last segment of this path. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLastSegment(string path)
        {
            var id = path.LastIndexOfAny(RecognizedDelimiters);

            if (id < 0)     //that's a raw file
                return path;

            if (id + 1 >= path.Length)  //that's a dir
                return string.Empty;

            return path.Substring(id + 1);
        }

        /// <summary>
        /// Removes the last segment from this path.
        /// </summary>
        public static string GetDirectory(string path)
        {
            var id = path.LastIndexOfAny(RecognizedDelimiters);
            if (id < 0)     //a root directory
                return string.Empty;

            return path.Substring(0, id);
        }



        /// <summary>
        /// Returns a path that points to the same resourse as the path in the first argument,
        /// but is rooted at the path specified by the second argument. 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string fullPath, string basePath)
        {
            return fullPath.GetRelativePath(basePath);
        }

        /// <summary>
        /// Determines whether the path in the first argument is a sub-folder of the path in the second argument.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="basePath">The base path.</param>
        public static bool IsSubFolderOf(string fullPath, string basePath)
        {
            var fSegm = SplitPath(fullPath);
            var bSegm = SplitPath(basePath);
            return fSegm.Length >= bSegm.Length
                && bSegm.Zip(fSegm, string.Equals).All(b => b);
        }

        /// <summary>
        /// Returned whether the two supplied paths are logically equivalent. 
        /// </summary>
        /// <param name="pathA">The first path.</param>
        /// <param name="pathB">The second path.</param>
        /// <returns></returns>
        public static bool Equals(string pathA, string pathB)
        {
            var aSegm = SplitPath(pathA);
            var bSegm = SplitPath(pathB);
            return aSegm.Length == bSegm.Length
                && aSegm.Zip(bSegm, string.Equals).All(b => b);
        }

        public static string RemoveExtension(string path)
        {
            var id = path.LastIndexOf('.');
            if (id < 0)
                return path;
            return path.Substring(0, id);
        }

        public static string GetExtension(string path)
        {
            var id = path.LastIndexOf('.');
            if (id < 0)
                return string.Empty;
            return path.Substring(id);
        }
    }
}
