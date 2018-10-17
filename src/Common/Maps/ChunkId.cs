using Ix.Math;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Common
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
        public static readonly Point ChunkSize = new Point(Constants.Client.TerrainChunkSize);


        /// <summary>
        /// Gets the map id of the chunk. 
        /// </summary>
        [ProtoMember(1)]
        public readonly Point ID;


        /// <summary>
        /// Gets a rectangle spanning the chunk. 
        /// </summary>
        public Rectangle Bounds => new Rectangle(BottomLeft, ChunkSize);

        public Point Size => ChunkSize;

        /// <summary>
        /// Gets the bottom left point of the chunk. 
        /// </summary>
        public Point BottomLeft => ID * ChunkSize;

        /// <summary>
        /// Gets the top right point of the chunk. 
        /// </summary>
        public Point TopRight => (ID + 1) * ChunkSize;

        /// <summary>
        /// Gets the center of the chunk. 
        /// </summary>
        public Point Center => ID * ChunkSize + ChunkSize / 2;


        /// <summary>
        /// Creates a new MapChunk with the specified <see cref="ID"/>. 
        /// </summary>
        public ChunkId(int idX, int idY)
        {
            ID = new Point(idX, idY);
        }

        public ChunkId(Point id) { ID = id; }

        /// <summary>
        /// Returns the chunk that contains the given in-game point. 
        /// </summary>
        public static ChunkId ChunkOf(Vector2 pos)
            => new ChunkId((pos / ChunkSize).Floor());

        /// <summary>
        /// Returns the chunks inside the given in-game rectangle. 
        /// Returns the chunks containing the locations, too. 
        /// </summary>
        /// <param name="lowerLeft"></param>
        /// <param name="upperRight"></param>
        /// <returns></returns>
        public static IEnumerable<ChunkId> ChunksBetween(Vector2 lowerLeft, Vector2 upperRight)
        {
            var lowBin = ChunkOf(lowerLeft).ID;
            var hiBin = ChunkOf(upperRight).ID;

            for (int ix = lowBin.X; ix <= hiBin.X; ix++)
                for (int iy = lowBin.Y; iy <= hiBin.Y; iy++)
                    yield return new ChunkId(ix, iy);
        }

        public bool Equals(ChunkId other) => ID == other.ID;
        public static bool operator ==(ChunkId a, ChunkId b) => a.Equals(b);
        public static bool operator !=(ChunkId a, ChunkId b) => !a.Equals(b);

        public override bool Equals(object obj) => (obj is ChunkId other) && Equals(other);
        public override string ToString() => ID.ToString();
        public override int GetHashCode() => ID.GetHashCode();
    }
}
