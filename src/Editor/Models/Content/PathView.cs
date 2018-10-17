using System;

namespace Shanism.Editor.Models.Content
{
    struct PathView
    {
        const char PathDelimiter = '/';
        static readonly char[] PathTrimChars = { PathDelimiter };

        readonly string[] segments;
        readonly int index;

        public PathView(string fullPath)
        {
            segments = fullPath.Split(PathTrimChars, StringSplitOptions.RemoveEmptyEntries);
            index = 0;
        }

        PathView(string[] segments, int index)
        {
            this.segments = segments;
            this.index = index;
        }

        public string First => (index == segments.Length) ? null : segments[index];
        public bool HasRest => index + 1 < segments.Length;
        public PathView Rest => new PathView(segments, index + 1);
    }
}
