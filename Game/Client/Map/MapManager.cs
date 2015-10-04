using Client;
using Client.Sprites;
using IO;
using IO.Common;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Map
{
    /// <summary>
    /// Handles map communication with the server. 
    /// </summary>
    class MapManager
    {
        /// <summary>
        /// The time before an uncompleted chunk request is re-sent. 
        /// </summary>
        const int SpamInterval = 5000;

        /// <summary>
        /// The maximum number of chunks to keep in memory. 
        /// </summary>
        const int MaxChunks = 100;

        /// <summary>
        /// The size of each chunk in game units. 
        /// </summary>
        public const int ChunkSize = Constants.Client.WindowHeight / 2;

        /// <summary>
        /// The server instance to query for maps. 
        /// </summary>
        protected readonly IGameReceptor server;

        /// <summary>
        /// Contains a map of all available chunks. 
        /// </summary>
        ConcurrentDictionary<MapChunkId, ChunkData> ChunksAvailable = new ConcurrentDictionary<MapChunkId, ChunkData>();

        /// <summary>
        /// Contains a map of all chunk requests made so far. 
        /// </summary>
        Dictionary<MapChunkId, long> chunkRequests = new Dictionary<MapChunkId, long>();

        public MapManager(IGameReceptor serv, GraphicsDevice device)
        {
            this.server = serv;
            serv.ChunkReceived += serv_TilesReceived; 
            effect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
            };
        }

        void serv_TilesReceived(MapChunkId chunk, TerrainType[,] tiles)
        {
            var rect = chunk.Span;

            var chunkData = new ChunkData(chunk, tiles);

            ChunksAvailable[chunkData.Chunk] = chunkData;
            chunkData.BuildBuffer(effect.GraphicsDevice, GetTile);
            RemoveOldChunks();
        }

        public TerrainType GetTile(int x, int y)
        {
            var id = MapChunkId.ChunkOf(new Vector(x, y) + new Vector(0.5));
            var chunk = GetChunk(id);
            return chunk?.GetTile(x, y) ?? TerrainType.None;
        }

        public void Update()
        {
            //request nearby chunks
            foreach (var c in EnumerateNearbyChunks().Except(ChunksAvailable.Keys))
                requestChunk(c);
        }

        public IEnumerable<MapChunkId> EnumerateNearbyChunks()
        {
            var cameraPos = server.CameraPosition;


            var lowLeft = cameraPos - (Vector)Constants.Client.WindowSize / 2;
            var upRight = cameraPos + (Vector)Constants.Client.WindowSize / 2;

            var chunks = MapChunkId.ChunksBetween(lowLeft, upRight).ToArray();
            return chunks;
        }

        /// <summary>
        /// Sends a request to the server for the given chunk. 
        /// </summary>
        /// <param name="chunkId"></param>
        void requestChunk(MapChunkId chunk)
        {
            if (ChunksAvailable.ContainsKey(chunk))
                return;

            //get last request timestamp
            long lastRequest = 0;
            chunkRequests.TryGetValue(chunk, out lastRequest);

            //make sure we don't spam the server
            long timeNow = Environment.TickCount;
            if (timeNow - lastRequest < SpamInterval)
                return;

            //make the request and set last timestamp
            chunkRequests[chunk] = timeNow;
            server.RequestChunk(chunk);
        }

        /// <summary>
        /// Tries to get the Chunk with the given id, or returns null if it is not available. 
        /// </summary>
        /// <param name="chunkId">The id of the chunk as a Point. </param>
        /// <returns></returns>
        public ChunkData GetChunk(MapChunkId chunk)
        {
            ChunkData chunkTiles = null;
            ChunksAvailable.TryGetValue(chunk, out chunkTiles);
            if (chunkTiles == null)
                return null;
            return chunkTiles;
        }

        int chunkRemoveLock = 0;

        public void RemoveOldChunks()
        {
            var isLocked = Interlocked.Exchange(ref chunkRemoveLock, 1);
            if(isLocked == 0)
            {
                if (ChunksAvailable.Count > MaxChunks)
                {
                    var heroPos = server.MainHero.Position;
                    var toRemove = ChunksAvailable.Keys
                        .OrderBy(chunk => ((Vector)chunk.Center).DistanceTo(heroPos))
                        .Skip(MaxChunks * 3 / 4);
                    foreach (var id in toRemove)
                    {
                        var chunk = ChunksAvailable[id];
                        ChunksAvailable.TryRemove(id, out chunk);
                        chunkRequests.Remove(id);
                        //chunk.Dispose();
                    }
                    Console.WriteLine("Remove chunks!");
                }
                chunkRemoveLock = 0;
            }
        }

        BasicEffect effect;

        public void DrawTerrain()
        {
            var cameraLoc = server.CameraPosition;

            var device = effect.GraphicsDevice;

            //setup texturestuff
            effect.Set2DMatrices();
            effect.World = Microsoft.Xna.Framework.Matrix.CreateTranslation((float)-cameraLoc.X, (float)-cameraLoc.Y, 0);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = SpriteFactory.Terrain.TerrainAtlas.Texture;

            //draw all chunks around us
            foreach (var chunkId in EnumerateNearbyChunks())
            {
                var chunk = GetChunk(chunkId);
                if (chunk != null && chunk.HasBuffer)
                {
                    device.SetVertexBuffer(chunk.Buffer);
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2 * chunk.Area);
                    }
                }
            }

        }

    }
}
