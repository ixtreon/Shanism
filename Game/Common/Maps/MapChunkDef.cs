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
    public struct MapChunkId : IEquatable<MapChunkId>
    {
        /// <summary>
        /// Gets the size of a chunk. 
        /// </summary>
        public static readonly Point ChunkSize = new Point(Constants.Terrain.ChunkSize);


        /// <summary>
        /// Gets the map id of the chunk. 
        /// </summary>
        [ProtoMember(1)]
        public readonly Point ChunkId;


        /// <summary>
        /// Gets a rectangle spanning the chunk. 
        /// </summary>
        public Rectangle Span => new Rectangle(BottomLeft, ChunkSize);

        /// <summary>
        /// Gets the bottom left point of the chunk. 
        /// </summary>
        public Point BottomLeft => ChunkId * ChunkSize;

        /// <summary>
        /// Gets the top right point of the chunk. 
        /// </summary>
        public Point TopRight => (ChunkId + 1) * ChunkSize;

        /// <summary>
        /// Gets the center of the chunk. 
        /// </summary>
        public Point Center => ChunkId * ChunkSize + ChunkSize / 2;


        /// <summary>
        /// Creates a new MapChunk with the specified <see cref="ChunkId"/>. 
        /// </summary>
        public MapChunkId(int idX, int idY)
        {
            ChunkId = new Point(idX, idY);
        }

        /// <summary>
        /// Returns the chunk that contains the given in-game point. 
        /// </summary>
        public static MapChunkId ChunkOf(Vector pos)
        {
            var x = (int)Math.Floor(pos.X / ChunkSize.X);
            var y = (int)Math.Floor(pos.Y / ChunkSize.Y);
            return new MapChunkId(x, y);
        }

        /// <summary>
        /// Returns the chunks inside the given in-game rectangle. 
        /// Returns the chunks containing the locations, too. 
        /// </summary>
        /// <param name="lowerLeft"></param>
        /// <param name="upperRight"></param>
        /// <returns></returns>
        public static IEnumerable<MapChunkId> ChunksBetween(Vector lowerLeft, Vector upperRight)
        {
            var lowBin = ChunkOf(lowerLeft).ChunkId;
            var hiBin = ChunkOf(upperRight).ChunkId;

            for (int ix = lowBin.X; ix <= hiBin.X; ix++)
                for (int iy = lowBin.Y; iy <= hiBin.Y; iy++)
                    yield return new MapChunkId(ix, iy);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => ChunkId.ToString();

        public static bool operator ==(MapChunkId a, MapChunkId b)
        {
            return a.ChunkId == b.ChunkId;
        }

        public static bool operator !=(MapChunkId a, MapChunkId b)
        {
            return a.ChunkId != b.ChunkId;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return ChunkId.GetHashCode();
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
            if (!(obj is MapChunkId))
                return false;
            return (MapChunkId)obj == this;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(MapChunkId other)
        {
            return this == other;
        }
    }
}
