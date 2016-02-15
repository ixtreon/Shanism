using IO.Common;
using ProtoBuf;
using Newtonsoft.Json;
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
    [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
    public class TextureDef
    {
        /// <summary>
        /// A placeholder texture that is present in all games. 
        /// </summary>
        public static readonly TextureDef Default = new TextureDef(Constants.Content.DefaultValues.ModelTexture);

        /// <summary>
        /// The number of logical divisions in the file, if there is more than one such segment.  
        /// </summary>
        [JsonProperty]
        public Point Splits;

        /// <summary>
        /// The name, also the path, to the file behind this texture. 
        /// </summary>
        [JsonProperty]
        public readonly string Name;

        /// <summary>
        /// Gets the total number of segments in this texture. 
        /// </summary>
        public int SegmentsCount {  get { return Splits.X * Splits.Y; } }

        TextureDef() { }

        /// <summary>
        /// Creates a new TextureDef for the given file containing multiple image segments. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        public TextureDef(string name, Point logicalSize)
        {
            Name = name;
            Splits = logicalSize;
        }



        /// <summary>
        /// Creates a new TextureDef for the given file. 
        /// </summary>
        /// <param name="name">The name (or path) of the file. </param>
        public TextureDef(string name)
            : this(name, new Point(1))
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


        public override bool Equals(object obj)
        {
            if (!(obj is TextureDef))
                return false;
            return ((TextureDef)obj).Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
