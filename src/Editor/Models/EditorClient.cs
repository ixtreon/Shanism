using Shanism.Client;
using Shanism.Client.Assets;
using Shanism.Common;
using Shanism.Common.Messages;
using Shanism.Editor.Actions;
using Shanism.Editor.Game;
using Shanism.ScenarioLib;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shanism.Editor.Controllers
{
    class EditorGameState : ClientGameState, IClientReceptor
    {

        public string Name { get; } = "MapEdit";
        public PlayerState PlayerState { get; } = new PlayerState();

        public ScenarioConfig Scenario { get; }

        //[Obsolete]
        //public IEngineReceptor Engine { get; private set; }


        // systems

        public ActionList History { get; }

        public ObjectsController ObjectEditor { get; }
        public TerrainController TerrainEditor { get; }
        public ContentController ContentEditor { get; }


        public EditorGameState(IClient client, ContentList mapContent, ScenarioConfig scenario)
            : this(client, mapContent, scenario, new EditorEngine(scenario)) { }

        EditorGameState(IClient client, ContentList mapContent, ScenarioConfig scenario, EditorEngine engine)
            : base(client, engine)
        {
            Scenario = scenario;

            //var trustedAssemblyPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);


            //var sysPath = trustedAssemblyPaths
            //    .Where(x => x.EndsWith("\\System.dll"))
            //    .ToArray();

            //var ans = Assembly.LoadFrom(sysPath[0]);
            //var ts = ans.GetForwardedTypes();

            var compiled = new ScenarioCompiler().TryCompile(Assembly.Load, scenario.BaseDirectory);

            var terrain = scenario.Map.GetTerrainMap();
            var combinedContent = new ContentList(client.DefaultContent, mapContent);

            History = new ActionList(this);
            ObjectEditor = new ObjectsController(History, scenario);
            TerrainEditor = new TerrainController(History, terrain);
            ContentEditor = new ContentController(History, scenario, mapContent);

            Initialize(combinedContent);
            engine.MessageHandler += HandleMessage;
        }

        public void HandleMessage(ServerMessage msg)
        {
            switch (msg.Type)
            {
                case ServerMessageType.MapData:
                    Terrain.SetChunkData((MapData)msg);
                    break;
            }
        }

        public void SetTerrainChunk(MapData chunk) => Terrain.SetChunkData(chunk);

    }
}
