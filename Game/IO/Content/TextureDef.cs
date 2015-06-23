using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Content
{
    /// <summary>
    /// Represents a texture content file used by the game. 
    /// </summary>
    public class TextureDef
    {
        /// <summary>
        /// The number of segments in the file, if there is more than one. 
        /// </summary>
        public readonly Point Splits;

        /// <summary>
        /// The name, also the path, to the file. 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Creates a new TextureDef for the given file of a single segment. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        public TextureDef(string name)
        {
            this.Name = name;
            this.Splits = new Point(1);
        }

        /// <summary>
        /// Creates a new TextureDef for the given file containing multiple image segments. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        /// <param name="width">The number of image segments along the horizontal. </param>
        /// <param name="height">The number of image segments along the vertical. </param>
        public TextureDef(string name, int width, int height)
        {
            this.Name = name;
            this.Splits = new Point(width, height);
        }
    }
}
