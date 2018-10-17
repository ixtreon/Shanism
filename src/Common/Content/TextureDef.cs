using Ix.Math;

namespace Shanism.Common.Content
{
    /// <summary>
    /// Represents a texture content file used by the game. 
    /// Contains the name of the texture along with the logical splits that divide it into chunks. 
    /// </summary>
    public class TextureDef
    {

        /// <summary>
        /// The number of logical divisions in the file, if there is more than one such segment.  
        /// </summary>
        public Point Cells { get; set; }

        /// <summary>
        /// The name, also the path, to the file behind this texture. 
        /// </summary>
        public string Name { get; set; }


        TextureDef() { }

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
        /// <param name="name">The name of (also relative path to) the file. </param>
        /// <param name="logicalSize">The number of logical divisions of this texture, in case it is an atlas. </param>
        public TextureDef(string name, Point logicalSize)
        {
            Name = name;
            Cells = logicalSize;
        }


        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
            => (obj is TextureDef other) 
            && other.Name == Name;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
            => Name.GetHashCode();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() 
            => $"{Name}";
    }
}
