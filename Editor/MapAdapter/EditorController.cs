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

using Color = Microsoft.Xna.Framework.Color;
using Shanism.Engine.Common;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Editor.MapAdapter
{

    class EditorController : IReceptor, IEditorEngine
    {

        Vector inGameWindowSize = Constants.Client.WindowSize;

        /// <summary>
        /// Whether the map is currently being dragged around
        /// </summary>
        bool isPanningMap = false;

        /// <summary>
        /// The in-game point where panning started. 
        /// </summary>
        Vector mapPanningStart;

        MapTool currentTool;

        Vector mousePositionInGame = Vector.Zero;

        ObjectCreator Creator;

        readonly HashSet<Entity> _startupObjects = new HashSet<Entity>();

        readonly EditorControl Control;

        readonly SelectionTool selectionTool;

        /// <summary>
        /// The unit that is always at the center of the screen. 
        /// </summary>
        readonly HeroStub God;

        public ScenarioViewModel ScenarioView { get; private set; }


        public event Action MapChanged;


        IClientEngine Client => Control.Client;

        ScenarioConfig config => ScenarioView.Scenario.Config;

        MapConfig map => config.Map;

        public MapTool CurrentTool => currentTool;

        public IEnumerable<Entity> StartupObjects => _startupObjects;

        public event Action<IEnumerable<Entity>> SelectionChanged
        {
            add { selectionTool.SelectionChanged += value; }
            remove { selectionTool.SelectionChanged -= value; }
        }

        public EditorController(EditorControl c)
        {
            selectionTool = new SelectionTool(this);

            setTool(selectionTool);


            God = new HeroStub { Id = 100 };

            Control = c;
            Control.Resize += updateClientSize;
            Control.KeyDown += onClientKeyDown;
            Control.ClientLoaded += () =>
            {
                Client.SetWindowSize(Control.Size.ToPoint());
                updateClientSize(null, null);
            };

            initMapPanScroll();
            initMapTools();
        }

        public void LoadScenario(ScenarioViewModel sc)
        {
            ScenarioView = sc;

            //Start the client
            Control.Client.SetServer(this);

            //send the scenario datas
            var scData = config.SaveToBytes();
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

            //resend map
            resendMap();

            //resend units
            foreach (var u in _startupObjects.ToList())
                RemoveObject(u);

            var ocs = ScenarioView.Scenario.Config.Map.Objects.ToList();

            foreach (var oc in ocs)   //LMAOOOO
            {
                var o = CreateObject(oc);
                if (o != null)
                    AddObject(o);
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
            var e = Creator.CreateObject(oc);
            if (e == null)
                return null;

            e.Data = oc;
            return e;
        }

        /// <summary>
        /// Adds the given object to the <see cref="_startupObjects"/> list
        /// and sends an <see cref="ObjectSeenMessage"/> message to the game client. 
        /// </summary>
        public bool AddObject(Entity o)
        {
            if (_startupObjects.Add(o))
            {
                onMessageSent(new ObjectSeenMessage(o));
                return true;
            }
            return false;
        }

        public bool RemoveObject(Entity o)
        {
            if (_startupObjects.Remove(o))
            {
                onMessageSent(new ObjectUnseenMessage(o.Id, true));
                return true;
            }

            return false;
        }


        void setTool(MapTool newTool)
        {
            currentTool?.Dispose();
            currentTool = newTool;
            currentTool.MessageSent += onMessageSent;
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
                    setTool(selectionTool);
                    break;
                default:
                    CurrentTool.OnKeyPress(e);
                    break;
            }
        }

        void updateClientSize(object sender, EventArgs e)
        {
            var area = inGameWindowSize.X * inGameWindowSize.Y;

            if (Control.Width * Control.Height == 0)
                return;

            var ratio = (double)Control.Width / Control.Height;
            var h = Math.Sqrt(area / ratio);
            var w = h * ratio;
            inGameWindowSize = new Vector(w, h);
            Client?.SetCameraParams(null, God, inGameWindowSize);
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
            Control.MouseDown += (o, e) =>
            {
                if (e.Button != MouseButtons.Right) return;
                Control.Cursor = Cursors.NoMove2D;

                isPanningMap = true;
                mapPanningStart = mousePositionInGame;
            };

            Control.MouseMove += (o, e) =>
            {
                if (isPanningMap)
                {
                    var d = mapPanningStart - mousePositionInGame;
                    var newPos = (God.Position + d).Clamp(Vector.Zero, map.Size);

                    God.Position = newPos;
                }
            };

            Control.MouseUp += (o, e) =>
            {
                if (e.Button != MouseButtons.Right) return;
                Control.Cursor = Cursors.Arrow;

                isPanningMap = false;
            };

            //zoom in/out
            Control.MouseWheel += (o, e) =>
            {
                var ratio = (1 - (double)e.Delta / 120 * zoomFactor);
                inGameWindowSize *= ratio;
                Client.SetCameraParams(null, God, inGameWindowSize);
            };
        }


        void initMapTools()
        {
            Control.MouseDown += (o, e) =>
            {
                mousePositionInGame = Client.ScreenToGame(new Vector(e.X, e.Y));
                CurrentTool.OnMouseDown(e.Button, mousePositionInGame);
            };
            Control.MouseMove += (o, e) =>
            {
                mousePositionInGame = Client.ScreenToGame(new Vector(e.X, e.Y));
                CurrentTool.OnMouseMove(e.Button, mousePositionInGame);
            };
            Control.MouseUp += (o, e) =>
            {
                CurrentTool.OnMouseUp(e.Button, mousePositionInGame);
            };

            Control.OnDraw += () =>
            {
                CurrentTool?.OnDraw(Control, mousePositionInGame);
            };
        }

        void resendMap()
        {
            var mapData = getMapData(map);
            var msg = new MapDataMessage(new Rectangle(Point.Zero, map.Size), mapData);
            MessageSent(msg);
        }

        static TerrainType[] getMapData(MapConfig map)
        {
            var tty = new TerrainType[map.Width * map.Height];

            foreach (var x in Enumerable.Range(0, map.Width))
                foreach (var y in Enumerable.Range(0, map.Height))
                    tty[x + map.Width * y] = map.Terrain[x, y];

            return tty;
        }
    }
}
