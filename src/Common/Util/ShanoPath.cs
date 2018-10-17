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
        const char FileExtensionDelimiter = '.';
        const char PathDelimiter = '/';
        const char AltPathDelimiter = '\\';

        /// <summary>
        /// The list of all recognized path segment delimiters.
        /// </summary>
        public static readonly char[] RecognizedDelimiters = { PathDelimiter, AltPathDelimiter };


        /// <summary>
        /// Gets a normalized version of this path. 
        /// </summary>
        /// <param name="path">The path to normalize. </param>
        static string Normalize(string path)
        {
            if(string.IsNullOrEmpty(path))
                return string.Empty;

            return path
                .ToLowerInvariant()
                .Replace(AltPathDelimiter, PathDelimiter)
                .TrimEnd(PathDelimiter);
        }

        public static string NormalizeTexture(string texName)
            => Normalize(RemoveExtension(texName));

        public static string NormalizeAnimation(string animName)
            => Normalize(animName);



        /// <summary>
        /// Splits the path into segments. 
        /// </summary>
        public static string[] SplitPath(string path)
        {
            if(string.IsNullOrEmpty(path))
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
            => string.Join(PathDelimiter.ToString(), paths);

        /// <summary>
        /// Combines the specified paths using the default path delimiter. 
        /// Returns a path in normal form.
        /// </summary>
        /// <param name="paths">The paths to combine.</param>
        /// <returns>A single path that is a concatenation of all the given paths. </returns>
        public static string Combine(params string[] paths)
            => string.Join(PathDelimiter.ToString(), paths);


        /// <summary>
        /// Returns the last segment of this path. 
        /// e.g. "a/b/c/d" -> "d"
        /// </summary>
        public static string GetLastSegment(string path)
        {
            var id = path.LastIndexOfAny(RecognizedDelimiters);

            if(id < 0)     //that's a raw file
                return path;

            return path.Substring(id + 1);
        }

        /// <summary>
        /// Removes the last segment from this path.
        /// e.g. "a/b/c/d" -> "a/b/c"
        /// </summary>
        public static string GetDirectoryPath(string path)
        {
            var id = path.LastIndexOfAny(RecognizedDelimiters);
            if(id < 0)     //a root directory
                return string.Empty;

            return path.Substring(0, id);
        }

        /// <summary>
        /// Gets the root directory of this path.
        /// e.g. "a/b/c/d" -> "a"
        /// </summary>
        public static string GetRootDirectory(string path)
        {
            var id = path.IndexOfAny(RecognizedDelimiters);
            if (id < 0)     //a root directory
                return string.Empty;

            return path.Substring(0, id);
        }


        /// <summary>
        /// Returns a path that points to the same resourse as the path in the first argument,
        /// but is rooted at the path specified by the second argument. 
        /// </summary>
        /// <param name="toPath"></param>
        /// <param name="fromPath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string fromPath, string toPath)
        {
            var from = SplitPath(fromPath);
            var to = SplitPath(toPath);

            var start = 0;
            var end = Math.Min(from.Length, to.Length);
            while(start < end && to[start].Equals(from[start], StringComparison.Ordinal))
                start++;

            var sb = new StringBuilder();

            for(var i = start; i < from.Length; i++)
            {
                sb.Append("..");
                sb.Append('/');
            }

            for(int i = start; i < to.Length; i++)
            {
                sb.Append(to[i]);
                sb.Append('/');
            }

            return sb.ToString(0, sb.Length - 1);
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

        public static string RemoveExtension(string path)
            => path.Substring(0, findExtensionStart(path));

        public static string GetExtension(string path)
            => path.Substring(findExtensionStart(path));

        static int findExtensionStart(string path)
        {

            var lastDotPos = path.LastIndexOf(FileExtensionDelimiter);
            if(lastDotPos < 0)
                return path.Length;

            var lastDelimiter = path.LastIndexOfAny(RecognizedDelimiters);
            if(lastDotPos < lastDelimiter)
                return path.Length;

            return lastDotPos;
        }

    }
}
