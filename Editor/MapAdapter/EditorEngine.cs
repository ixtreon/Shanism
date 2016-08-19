using Shanism.Client;
using Shanism.Common;
using Shanism.Common.Message.Server;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Engine.Objects;
using Shanism.Common.Game;
using Shanism.ScenarioLib;
using System.Windows.Forms;
using Shanism.Common.StubObjects;
using Shanism.Engine;
using Shanism.Engine.Entities;


using Shanism.Engine.Common;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Editor.MapAdapter
{

    class EditorEngine : IShanoEngine, IEditorEngine
    {
        readonly HashSet<Entity> _startupObjects = new HashSet<Entity>();
        readonly HashSet<IEntity> _visibleObjects = new HashSet<IEntity>();

        /// <summary>
        /// The unit that is always at the center of the screen. 
        /// </summary>
        public readonly HeroStub God;

        readonly ObjectCreator creator;
        EditorReceptor player;




        public ScenarioViewModel ScenarioView { get; private set; }


        Vector inGameWindowSize = Constants.Client.WindowSize;



        ScenarioConfig config => ScenarioView.Scenario.Config;
        MapConfig map => config.Map;


        public IReadOnlyCollection<Entity> StartupObjects => _startupObjects;
        public IReadOnlyCollection<IEntity> VisibleEntities => _visibleObjects;

        readonly byte[] scenarioConfigData;
        readonly byte[] scenarioContentData;

        public event Action SomethingChanged;

        public EditorEngine(ScenarioViewModel sc)
        {
            ScenarioView = sc;

            //save the scenario datas
            scenarioConfigData = config.SaveToBytes();
            scenarioContentData = config.ZipContent();

            //objectconstr/creator
            creator = new ObjectCreator(ScenarioView.Scenario);

            // Reset the entities, create god (aka the camera)
            God = new HeroStub(1) { Position = sc.Scenario.Config.Map.Size / 2 };
            _visibleObjects.Add(God);

            //recreate startup objects
            var ocs = ScenarioView.Scenario.Config.Map.Objects.ToList();    //LMAOOOO
            foreach (var oc in ocs)
            {
                var o = CreateObject(oc);
                if (o != null)
                {
                    _startupObjects.Add(o);
                    _visibleObjects.Add(o);
                }
                else
                    ScenarioView.Scenario.Config.Map.Objects.Remove(oc);
            }
        }

        /// <summary>
        /// Creates the entity specified in the given <see cref="ObjectConstructor"/>. 
        /// Saves the <see cref="ObjectConstructor"/> in the <see cref="Entity.Data"/> field. 
        /// </summary>
        public Entity CreateObject(ObjectConstructor oc)
        {
            var e = creator.CreateObject(oc);
            if (e == null)
                return null;

            e.Data = oc;
            return e;
        }

        public bool AddObject(Entity e)
        {
            var ans = _startupObjects.Add(e) && _visibleObjects.Add(e);
            if (ans) SomethingChanged?.Invoke();
            return ans;
        }

        public bool RemoveObject(Entity e)
        {
            var ans = _startupObjects.Remove(e) && _visibleObjects.Remove(e);
            if (ans) SomethingChanged?.Invoke();
            return ans;
        }

        public void ResizeMap(Point newSize)
        {
            if (ScenarioView == null) return;

            //send blank map
            player.SendMessage(new MapDataMessage(new Rectangle(Point.Zero, map.Size)));
            //do the resize
            map.ResizeMap(newSize.X, newSize.Y);
            //send it ot the client
            resendMap();

            SomethingChanged?.Invoke();
        }

        void IEditorEngine.SendMessage(IOMessage msg) => player.SendMessage(msg);

        void resendMap()
        {
            var mapData = getMapData(map);
            var msg = new MapDataMessage(new Rectangle(Point.Zero, map.Size), mapData);
            player.SendMessage(msg);
        }

        static TerrainType[] getMapData(MapConfig map)
        {
            var tty = new TerrainType[map.Width * map.Height];

            foreach (var x in Enumerable.Range(0, map.Width))
                foreach (var y in Enumerable.Range(0, map.Height))
                    tty[x + map.Width * y] = map.Terrain[x, y];

            return tty;
        }

        IReceptor IShanoEngine.AcceptClient(IShanoClient c)
        {
            if (player != null)
                throw new InvalidOperationException();

            return player = new EditorReceptor(this, 1);
        }

        void IShanoEngine.StartPlaying(IReceptor rec)
        {
            //send scenario
            player.SendMessage(new HandshakeReplyMessage(player.Id, scenarioConfigData, scenarioContentData));
            //send map
            resendMap();
            //send god
            player.SendMessage(new PlayerStatusMessage(God.Id));
        }

        void IShanoEngine.OpenToNetwork()
        {
            throw new InvalidOperationException();
        }

        void IShanoEngine.RestartScenario()
        {
            throw new InvalidOperationException();
        }

        void IShanoEngine.Update(int msElapsed)
        {

        }
    }
}
