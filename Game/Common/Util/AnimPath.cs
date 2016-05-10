using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Contains methods for handling paths inside animations. 
    /// Paths are Unix-like, always using a forward-slash for delimiter.  
    /// </summary>
    public static class AnimPath
    {
        static readonly string PathDelimiter = "/";
        static readonly char[] RecognizedDelimiters = new[] { '/', '\\' };

        /// <summary>
        /// Splits the path into segments. 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string[] SplitPath(string path)
        {
            return path.ToLower()
                .Split(RecognizedDelimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Combines the specified paths using the default path delimiter. 
        /// </summary>
        /// <param name="paths">The paths to combine.</param>
        /// <returns>A single path that is a concatenation of all the given paths. </returns>
        public static string Combine(IEnumerable<string> paths)
            => string.Join(PathDelimiter, paths.Select(p => p.ToLower().Trim(RecognizedDelimiters)));

        /// <summary>
        /// Combines the specified paths using the default path delimiter. 
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
        public static string GetPathFileName(string path)
            => SplitPath(path).Last();

        /// <summary>
        /// Gets a normalized version of this path. 
        /// </summary>
        /// <param name="path">The path to normalize. </param>
        public static string Normalize(string path)
            => Combine(SplitPath(path));

        /// <summary>
        /// Returns a path that points to the same resourse as the path in the first argument,
        /// but is rooted at the path specified by the second argument. 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string fullPath, string basePath)
        {
            var baseSegm = SplitPath(basePath);
            var relSegments = SplitPath(fullPath)
                .SkipWhile((s, i) => i < baseSegm.Length && s == baseSegm[i]);

            return Combine(relSegments);
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
    }
}
