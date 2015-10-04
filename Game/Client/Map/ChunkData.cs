﻿using Client.Sprites;
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
    class ChunkData : IDisposable
    {
        public readonly TerrainType[,] Tiles;

        public readonly int Timestamp;


        public readonly IO.Common.MapChunkId Chunk;

        volatile VertexBuffer buffer;
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

        public TerrainType GetTile(int x, int y)
        {
            return Tiles[x - Chunk.BottomLeft.X, y - Chunk.BottomLeft.Y];
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
        public void BuildBuffer(GraphicsDevice device, Func<int, int, TerrainType> getTerrain)
        {
            if (HasBuffer)
                throw new Exception("Dont call me twice!");

            ThreadPool.QueueUserWorkItem(o =>
                {
                    var vertexData = new List<VertexPositionTexture>();
                    var i = 0;
                    for(int x = 0; x < Width; x++)
                        for(int y = 0; y < Height; y++)
                        {
                            var mapTile = Tiles[x, y];
                            var sprite = SpriteFactory.Terrain.GetSprite(mapTile);
                            var pos = (Chunk.BottomLeft + new Vector(x , y)).ToVector2();
                            var sz = 1.01f;
                            var srcRect = sprite.SourceRectangle;


                            //vertexData.Add(genPoint(pos, sprite, -1, -1));

                            vertexData.Add(genPoint(pos.X, pos.Y, sprite.Points.TopLeft));
                            vertexData.Add(genPoint(pos.X + sz, pos.Y, sprite.Points.TopRight));
                            vertexData.Add(genPoint(pos.X, pos.Y + sz, sprite.Points.BottomLeft));

                            vertexData.Add(genPoint(pos.X, pos.Y + sz, sprite.Points.BottomLeft));
                            vertexData.Add(genPoint(pos.X + sz, pos.Y, sprite.Points.TopRight));
                            vertexData.Add(genPoint(pos.X + sz, pos.Y + sz, sprite.Points.BottomRight));

                            i++;
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
        VertexPositionTexture genPoint(double x, double y, Vector2 texPos)
        {
            return new VertexPositionTexture(new Vector3((float)x, (float)y, 0), texPos);
        }

        //VertexPositionTexture genPoint(Vector2 p, Sprite s, int dx, int dy)
        //{
        //    return new VertexPositionTexture(new Vector3(p.X, p.Y, 0), s.Points.Get(dx, dy));
        //}

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
