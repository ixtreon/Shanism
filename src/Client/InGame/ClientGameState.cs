using Shanism.Client.Assets;
using Shanism.Client.Sprites;
using Shanism.Client.Systems;
using Shanism.Common;
using Shanism.Common.Entities;
using Shanism.Common.Messages;
using System;

namespace Shanism.Client
{
    /// <summary>
    /// Draws and updates the client game state.
    /// </summary>
    public class ClientGameState
    {
        readonly IClient client;
        readonly IEngineReceptor engine;

        TerrainPainter terrainPainter;
        EntityPainter entitiesPainter;

        protected TerrainSystem Terrain { get; private set; }
        protected EntitySystem Entities { get; private set; }


        protected ContentList ScenarioContent { get; private set; }

        public IHero MainHero => Entities?.Hero;
        public UnitSprite MainHeroSprite => Entities?.HeroSprite;
        public EntitySprite HoverSprite => Entities?.HoverSprite;

        public ClientGameState(IClient client, IEngineReceptor engine)
        {
            this.client = client;
            this.engine = engine;
        }

        public virtual void Initialize(ContentList content)
        {
            if (ScenarioContent != null)
                throw new Exception();

            ScenarioContent = content;

            // systems
            Terrain = new TerrainSystem(client.GraphicsDevice, client.Screen.Game, content);
            Terrain.RequestChunk += engine.HandleMessage;

            Entities = new EntitySystem(engine, client.Screen.Game, content, client.Mouse);

            // painters
            terrainPainter = new TerrainPainter(Terrain);
            entitiesPainter = new EntityPainter(Entities);
        }

        public virtual void Update(int msElapsed)
        {
            if (Terrain != null)
            {
                Terrain?.Update(msElapsed);
                Entities?.Update(msElapsed);
            }

            // track entity changes
        }

        public virtual void Draw(CanvasStarter canvas)
        {
            if (terrainPainter != null)
            {
                terrainPainter.Draw();
                entitiesPainter.Draw(canvas);
            }
        }

        public void SetChunkData(MapData message) => Terrain.SetChunkData(message.Span, message.Data);

        public void PanCameraToMainHero()
        {
            if(MainHero != null)
                client.Screen.Game.Pan(MainHero.Position);
        }

    }
}
