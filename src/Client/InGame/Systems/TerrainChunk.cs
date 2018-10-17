using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using System;
using System.Numerics;
using System.Threading;
using static Shanism.Common.Constants.Terrain;

namespace Shanism.Client.Terrain
{
    /// <summary>
    /// Contains the data for a chunk, including an array of the tiles 
    /// and a pointer to the VertexBuffer on the GPU where it is contained. 
    /// </summary>
    public class TerrainChunk : IDisposable
    {

        readonly TerrainType[] tiles;
        readonly VertexPositionTexture[] vertexData;


        public ChunkId Id { get; }

        public int Timestamp { get; }

        public VertexBuffer Buffer { get; }

        public bool HasBuffer { get; private set; }



        public int Width => Constants.Client.TerrainChunkSize;

        public int Height => Constants.Client.TerrainChunkSize;

        public int Area => Width * Height;


        public TerrainChunk(GraphicsDevice device, ChunkId chunk, TerrainType[] msgTiles, Rectangle msgSpan)
        {
            vertexData = new VertexPositionTexture[6 * Area];

            Id = chunk;
            Timestamp = Environment.TickCount;

            Buffer = new VertexBuffer(device, typeof(VertexPositionTexture), 6 * Area, BufferUsage.WriteOnly);
            tiles = new TerrainType[Constants.Client.TerrainChunkSize * Constants.Client.TerrainChunkSize];
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
            var intersect = Id.Bounds.Intersect(msgSpan);
            setTilesUnsafe(intersect, (_) => TerrainType.None);

            rebuildBuffer(intersect - Id.BottomLeft);
        }


        public void SetTiles(TerrainType[] msgTiles, Rectangle msgSpan)
        {
            var intersect = Id.Bounds.Intersect(msgSpan);
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
                var chunkPos = chunkPt.X + Id.Bounds.Width * chunkPt.Y;
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
                    var ttyId = pt.X + pt.Y * Constants.Client.TerrainChunkSize;
                    var srcRect = GetTileTextureBounds(tiles[ttyId]);
                    var destRect = new RectangleF(Id.BottomLeft + pt, Vector2.One);
                    var offset = 6 * ttyId;
                    writePtData(offset, srcRect, destRect);
                }

                Buffer.SetData(vertexData);
                HasBuffer = true;
            });
        }

        public RectangleF GetTileTextureBounds(TerrainType tty)
        {
            const float delta = 1e-5f;

            var ttyId = (int)tty;
            var x = ttyId % LogicalSize.X;
            var y = ttyId / LogicalSize.X;

            return new RectangleF(x, y, 1, 1).Inflate(-delta) / LogicalSize;
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
        VertexPositionTexture genPoint(Vector2 inGamePos, Vector2 texPos)
            => new VertexPositionTexture(inGamePos.ToXnaVector3(), texPos.ToXnaVector());


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
