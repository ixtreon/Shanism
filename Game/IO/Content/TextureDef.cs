using IO.Common;
using IxSerializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Content
{
    /// <summary>
    /// Represents a texture content file used by the game. 
    /// Contains the name of the texture along with the logical splits that divide it into chunks. 
    /// </summary>
    [SerialKiller]
    public struct TextureDef
    {
        /// <summary>
        /// The number of segments in the file, if there is more than one. 
        /// </summary>
        [SerialMember]
        public readonly Point Splits;

        /// <summary>
        /// The name, also the path, to the file. 
        /// </summary>
        [SerialMember]
        public readonly string Name;

        /// <summary>
        /// Creates a new TextureDef for the given file containing multiple image segments. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        public TextureDef(string name, Point logicalSize)
        {
            this.Name = name;
            this.Splits = logicalSize;
        }

        /// <summary>
        /// Creates a new TextureDef for the given file containing multiple image segments. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        /// <param name="logicalWidth">The number of image segments along the horizontal. </param>
        /// <param name="logicalHeight">The number of image segments along the vertical. </param>
        public TextureDef(TextureType type, string name, Point logicalSize)
            : this(type.GetDirectory(name), logicalSize)
        { }



        /// <summary>
        /// Creates a new TextureDef for the given file of a single segment. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        public TextureDef(string name)
            : this(name, new Point(1))
        { }

        public TextureDef(TextureType type, string name)
            : this(type.GetDirectory(name), new Point(1))
        { }



        /// <summary>
        /// Creates a new TextureDef for the given file containing multiple image segments. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        /// <param name="logicalWidth">The number of image segments along the horizontal. </param>
        /// <param name="logicalHeight">The number of image segments along the vertical. </param>
        public TextureDef(string name, int logicalWidth, int logicalHeight)
            : this(name, new Point(logicalWidth, logicalHeight))
        { }

        /// <summary>
        /// Creates a new TextureDef for the given file containing multiple image segments. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        /// <param name="logicalWidth">The number of image segments along the horizontal. </param>
        /// <param name="logicalHeight">The number of image segments along the vertical. </param>
        public TextureDef(TextureType type, string name, int logicalWidth, int logicalHeight)
            : this(type.GetDirectory(name), new Point(logicalWidth, logicalHeight))
        { }
    }
}
