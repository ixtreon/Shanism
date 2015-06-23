using Client.Sprites;
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

namespace Client.Map
{
    /// <summary>
    /// Contains the data for a chunk, including an array of the tiles and a pointer to the 
    /// </summary>
    class ChunkData : IDisposable
    {
        public readonly TerrainType[,] Tiles;

        public readonly int Timestamp;


        public readonly IO.Common.MapChunkId Chunk;

        private volatile VertexBuffer buffer;

        public VertexBuffer Buffer
        {
            get { return buffer; }
        }



        public bool HasBuffer { get; private set; }


        public int Width
        {
            get { return Tiles.GetLength(0); }
        }
        public int Height
        {
            get { return Tiles.GetLength(1); }
        }
        public int Area
        {
            get { return Width * Height; }
        }

        public ChunkData(MapChunkId chunk, TerrainType[,] tiles)
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
                throw new Exception("Dont call me twice!");

            ThreadPool.QueueUserWorkItem(o =>
                { 
                    var vertexData = new VertexPositionTexture[6 * Area];
                    var i = 0;
                    for(int x = 0; x < Width; x++)
                        for(int y = 0; y < Height; y++)
                        {
                            var mapTile = Tiles[x, y];
                            var sprite = SpriteFactory.Terrain.GetSprite(mapTile);
                            var pos = ((Vector)Chunk.BottomLeft + new Vector(x , y)).ToVector2();
                            var sz = 1.01f;
                            var tex = sprite.Texture;
                            var srcRect = sprite.SourceRectangle;
                            var texTopLeft = new Vector2((float)srcRect.Left / tex.Width, (float)srcRect.Top / tex.Height);
                            var texTopRight = new Vector2((float)srcRect.Right / tex.Width, (float)srcRect.Top / tex.Height);
                            var texBotLeft = new Vector2((float)srcRect.Left / tex.Width, (float)srcRect.Bottom / tex.Height);
                            var texBotRight = new Vector2((float)srcRect.Right / tex.Width, (float)srcRect.Bottom / tex.Height);

                            vertexData[6 * i + 0] = new VertexPositionTexture(
                                new Vector3(pos.X, pos.Y, 0),
                                texTopLeft);
                            vertexData[6 * i + 1] = new VertexPositionTexture(
                                new Vector3(pos.X + sz, pos.Y, 0),
                                texTopRight);
                            vertexData[6 * i + 2] = new VertexPositionTexture(
                                new Vector3(pos.X, pos.Y + sz, 0),
                                texBotLeft);

                            vertexData[6 * i + 3] = new VertexPositionTexture(
                                new Vector3(pos.X, pos.Y + sz, 0),
                                texBotLeft);
                            vertexData[6 * i + 4] = new VertexPositionTexture(
                                new Vector3(pos.X + sz, pos.Y, 0),
                                texTopRight);
                            vertexData[6 * i + 5] = new VertexPositionTexture(
                                new Vector3(pos.X + sz, pos.Y + sz, 0),
                                texBotRight);
                            i++;
                        }

                    buffer = new VertexBuffer(device, typeof(VertexPositionTexture), 6 * Area, BufferUsage.WriteOnly);
                    buffer.SetData(vertexData);
                    HasBuffer = true;
                });
        }

        public void Dispose()
        {
            if(HasBuffer)
            {
                HasBuffer = false;
                Buffer.Dispose();
            }
        }
    }
}
