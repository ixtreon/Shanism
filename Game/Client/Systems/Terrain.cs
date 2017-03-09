using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Client.Systems;
using Shanism.Common.Message.Client;
using Shanism.Client.Drawing;

namespace Shanism.Client.Map
{

    /// <summary>
    /// Displays terrain chunks and sends requests for new ones. 
    /// </summary>
    class Terrain : ClientSystem
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
        readonly Dictionary<ChunkId, TerrainChunk> ChunksAvailable = new Dictionary<ChunkId, TerrainChunk>();

        /// <summary>
        /// Contains a map of all chunk requests made so far. 
        /// </summary>
        readonly Dictionary<ChunkId, long> chunkRequests = new Dictionary<ChunkId, long>();


        readonly BasicEffect effect;

        TerrainCache terrain => Content.Terrain;


        public Terrain(GameComponent game)
            : base(game)
        {
            effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = false,
                TextureEnabled = true,
            };
            effect.SetStaticViewMatrix();
        }

        Vector CameraPosition => Screen.GameCenter;

        public override void Update(int msElapsed)
        {
            //request nearby chunks
            foreach (var c in EnumerateNearbyChunks(1))
                tryRequestChunk(c);

            //remove stale chunks
            cleanupChunks();

            //update effect values
            effect.World = Microsoft.Xna.Framework.Matrix.CreateTranslation((float)-CameraPosition.X, (float)-CameraPosition.Y, 0);
            effect.Projection = Microsoft.Xna.Framework.Matrix.CreateOrthographic(
                (float)Screen.GameSize.X, 
                (float)Screen.GameSize.Y, 
                -5, 5);
            effect.Texture = terrain.Texture;
        }

        public void Draw()
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            //draw all chunks around us
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunkId in EnumerateNearbyChunks())
                {
                    var chunk = ChunksAvailable.TryGet(chunkId);
                    if (chunk != null && chunk.HasBuffer)
                    {
                        GraphicsDevice.SetVertexBuffer(chunk.Buffer);
                        GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2 * chunk.Area);
                    }
                }
            }
        }

        public override void HandleMessage(IOMessage ioMsg)
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
            foreach (var ch in ChunkId.ChunksBetween(msg.Span.Position, msg.Span.FarPosition - 1))
            {
                var chunkData = ChunksAvailable.TryGet(ch);

                if (chunkData == null)
                {
                    chunkData = new TerrainChunk(GraphicsDevice, ch, effect.Texture, msg.Data, msg.Span);
                    ChunksAvailable[ch] = chunkData;
                }
                else
                    chunkData.SetTiles(msg.Data, msg.Span);
            }
        }

        void clearMap(MapDataMessage msg)
        {
            TerrainChunk chunkData;

            foreach (var ch in ChunkId.ChunksBetween(msg.Span.Position, msg.Span.FarPosition))
            {
                var overlap = ch.Span.IntersectWith(msg.Span);

                //remove the whole chunk if we can
                if (overlap == ch.Span)
                    destroyChunk(ch);
                else if(ChunksAvailable.TryGetValue(ch, out chunkData))
                    chunkData.ClearTiles(overlap);
            }
        }

        /// <summary>
        /// Sends a request to the server for the given chunk. 
        /// </summary>
        bool tryRequestChunk(ChunkId chunk)
        {
            if (ChunksAvailable.ContainsKey(chunk))
                return false;

            //make sure we don't spam the server
            long lastRequest;
            if (!chunkRequests.TryGetValue(chunk, out lastRequest))
                lastRequest = long.MinValue;

            var timeNow = Environment.TickCount;
            if (timeNow - SpamInterval < lastRequest)
                return false;

            //make the request and set last timestamp
            chunkRequests[chunk] = timeNow;
            SendMessage(new MapRequestMessage(chunk));
            return true;
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
                    destroyChunk(id);
                    chunkRequests.Remove(id);
                }
            }
        }

        void destroyChunk(ChunkId id)
        {
            TerrainChunk chunk;
            if (ChunksAvailable.TryGetValue(id, out chunk))
            {
                ChunksAvailable.Remove(id);
                chunk.Dispose();
            }
        }



        IEnumerable<ChunkId> EnumerateNearbyChunks(int bonusRange = 0)
        {
            var lowLeft = CameraPosition - Screen.GameSize / 2 - ChunkId.ChunkSize * bonusRange;
            var upRight = CameraPosition + Screen.GameSize / 2 + ChunkId.ChunkSize * bonusRange;

            return ChunkId.ChunksBetween(lowLeft, upRight).ToList();
        }

    }
}
