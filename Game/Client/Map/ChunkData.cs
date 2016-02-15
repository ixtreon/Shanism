using IO.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using System.Threading;
using IO;

namespace Client.Map
{
    /// <summary>
    /// Contains the data for a chunk, including an array of the tiles 
    /// and a pointer to the VertexBuffer on the GPU where it is contained. 
    /// </summary>
    [Obsolete]
    class ChunkData : IDisposable
    {
        public readonly TerrainType[] Tiles;

        public readonly int Timestamp;


        public readonly MapChunkId Chunk;

        volatile VertexBuffer buffer;
        public VertexBuffer Buffer
        {
            get { return buffer; }
        }



        public bool HasBuffer { get; private set; }


        public int Width
        {
            get { return Constants.Terrain.ChunkSize; }
        }

        public int Height
        {
            get { return Constants.Terrain.ChunkSize; }
        }

        public int Area
        {
            get { return Width * Height; }
        }

        public TerrainType GetTile(int x, int y)
        {
            return Tiles[(x - Chunk.BottomLeft.X) + Constants.Terrain.ChunkSize * (y - Chunk.BottomLeft.Y)];
        }

        public ChunkData(MapChunkId chunk, TerrainType[] tiles)
        {
            this.Chunk = chunk;
            this.Tiles = tiles;
            this.Timestamp = Environment.TickCount;
        }

        /// <summary>
        /// Builds the framebuffer for this chunk. Throws an exception if the framebuffer is already created. 
        /// </summary>
        /// <param name="device"></param>
        public void BuildBuffer(GraphicsDevice device)
        {
            if (HasBuffer)
                throw new Exception("Don't call me twice!");

            ThreadPool.QueueUserWorkItem(o =>
            {
                var vertexData = new List<VertexPositionTexture>();

                foreach (var x in Enumerable.Range(0, Width))
                    foreach (var y in Enumerable.Range(0, Height))
                    {
                        var pi = Content.Terrain.GetTile(Tiles[x + y * Constants.Terrain.ChunkSize]);
                        var pos = Chunk.BottomLeft + new Vector(Chunk.Span.Width * x / Width, Chunk.Span.Height * y / Height);
                        var _tileFar = 1;
                        var _tileClose = 0;

                        vertexData.Add(genPoint(pos.X + _tileClose, pos.Y + _tileClose, pi.TopLeft));
                        vertexData.Add(genPoint(pos.X + _tileFar, pos.Y + _tileClose, pi.TopRight));
                        vertexData.Add(genPoint(pos.X + _tileClose, pos.Y + _tileFar, pi.BottomLeft));

                        vertexData.Add(genPoint(pos.X + _tileClose, pos.Y + _tileFar, pi.BottomLeft));
                        vertexData.Add(genPoint(pos.X + _tileFar, pos.Y + _tileClose, pi.TopRight));
                        vertexData.Add(genPoint(pos.X + _tileFar, pos.Y + _tileFar, pi.BottomRight));
                    }

                buffer = new VertexBuffer(device, typeof(VertexPositionTexture), 6 * Area, BufferUsage.WriteOnly);
                buffer.SetData(vertexData.ToArray());
                HasBuffer = true;
            });
        }


        /// <summary>
        /// Checks a corner for nice tiling. NYI
        /// </summary>
        void checkCorner(int x, int y, int dx, int dy, Func<int, int, TerrainType> func, List<VertexPositionTexture> list)
        {
            //var thisTile = func(x, y);
            //var tiles = new[]
            //{
            //    func(x + dx, y),
            //    func(x, y + dx),
            //    func(x + dx, y + dx),
            //};

            //if(tiles[0] == tiles[1] && tiles[1] == tiles[2] && tiles[0] != thisTile)
            //{
            //    var sprite = SpriteFactory.Terrain.GetSprite(tiles[0]);
            //}
            //if (n_same == 0)
            //{
            //    var loc = new Vector(x + 0.5, y + 0.5);
            //    list.Add(genPoint())
            //}
        }

        /// <summary>
        /// Returns a VertexPositionTexture for the given in-texture point 
        /// which is at the provided in-game x/y co-ordinates. 
        /// </summary>
        VertexPositionTexture genPoint(double x, double y, Vector texPos)
        {
            return new VertexPositionTexture(new Vector3((float)x, (float)y, 0), texPos.ToXnaVector());
        }

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
