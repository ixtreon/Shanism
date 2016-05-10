using Shanism.Client;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Client.Map
{
    /// <summary>
    /// Handles map communication with the server. 
    /// </summary>
    class MapManager : IClientSystem
    {
        /// <summary>
        /// The time before an uncompleted chunk request is re-sent. 
        /// </summary>
        const int SpamInterval = 5000;

        /// <summary>
        /// The maximum number of chunks to keep in memory. 
        /// </summary>
        const int MaxChunks = 10000;

        /// <summary>
        /// Contains a map of all available chunks. 
        /// </summary>
        readonly Dictionary<MapChunkId, ChunkData> ChunksAvailable = new Dictionary<MapChunkId, ChunkData>();

        /// <summary>
        /// Contains a map of all chunk requests made so far. 
        /// </summary>
        readonly Dictionary<MapChunkId, long> chunkRequests = new Dictionary<MapChunkId, long>();

        readonly BasicEffect effect;

        public event Action<MapChunkId> ChunkRequested;

        public Vector CameraPosition { get { return Screen.InGameCenter; } }

        readonly GraphicsDevice _device;

        public MapManager(GraphicsDevice device)
        {
            _device = device;
            effect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
            };
        }

        public void Update(int msElapsed)
        {
            //request nearby chunks
            foreach (var c in EnumerateNearbyChunks(1))
                requestChunk(c);

            cleanupChunks();
        }

        public void HandleMessage(IOMessage ioMsg)
        {
            //only handle MapReply
            var msg = ioMsg as MapDataMessage;
            if (msg == null)
                return;

            //clear map
            if(!msg.HasMap)
            {
                clearMap(msg);
                return;
            }

            //set map
            setMap(msg);
        }

        void setMap(MapDataMessage msg)
        {
            foreach (var ch in MapChunkId.ChunksBetween(msg.Span.Position, msg.Span.FarPosition - 1))
            {
                var chunkData = ChunksAvailable.TryGet(ch);

                if (chunkData == null)
                {
                    chunkData = new ChunkData(_device, ch, msg.Data, msg.Span);
                    ChunksAvailable[ch] = chunkData;
                }
                else
                    chunkData.SetTiles(msg.Data, msg.Span);
            }
        }

        void clearMap(MapDataMessage msg)
        {
            ChunkData chunkData;

            foreach (var ch in MapChunkId.ChunksBetween(msg.Span.Position, msg.Span.FarPosition))
            {
                var overlap = ch.Span.IntersectWith(msg.Span);

                //remove the whole chunk if we can
                if (overlap == ch.Span)
                    ChunksAvailable.Remove(ch);
                else if(ChunksAvailable.TryGetValue(ch, out chunkData))
                    chunkData.ClearTiles(overlap);
            }
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
                    ChunksAvailable.Remove(id);
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
                    var chunk = ChunksAvailable.TryGet(chunkId);
                    if (chunk != null && chunk.HasBuffer)
                    {
                        device.SetVertexBuffer(chunk.Buffer);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2 * chunk.Area);
                    }
                }
            }

        }

        IEnumerable<MapChunkId> EnumerateNearbyChunks(int bonusRange = 0)
        {
            var lowLeft = CameraPosition - Screen.InGameSize / 2 - MapChunkId.ChunkSize * bonusRange;
            var upRight = CameraPosition + Screen.InGameSize / 2 + MapChunkId.ChunkSize * bonusRange;

            var chunks = MapChunkId.ChunksBetween(lowLeft, upRight).ToArray();
            return chunks;
        }

    }
}
