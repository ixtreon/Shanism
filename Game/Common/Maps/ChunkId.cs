using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Game
{
    /// <summary>
    /// A map chunk identifier. Wrapper for a point. 
    /// </summary>
    [ProtoContract]
    public struct ChunkId : IEquatable<ChunkId>
    {
        /// <summary>
        /// Gets the size of a chunk. 
        /// </summary>
        public static readonly Point ChunkSize = new Point(Constants.Client.ChunkSize);


        /// <summary>
        /// Gets the map id of the chunk. 
        /// </summary>
        [ProtoMember(1)]
        public readonly Point Id;


        /// <summary>
        /// Gets a rectangle spanning the chunk. 
        /// </summary>
        public Rectangle Span => new Rectangle(BottomLeft, ChunkSize);

        /// <summary>
        /// Gets the bottom left point of the chunk. 
        /// </summary>
        public Point BottomLeft => Id * ChunkSize;

        /// <summary>
        /// Gets the top right point of the chunk. 
        /// </summary>
        public Point TopRight => (Id + 1) * ChunkSize;

        /// <summary>
        /// Gets the center of the chunk. 
        /// </summary>
        public Point Center => Id * ChunkSize + ChunkSize / 2;


        /// <summary>
        /// Creates a new MapChunk with the specified <see cref="Id"/>. 
        /// </summary>
        public ChunkId(int idX, int idY)
        {
            Id = new Point(idX, idY);
        }

        /// <summary>
        /// Returns the chunk that contains the given in-game point. 
        /// </summary>
        public static ChunkId ChunkOf(Vector pos)
        {
            var x = (int)Math.Floor(pos.X / ChunkSize.X);
            var y = (int)Math.Floor(pos.Y / ChunkSize.Y);
            return new ChunkId(x, y);
        }

        /// <summary>
        /// Returns the chunks inside the given in-game rectangle. 
        /// Returns the chunks containing the locations, too. 
        /// </summary>
        /// <param name="lowerLeft"></param>
        /// <param name="upperRight"></param>
        /// <returns></returns>
        public static IEnumerable<ChunkId> ChunksBetween(Vector lowerLeft, Vector upperRight)
        {
            var lowBin = ChunkOf(lowerLeft).Id;
            var hiBin = ChunkOf(upperRight).Id;

            for (int ix = lowBin.X; ix <= hiBin.X; ix++)
                for (int iy = lowBin.Y; iy <= hiBin.Y; iy++)
                    yield return new ChunkId(ix, iy);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => Id.ToString();

        /// <summary>
        /// Checks whether the two chunks have the same IDs. 
        /// </summary>
        public static bool operator ==(ChunkId a, ChunkId b)
        {
            return a.Id == b.Id;
        }

        /// <summary>
        /// Checks whether the two chunks have the different IDs. 
        /// </summary>
        public static bool operator !=(ChunkId a, ChunkId b)
        {
            return a.Id != b.Id;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ChunkId))
                return false;
            return (ChunkId)obj == this;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ChunkId other)
        {
            return this == other;
        }
    }
}
