using Client;
using IO;
using IO.Message.Server;
using ShanoEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Message;
using Engine.Objects;
using IO.Common;
using ScenarioLib;
using System.Windows.Forms;
using IO.Objects;
using Engine;
using Engine.Objects.Entities;

using Color = Microsoft.Xna.Framework.Color;
using Engine.Common;

namespace ShanoEditor.MapAdapter
{
    enum BrushType
    {
        None, Terrain, Object
    }


    /// <summary>
    ///  
    /// </summary>
    class EditorEngine : IReceptor, IEditorEngine
    {
        Vector inGameSize = Constants.Client.WindowSize;


        /// <summary>
        /// Whether the map is currently being dragged around
        /// </summary>
        bool isPanningMap = false;

        /// <summary>
        /// The in-game point where panning started. 
        /// </summary>
        Vector mapPanningStart;

        MapTool _currentTool;

        Vector _mousePositionInGame = Vector.Zero;

        readonly HashSet<Entity> _startupObjects = new HashSet<Entity>();

        readonly EditorControl Client;

        /// <summary>
        /// The unit that is always at the center of the screen. 
        /// </summary>
        readonly HeroStub God;

        ObjectCreator Creator;

        public ScenarioViewModel ScenarioView { get; private set; }


        public event Action MapChanged;


        IClientEngine Engine => Client.Engine;

        ScenarioConfig config => ScenarioView.Scenario.Config;

        MapConfig map => config.Map;

        public MapTool CurrentTool => _currentTool;

        public IEnumerable<Entity> StartupObjects => _startupObjects;


        public EditorEngine(EditorControl c)
        {
            setTool(new SelectionTool(this));

            God = new HeroStub { Id = 100 };

            Client = c;
            Client.Resize += onClientResize;
            Client.KeyDown += onClientKeyDown;
            onClientResize(null, null);

            initMapPanScroll();
            initMapTools();
        }

        public void LoadScenario(ScenarioViewModel sc)
        {
            ScenarioView = sc;

            //Start the client
            Client.Engine.SetServer(this);

            //send the scenario datas
            var scData = config.GetBytes();
            var contentData = config.ZipContent();
            IOMessage msg = new HandshakeReplyMessage(true, scData, contentData);
            MessageSent(msg);

            //objectconstr/creator
            Creator = new ObjectCreator(ScenarioView.Scenario);

            // Create God aka the camera
            God.Position = sc.Scenario.Config.Map.Size / 2;

            //send obj seen
            msg = new ObjectSeenMessage(God);
            MessageSent(msg);

            //send obj owned
            msg = new PlayerStatusMessage(God.Id);
            MessageSent(msg);

            //send map
            resendMap();

            //send units
            foreach (var oc in ScenarioView.Scenario.Config.Map.Objects)   //LMAOOOO
            {
                var o = CreateObject(oc);
                AddObject(o);
            }
        }

        /// <summary>
        /// Creates the entity specified in the given ObjectConstructor. 
        /// </summary>
        /// <param name="oc"></param>
        /// <returns></returns>
        public Entity CreateObject(ObjectConstructor oc)
        {
            return Creator.CreateObject(oc);
        }

        /// <summary>
        /// Adds the given object to the <see cref="_startupObjects"/> list
        /// and sends an <see cref="ObjectSeenMessage"/> message to the game client. 
        /// </summary>
        /// <param name="oc"></param>
        /// <returns></returns>
        public bool AddObject(Entity o)
        {
            if (_startupObjects.Add(o))
            {
                MessageSent(new ObjectSeenMessage(o));
                return true;
            }
            return false;
        }

        public bool RemoveObject(Entity o)
        {
            if (_startupObjects.Remove(o))
            {
                MessageSent(new ObjectUnseenMessage(o.Id));
                return true;
            }

            return false;
        }


        void setTool(MapTool newTool)
        {
            _currentTool?.Dispose();
            _currentTool = newTool;
            _currentTool.MessageSent += onMessageSent;
        }

        void onMessageSent(IOMessage msg)
        {
            MessageSent?.Invoke(msg);
            MapChanged?.Invoke();
        }

        void onClientKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    setTool(new SelectionTool(this));
                    break;
                default:
                    CurrentTool.OnKeyPress(e);
                    break;
            }
        }

        void onClientResize(object sender, EventArgs e)
        {
            var area = inGameSize.X * inGameSize.Y;

            if (Client.Width * Client.Height == 0)
                return;

            var ratio = (double)Client.Width / Client.Height;
            var h = Math.Sqrt(area / ratio);
            var w = h * ratio;
            inGameSize = new Vector(w, h);
            Engine?.SetCameraParams(null, God, inGameSize);
        }

        public void ResizeMap(Point newSize)
        {
            if (ScenarioView == null) return;

            clearMap();

            //do the resize
            map.ResizeMap(newSize.X, newSize.Y);

            //fill the client map
            resendMap();

            MapChanged?.Invoke();
        }

        void clearMap()
        {
            var mapSize = map.Size;

            //null out the client map
            MessageSent(new MapDataMessage(new Rectangle(Point.Zero, mapSize)));
        }

        public void SetBrush(TerrainType tty, int size, bool isCircle)
        {
            if (ScenarioView == null) return;

            setTool(new TerrainBrush(this, tty, size, isCircle));
        }

        public void SetBrush(IEntity obj)
        {
            if (ScenarioView == null) return;

            setTool(new CustomObjectBrush(this, obj));
        }

        public void SetBrush(ObjectConstructor oc)
        {
            setTool(new ObjectConstructorBrush(this, oc));
        }

        #region IReceptor implementation

        public event Action<IOMessage> MessageSent;

        public void UpdateServer(int msElapsed)
        {
            Keyboard.Update();
        }

        public string GetPerfData()
        {
            return "All is OK!";
        }

        #endregion


        void initMapPanScroll()
        {
            const double zoomFactor = 0.05;

            //drag-to-move
            Client.MouseDown += (o, e) =>
            {
                if (e.Button != MouseButtons.Right) return;
                Client.Cursor = Cursors.NoMove2D;

                isPanningMap = true;
                mapPanningStart = _mousePositionInGame;
            };

            Client.MouseMove += (o, e) =>
            {
                if (isPanningMap)
                {
                    var d = mapPanningStart - _mousePositionInGame;

                    God.Position += d;

                }
            };

            Client.MouseUp += (o, e) =>
            {
                if (e.Button != MouseButtons.Right) return;
                Client.Cursor = Cursors.Arrow;

                isPanningMap = false;
            };

            //zoom in/out
            Client.MouseWheel += (o, e) =>
            {
                var ratio = (1 - (double)e.Delta / 120 * zoomFactor);
                inGameSize *= ratio;
                Engine.SetCameraParams(null, God, inGameSize);
            };
        }


        void initMapTools()
        {
            Client.MouseDown += (o, e) =>
            {
                _mousePositionInGame = Engine.ScreenToGame(new Vector(e.X, e.Y));
                CurrentTool.OnMouseDown(e.Button, _mousePositionInGame);
            };
            Client.MouseMove += (o, e) =>
            {
                _mousePositionInGame = Engine.ScreenToGame(new Vector(e.X, e.Y));
                CurrentTool.OnMouseMove(e.Button, _mousePositionInGame);
            };
            Client.MouseUp += (o, e) =>
            {
                CurrentTool.OnMouseUp(e.Button, _mousePositionInGame);
            };

            Client.OnDraw += () =>
            {
                CurrentTool?.OnDraw(Client, _mousePositionInGame);
            };
        }

        void resendMap()
        {
            var mapData = getMapData(map);
            var msg = new MapDataMessage(new Rectangle(Point.Zero, map.Size), mapData);
            MessageSent(msg);
        }

        TerrainType[] getMapData(MapConfig map)
        {
            var tty = new TerrainType[map.Width * map.Height];

            foreach (var x in Enumerable.Range(0, map.Width))
                foreach (var y in Enumerable.Range(0, map.Height))
                    tty[x + map.Width * y] = map.Terrain[x, y];

            return tty;
        }
    }
}
