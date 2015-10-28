using Client;
using Client.Sprites;
using IO;
using IO.Common;
using IO.Message.Server;
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
        /// Contains a map of all available chunks. 
        /// </summary>
        ConcurrentDictionary<MapChunkId, ChunkData> ChunksAvailable = new ConcurrentDictionary<MapChunkId, ChunkData>();

        /// <summary>
        /// Contains a map of all chunk requests made so far. 
        /// </summary>
        Dictionary<MapChunkId, long> chunkRequests = new Dictionary<MapChunkId, long>();


        public event Action<MapChunkId> ChunkRequested;



        public MapManager(GraphicsDevice device)
        {
            effect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
            };
        }

        public void Update(Vector cameraPos)
        {
            //request nearby chunks
            foreach (var c in EnumerateNearbyChunks(cameraPos).Except(ChunksAvailable.Keys))
                requestChunk(c);

            RemoveOldChunks(cameraPos);
        }

        public void HandleMapReply(MapReplyMessage msg)
        {
            var rect = msg.Chunk.Span;

            var chunkData = new ChunkData(msg.Chunk, msg.Data);

            ChunksAvailable[chunkData.Chunk] = chunkData;
            chunkData.BuildBuffer(effect.GraphicsDevice, GetTile);
        }

        public TerrainType GetTile(int x, int y)
        {
            var id = MapChunkId.ChunkOf(new Vector(x, y) + new Vector(0.5));
            var chunk = GetChunk(id);
            return chunk?.GetTile(x, y) ?? TerrainType.None;
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
            long lastRequest;
            if (!chunkRequests.TryGetValue(chunk, out lastRequest))
                lastRequest = long.MinValue;

            //make sure we don't spam the server
            long timeNow = Environment.TickCount;
            if (timeNow - lastRequest < SpamInterval)
                return;

            //make the request and set last timestamp
            chunkRequests[chunk] = timeNow;
            ChunkRequested(chunk);
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

        public void RemoveOldChunks(Vector cameraPos)
        {
            var isLocked = Interlocked.Exchange(ref chunkRemoveLock, 1);
            if(isLocked == 0)
            {
                if (ChunksAvailable.Count > MaxChunks)
                {
                    var toRemove = ChunksAvailable.Keys
                        .OrderBy(chunk => ((Vector)chunk.Center).DistanceTo(cameraPos))
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

        public void DrawTerrain(Vector cameraPos)
        {

            var device = effect.GraphicsDevice;

            //setup texturestuff
            effect.Set2DMatrices();
            effect.World = Microsoft.Xna.Framework.Matrix.CreateTranslation((float)-cameraPos.X, (float)-cameraPos.Y, 0);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = SpriteFactory.Terrain.TerrainAtlas.Texture;

            //draw all chunks around us
            foreach (var chunkId in EnumerateNearbyChunks(cameraPos))
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

        public static IEnumerable<MapChunkId> EnumerateNearbyChunks(Vector cameraPos)
        {
            var lowLeft = cameraPos - (Vector)Constants.Client.WindowSize / 2;
            var upRight = cameraPos + (Vector)Constants.Client.WindowSize / 2;

            var chunks = MapChunkId.ChunksBetween(lowLeft, upRight).ToArray();
            return chunks;
        }

    }
}
