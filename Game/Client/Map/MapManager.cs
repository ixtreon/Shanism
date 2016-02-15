using Client;
using IO;
using IO.Common;
using IO.Message;
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
    [Obsolete]
    class MapManager : IClientSystem
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
        /// Contains a map of all available chunks. 
        /// </summary>
        readonly ConcurrentDictionary<MapChunkId, ChunkData> ChunksAvailable = new ConcurrentDictionary<MapChunkId, ChunkData>();

        /// <summary>
        /// Contains a map of all chunk requests made so far. 
        /// </summary>
        readonly Dictionary<MapChunkId, long> chunkRequests = new Dictionary<MapChunkId, long>();

        readonly BasicEffect effect;

        public event Action<MapChunkId> ChunkRequested;

        public Vector CameraPosition { get; set; }

        public MapManager(GraphicsDevice device)
        {
            effect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
            };
        }

        public void Update(int msElapsed)
        {
            //request nearby chunks
            foreach (var c in EnumerateNearbyChunks())
                requestChunk(c);

            cleanupChunks();
        }

        public void HandleMessage(IOMessage ioMsg)
        {
            //only handle MapReply
            var msg = ioMsg as MapDataMessage;
            if (msg == null)
                return;

            var rect = msg.Chunk.Span;
            var chunkData = new ChunkData(msg.Chunk, msg.Data);

            ChunksAvailable[chunkData.Chunk] = chunkData;
            chunkData.BuildBuffer(effect.GraphicsDevice);
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
            var lastRequest = chunkRequests.TryGetVal(chunk) ?? long.MinValue;

            //make sure we don't spam the server
            var timeNow = Environment.TickCount;
            if (timeNow - SpamInterval < lastRequest)
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

        void cleanupChunks()
        {
            if (ChunksAvailable.Count > MaxChunks)
            {
                var toRemove = ChunksAvailable.Keys
                    .OrderBy(chunk => ((Vector)chunk.Center).DistanceTo(CameraPosition))
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
        }


        public void DrawTerrain()
        {
            var device = effect.GraphicsDevice;

            //setup texturestuff
            effect.Set2DMatrices();
            effect.World = Microsoft.Xna.Framework.Matrix.CreateTranslation((float)-CameraPosition.X, (float)-CameraPosition.Y, 0);
            effect.Projection = Microsoft.Xna.Framework.Matrix.CreateOrthographic((float)Screen.InGameSize.X, (float)Screen.InGameSize.Y, -5, 5);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = Content.Terrain.Texture;

            //draw all chunks around us
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunkId in EnumerateNearbyChunks())
                {
                    var chunk = GetChunk(chunkId);
                    if (chunk != null && chunk.HasBuffer)
                    {
                        device.SetVertexBuffer(chunk.Buffer);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2 * chunk.Area);
                    }
                }
            }

        }

        IEnumerable<MapChunkId> EnumerateNearbyChunks()
        {
            var lowLeft = CameraPosition - Screen.InGameSize / 2;
            var upRight = CameraPosition + Screen.InGameSize / 2;

            var chunks = MapChunkId.ChunksBetween(lowLeft, upRight).ToArray();
            return chunks;
        }

    }
}
