using Ix.Logging;
using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Assets;
using Shanism.Client.IO;
using Shanism.Client.Terrain;
using Shanism.Common;
using Shanism.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using XnaMatrix = Microsoft.Xna.Framework.Matrix;
using XnaVector3 = Microsoft.Xna.Framework.Vector3;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// Requests visible chunks from the server and exposes an effect + tiles for drawing.
    /// </summary>
    public class TerrainSystem
    {
        static readonly XnaMatrix viewMatrix = XnaMatrix.CreateLookAt(
            new XnaVector3(0, 0, -3),
            new XnaVector3(0, 0, 0),
            new XnaVector3(0, -1, 0));

        /// <summary>
        /// The time before an uncompleted chunk request is re-sent, in seconds.
        /// </summary>
        const double SpamInterval = 10;

        /// <summary>
        /// The maximum number of chunks to keep in memory. 
        /// </summary>
        const int MaxChunks = 10_000;


        Dictionary<ChunkId, TerrainChunk> Chunks { get; } = new Dictionary<ChunkId, TerrainChunk>();

        /// <summary>
        /// A map of all chunk requests made so far. 
        /// </summary>
        Dictionary<ChunkId, DateTime> ChunkRequests { get; } = new Dictionary<ChunkId, DateTime>();

        InGameTransform Screen { get; }

        public GraphicsDevice GraphicsDevice { get; }

        BasicEffect Effect { get; }

        public event Action<MapRequest> RequestChunk;

        public TerrainSystem(GraphicsDevice graphicsDevice, InGameTransform screen, ContentList content)
        {
            GraphicsDevice = graphicsDevice;
            Screen = screen;
            Effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = false,
                TextureEnabled = true,
                View = viewMatrix,
            };

            SetContent(content);
        }

        public void SetContent(ContentList content)
        {
            if (!content.Textures.TryGet(Constants.Terrain.FileName, out var terrainTexture))
                ClientLog.Instance.Warning("Unable to load the terrain file!");

            Effect.Texture = terrainTexture.Texture;
        }

        public EffectPassCollection GetEffectPasses() => Effect.CurrentTechnique.Passes;

        public void Update(int msElapsed)
        {
            // TODO: use msElapsed to limit requests, clean-ups
            RequestNearbyChunks();

            CleanupDistantChunks();

            UpdateScreen();
        }

        public void UpdateScreen()
        {
            var gameCenter = Screen.Center;
            Effect.World = XnaMatrix.CreateTranslation(-gameCenter.X, -gameCenter.Y, 0);

            var gameSize = Screen.Size;
            Effect.Projection = XnaMatrix.CreateOrthographic(gameSize.X, gameSize.Y, -5, 5);
        }

        int RequestNearbyChunks()
        {
            if (RequestChunk == null)
            {
                ClientLog.Instance.Warning("No handler for map requests.. no maps for you.");
                return 0;
            }

            var nRequests = 0;
            foreach(var chunk in enumerateNearbyChunks(1))
            {
                if(Chunks.ContainsKey(chunk))
                    continue;

                //make sure we don't spam the server
                //TODO: server doesn't have similar protection
                var timeNow = DateTime.Now;
                if(ChunkRequests.TryGetValue(chunk, out var lastRequest))
                {
                    var tSinceRequest = (timeNow - lastRequest).TotalSeconds;
                    if(tSinceRequest < SpamInterval)
                        continue;
                }

                ChunkRequests[chunk] = timeNow;

                RequestChunk(new MapRequest(chunk));
                nRequests++;
            }
            return nRequests;
        }

        void CleanupDistantChunks()
        {
            const float cleanupPercentage = 0.25f;

            var nChunks = Chunks.Count;
            if(nChunks < MaxChunks)
                return;

            var toRemove = Chunks.Keys
                .OrderByDescending(chunk => ((Vector2)chunk.Center).DistanceToSquared(Screen.Center))
                .Take((int)(nChunks * cleanupPercentage));

            foreach(var id in toRemove)
            {
                destroyChunk(id);
                ChunkRequests.Remove(id);
            }
        }

        public IEnumerable<TerrainChunk> GetNearbyChunks()
        {
            var lowBin = ChunkId.ChunkOf(Screen.Origin).ID;
            var hiBin = ChunkId.ChunkOf(Screen.Origin + Screen.Size).ID;

            for(int ix = lowBin.X; ix <= hiBin.X; ix++)
                for(int iy = lowBin.Y; iy <= hiBin.Y; iy++)
                    if(Chunks.TryGetValue(new ChunkId(ix, iy), out var chunk))
                        yield return chunk;
        }

        public void SetChunkData(MapData message) => SetChunkData(message.Span, message.Data);

        public void SetChunkData(Rectangle rect, TerrainType[] tty)
        {
            foreach(var ch in ChunkId.ChunksBetween(rect.Position, rect.FarPosition - 1))
            {
                var chunkData = Chunks.TryGet(ch);

                if(chunkData == null)
                {
                    chunkData = new TerrainChunk(GraphicsDevice, ch, tty, rect);
                    Chunks[ch] = chunkData;
                }
                else
                    chunkData.SetTiles(tty, rect);
            }
        }

        public void InvalidateRect(Rectangle rect)
        {
            foreach(var ch in ChunkId.ChunksBetween(rect.Position, rect.FarPosition))
                destroyChunk(ch);
        }

        IEnumerable<ChunkId> enumerateNearbyChunks(int bonusRange = 0)
        {
            var halfScreenSize = Screen.Size / 2 + ChunkId.ChunkSize * bonusRange;
            var lowLeft = Screen.Center - halfScreenSize;
            var upRight = Screen.Center + halfScreenSize;

            return ChunkId.ChunksBetween(lowLeft, upRight);
        }

        bool destroyChunk(ChunkId id)
        {
            if(!Chunks.TryGetValue(id, out TerrainChunk chunk))
                return false;

            Chunks.Remove(id);
            ChunkRequests.Remove(id);
            chunk.Dispose();
            return true;
        }
    }
}
