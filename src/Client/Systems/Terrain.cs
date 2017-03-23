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
using Microsoft.Xna.Framework;

namespace Shanism.Client.Map
{

    /// <summary>
    /// Displays terrain chunks and sends requests for new ones. 
    /// </summary>
    class Terrain : ShanoComponent, IClientSystem
    {
        readonly static Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -3), new Vector3(0, 0, 0), new Vector3(0, -1, 0));

        public event Action<IOMessage> MessageSent;

        protected void SendMessage(IOMessage msg) => MessageSent?.Invoke(msg);

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
        readonly Dictionary<ChunkId, TerrainChunk> chunksAvailable = new Dictionary<ChunkId, TerrainChunk>();

        /// <summary>
        /// Contains a map of all chunk requests made so far. 
        /// </summary>
        readonly Dictionary<ChunkId, long> chunkRequests = new Dictionary<ChunkId, long>();


        readonly BasicEffect effect;

        readonly Texture2D terrainTexture;

        GraphicsDevice graphicsDevice;

        public Terrain(IShanoComponent game, GraphicsDevice device)
            : base(game)
        {
            graphicsDevice = device;
            //grab the terrain texture
            if (!Content.Textures.TryGetValue(Constants.Content.TerrainFile, out terrainTexture))
                Console.WriteLine("Warning: unable to load the terrain file!");

            //create the XNA effect used for drawing
            effect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = false,
                TextureEnabled = true,
                View = viewMatrix,
            };
        }

        Vector CameraPosition => Screen.GameCenter;

        public void Update(int msElapsed)
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
            effect.Texture = terrainTexture;
        }

        public void Draw()
        {
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            //draw all chunks around us
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunkId in EnumerateNearbyChunks())
                {
                    var chunk = chunksAvailable.TryGet(chunkId);
                    if (chunk != null && chunk.HasBuffer)
                    {
                        graphicsDevice.SetVertexBuffer(chunk.Buffer);
                        graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2 * chunk.Area);
                    }
                }
            }
        }



        public void ParseMessage(MapDataMessage msg)
        {
            if (msg.HasMap)
                setMap(msg);
            else
                clearMap(msg);
        }



        void setMap(MapDataMessage msg)
        {
            foreach (var ch in ChunkId.ChunksBetween(msg.Span.Position, msg.Span.FarPosition - 1))
            {
                var chunkData = chunksAvailable.TryGet(ch);

                if (chunkData == null)
                {
                    chunkData = new TerrainChunk(graphicsDevice, ch, effect.Texture, msg.Data, msg.Span);
                    chunksAvailable[ch] = chunkData;
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
                else if(chunksAvailable.TryGetValue(ch, out chunkData))
                    chunkData.ClearTiles(overlap);
            }
        }

        /// <summary>
        /// Sends a request to the server for the given chunk. 
        /// </summary>
        bool tryRequestChunk(ChunkId chunk)
        {
            if (chunksAvailable.ContainsKey(chunk))
                return false;

            //make sure we don't spam the server
            long lastRequest;
            if (!chunkRequests.TryGetValue(chunk, out lastRequest))
                lastRequest = int.MinValue;

            var timeNow = Environment.TickCount;
            if (timeNow - SpamInterval < lastRequest)
                return false;

            //make the request and set last timestamp
            chunkRequests[chunk] = timeNow;
            MessageSent?.Invoke(new MapRequestMessage(chunk));

            return true;
        }

        void cleanupChunks()
        {
            if (chunksAvailable.Count > MaxChunks)
            {
                var toRemove = chunksAvailable.Keys
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
            if (chunksAvailable.TryGetValue(id, out chunk))
            {
                chunksAvailable.Remove(id);
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
