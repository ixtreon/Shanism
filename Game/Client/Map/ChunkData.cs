using Shanism.Common.Game;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client;
using System.Threading;
using Shanism.Common;

namespace Shanism.Client.Map
{
    /// <summary>
    /// Contains the data for a chunk, including an array of the tiles 
    /// and a pointer to the VertexBuffer on the GPU where it is contained. 
    /// </summary>
    class ChunkData : IDisposable
    {
        readonly TerrainType[] tiles;

        readonly VertexBuffer buffer;

        readonly VertexPositionTexture[] vertexData;


        public MapChunkId Chunk { get; }

        public int Timestamp { get; }

        public bool HasBuffer { get; private set; }

        
        public VertexBuffer Buffer => buffer;

        public int Width => Constants.Terrain.ChunkSize;

        public int Height => Constants.Terrain.ChunkSize;

        public int Area => Width * Height;


        public ChunkData(GraphicsDevice device, MapChunkId chunk, TerrainType[] msgTiles, Rectangle msgSpan)
        {
            vertexData = new VertexPositionTexture[6 * Area];

            Chunk = chunk;
            Timestamp = Environment.TickCount;

            buffer = new VertexBuffer(device, typeof(VertexPositionTexture), 6 * Area, BufferUsage.WriteOnly);
            tiles = new TerrainType[Constants.Terrain.ChunkSize * Constants.Terrain.ChunkSize];
            ClearTiles(false);

            SetTiles(msgTiles, msgSpan);
        }

        public void ClearTiles(bool toRebuildBuffer = true)
        {
            for (var i = 0; i < tiles.Length; i++)
                tiles[i] = TerrainType.None;

            if(toRebuildBuffer)
                rebuildBuffer();
        }

        public void ClearTiles(Rectangle msgSpan)
        {
            var intersect = Chunk.Span.IntersectWith(msgSpan);
            setTilesUnsafe(intersect, (_) => TerrainType.None);

            rebuildBuffer(intersect - Chunk.BottomLeft);
        }


        public void SetTiles(TerrainType[] msgTiles, Rectangle msgSpan)
        {
            var intersect = Chunk.Span.IntersectWith(msgSpan);
            setTilesUnsafe(intersect, (inGamePt) =>
            {
                var arrPt = inGamePt - msgSpan.Position;
                var arrPos = arrPt.X + msgSpan.Width * arrPt.Y;

                return msgTiles[arrPos];
            });

            rebuildBuffer(intersect - Chunk.BottomLeft);
        }

        void setTilesUnsafe(Rectangle span, Func<Point, TerrainType> func)
        {
            foreach (var inGamePt in span.Iterate())
            {
                var chunkPt = inGamePt - Chunk.BottomLeft;
                var chunkPos = chunkPt.X + Chunk.Span.Width * chunkPt.Y;
                tiles[chunkPos] = func(inGamePt);
            }
        }

        /// <summary>
        /// Builds the framebuffer for this chunk. Throws an exception if the framebuffer is already created. 
        /// </summary>
        /// <param name="rebuildRect">The rectangle to rebuild local to the chunk. Defaults to (0, 0, w, h). </param>
        void rebuildBuffer(Rectangle? rebuildRect = null)
        {
            var r = rebuildRect ?? new Rectangle(0, 0, Width, Height);
            ThreadPool.QueueUserWorkItem(o =>
            {
                foreach (var pt in r.Iterate())
                {
                    var ttyId = pt.X + pt.Y * Constants.Terrain.ChunkSize;
                    var ttyTexPos = Content.Terrain.GetTileTextureBounds(tiles[ttyId]);
                    var ttyGamePos = new RectangleF(Chunk.BottomLeft + pt, Vector.One);

                    vertexData[6 * ttyId + 0] = genPoint(ttyGamePos.BottomLeft, ttyTexPos.BottomLeft);
                    vertexData[6 * ttyId + 1] = genPoint(ttyGamePos.BottomRight, ttyTexPos.BottomRight);
                    vertexData[6 * ttyId + 2] = genPoint(ttyGamePos.TopLeft, ttyTexPos.TopLeft);

                    vertexData[6 * ttyId + 3] = genPoint(ttyGamePos.TopLeft, ttyTexPos.TopLeft);
                    vertexData[6 * ttyId + 4] = genPoint(ttyGamePos.BottomRight, ttyTexPos.BottomRight);
                    vertexData[6 * ttyId + 5] = genPoint(ttyGamePos.TopRight, ttyTexPos.TopRight);
                }

                buffer.SetData(vertexData);
                HasBuffer = true;
            });
        }

        /// <summary>
        /// Returns a VertexPositionTexture for the given in-texture point 
        /// which is at the provided in-game x/y co-ordinates. 
        /// </summary>
        VertexPositionTexture genPoint(Vector inGamePos, Vector texPos)
            => new VertexPositionTexture(inGamePos.ToVector3(), texPos.ToVector2());


        public void Dispose()
        {
            if (HasBuffer)
            {
                HasBuffer = false;
                Buffer.Dispose();
            }
        }
    }
}
