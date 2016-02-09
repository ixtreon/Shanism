using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Common
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
        /// Gets a rectangle spanning the whole chunk. 
        /// </summary>
        public Rectangle Span
        {
            get { return new Rectangle(BottomLeft, ChunkSize); }
        }

        public Point BottomLeft
        {
            get { return ChunkId * ChunkSize; }
        }

        public Point TopRight
        {
            get { return ChunkId * ChunkSize + ChunkSize; }
        }

        public Point Center
        {
            get { return ChunkId * ChunkSize + ChunkSize / 2; }
        }


        /// <summary>
        /// Creates a new MapChunk with the specified <see cref="ChunkId"/>. 
        /// </summary>
        public MapChunkId(int idX, int idY)
        {
            ChunkId = new Point(idX, idY);
        }


        public static MapChunkId ChunkOf(Vector pos)
        {
            int x = (int)Math.Floor(pos.X / ChunkSize.X);
            int y = (int)Math.Floor(pos.Y / ChunkSize.Y);
            return new MapChunkId(x, y);
        }

        public static IEnumerable<MapChunkId> ChunksBetween(Vector lowerLeft, Vector upperRight)
        {
            var lowBin = ChunkOf(lowerLeft).ChunkId - new Point(1);
            var hiBin = ChunkOf(upperRight).ChunkId + new Point(1);

            for (int ix = lowBin.X; ix <= hiBin.X; ix++)
                for (int iy = lowBin.Y; iy <= hiBin.Y; iy++)
                    yield return new MapChunkId(ix, iy);
        }

        public override string ToString()
        {
            return "{0}, {1}".F(ChunkId.X, ChunkId.Y);
        }

        public static bool operator ==(MapChunkId a, MapChunkId b)
        {
            return a.ChunkId == b.ChunkId;
        }
        public static bool operator !=(MapChunkId a, MapChunkId b)
        {
            return a.ChunkId != b.ChunkId;
        }

        public override int GetHashCode()
        {
            return ChunkId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MapChunkId))
                return false;
            return (MapChunkId)obj == this;
        }

        public bool Equals(MapChunkId other)
        {
            return this == other;
        }
    }
}
