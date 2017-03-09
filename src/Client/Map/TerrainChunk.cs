using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client;
using System.Threading;
using Shanism.Common;

using static Shanism.Common.Constants.Content;

namespace Shanism.Client.Map
{
    /// <summary>
    /// Contains the data for a chunk, including an array of the tiles 
    /// and a pointer to the VertexBuffer on the GPU where it is contained. 
    /// </summary>
    class TerrainChunk : IDisposable
    {
        readonly Texture2D texture;

        readonly TerrainType[] tiles;

        readonly VertexBuffer buffer;

        readonly VertexPositionTexture[] vertexData;


        public ChunkId Id { get; }

        public int Timestamp { get; }

        public bool HasBuffer { get; private set; }

        
        public VertexBuffer Buffer => buffer;

        public int Width => Constants.Client.ChunkSize;

        public int Height => Constants.Client.ChunkSize;

        public int Area => Width * Height;


        public TerrainChunk(GraphicsDevice device, ChunkId chunk, Texture2D tex, TerrainType[] msgTiles, Rectangle msgSpan)
        {
            this.texture = tex;
            vertexData = new VertexPositionTexture[6 * Area];

            Id = chunk;
            Timestamp = Environment.TickCount;

            buffer = new VertexBuffer(device, typeof(VertexPositionTexture), 6 * Area, BufferUsage.WriteOnly);
            tiles = new TerrainType[Constants.Client.ChunkSize * Constants.Client.ChunkSize];
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
            var intersect = Id.Span.IntersectWith(msgSpan);
            setTilesUnsafe(intersect, (_) => TerrainType.None);

            rebuildBuffer(intersect - Id.BottomLeft);
        }


        public void SetTiles(TerrainType[] msgTiles, Rectangle msgSpan)
        {
            var intersect = Id.Span.IntersectWith(msgSpan);
            setTilesUnsafe(intersect, (inGamePt) =>
            {
                var arrPt = inGamePt - msgSpan.Position;
                var arrPos = arrPt.X + msgSpan.Width * arrPt.Y;

                return msgTiles[arrPos];
            });

            rebuildBuffer(intersect - Id.BottomLeft);
        }

        void setTilesUnsafe(Rectangle span, Func<Point, TerrainType> func)
        {
            foreach (var inGamePt in span.Iterate())
            {
                var chunkPt = inGamePt - Id.BottomLeft;
                var chunkPos = chunkPt.X + Id.Span.Width * chunkPt.Y;
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
                    var ttyId = pt.X + pt.Y * Constants.Client.ChunkSize;
                    var srcRect = GetTileTextureBounds(tiles[ttyId]);
                    var destRect = new RectangleF(Id.BottomLeft + pt, Vector.One);
                    var offset = 6 * ttyId;
                    writePtData(offset, srcRect, destRect);
                }

                buffer.SetData(vertexData);
                HasBuffer = true;
            });
        }

        public RectangleF GetTileTextureBounds(TerrainType tty)
        {
            const double delta = 1e-5;

            var ttyId = (int)tty;
            var x = ttyId % TerrainFileSplitsX;
            var y = ttyId / TerrainFileSplitsX;
            var logicalSize = new Vector(TerrainFileSplitsX, TerrainFileSplitsY);

            return new RectangleF(x, y, 1, 1).Inflate(-delta) / logicalSize;
        }

        void writePtData(int offset, RectangleF sourceRectangle, RectangleF destRectangle)
        {
            vertexData[offset + 0] = genPoint(destRectangle.TopLeft, sourceRectangle.TopLeft);
            vertexData[offset + 1] = genPoint(destRectangle.TopRight, sourceRectangle.TopRight);
            vertexData[offset + 2] = genPoint(destRectangle.BottomLeft, sourceRectangle.BottomLeft);

            vertexData[offset + 3] = genPoint(destRectangle.BottomLeft, sourceRectangle.BottomLeft);
            vertexData[offset + 4] = genPoint(destRectangle.TopRight, sourceRectangle.TopRight);
            vertexData[offset + 5] = genPoint(destRectangle.BottomRight, sourceRectangle.BottomRight);
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
