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

    class EditorController
    {
        public ScenarioViewModel ScenarioView { get; private set; }

        readonly EditorControl clientControl;
        IClientEngine gameClient;
        EditorEngine gameEngine;

        SelectionTool selectionTool;

        //
        Vector inGameWindowSize = Constants.Client.WindowSize;

        MapTool currentTool;

        //mouse position
        Vector mouseScreenPosition = Vector.Zero;
        Vector mouseGamePosition = Vector.Zero;

        //mouse pan/zoom
        bool isPanningMap;
        Vector mapPanStartPos;
        Vector mapPanGodPos;

        public event Action MapChanged;

        public event Action<IEnumerable<Entity>> SelectionChanged;

        public EditorController(EditorControl c)
        {

            clientControl = c;
            clientControl.Resize += updateClientSize;
            clientControl.KeyDown += onClientKeyDown;
            clientControl.ClientLoaded += () =>
            {
                gameClient = clientControl.GameClient;
                gameClient.SetWindowSize(clientControl.Size.ToPoint());
                updateClientSize(null, null);
            };

            initMapHandlers();
            initToolHandlers();
        }

        public void LoadScenario(ScenarioViewModel sc)
        {
            ScenarioView = sc;

            //create engine, connect the client to it
            gameEngine = new EditorEngine(sc);
            gameEngine.SomethingChanged += () => MapChanged?.Invoke();

            IReceptor rec;
            if (!gameClient.TryConnect(gameEngine, out rec))
                throw new Exception("Unable to connect to the editor engine!");

            ((IShanoEngine)gameEngine).StartPlaying(rec);

            //initialize the map tools
            selectionTool = new SelectionTool(gameEngine);
            selectionTool.SelectionChanged += (es) 
                => SelectionChanged?.Invoke(es);
            setTool(selectionTool);
        }

        void setTool(MapTool newTool)
        {
            currentTool?.Dispose();
            currentTool = newTool;
        }

        void onClientKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    setTool(selectionTool);
                    break;
                default:
                    currentTool.OnKeyPress(e);
                    break;
            }
        }

        void updateClientSize(object sender, EventArgs e)
        {
            var area = inGameWindowSize.X * inGameWindowSize.Y;

            if (clientControl.Width * clientControl.Height == 0)
                return;

            var ratio = (double)clientControl.Width / clientControl.Height;
            var h = Math.Sqrt(area / ratio);
            var w = h * ratio;
            inGameWindowSize = new Vector(w, h);
            gameClient?.MoveCamera(null, inGameWindowSize);
        }

        public void ResizeMap(Point newSize) => gameEngine.ResizeMap(newSize);

        public void SetBrush(TerrainType tty, int size, bool isCircle)
        {
            if (ScenarioView == null) return;
            setTool(new TerrainBrush(gameEngine, tty, size, isCircle));
        }

        public void SetBrush(IEntity obj)
        {
            if (ScenarioView == null) return;
            setTool(new CustomObjectBrush(gameEngine, obj));
        }

        public void SetBrush(ObjectConstructor oc)
        {
            if (ScenarioView == null) return;
            setTool(new ObjectConstructorBrush(gameEngine, oc));
        }



        void initMapHandlers()
        {
            const double zoomFactor = 0.05;

            //drag-to-move
            clientControl.MouseDown += (o, e) =>
            {
                if (e.Button != MouseButtons.Right) return;
                clientControl.Cursor = Cursors.NoMove2D;

                isPanningMap = true;
                mapPanStartPos = new Vector(e.X, e.Y);
                mapPanGodPos = gameEngine.God.Position;
            };

            clientControl.MouseMove += (o, e) =>
            {
                if (isPanningMap)
                {
                    var d = gameClient.ScreenToGame(new Vector(e.X, e.Y)) - gameClient.ScreenToGame(mapPanStartPos);

                    gameEngine.God.Position = mapPanGodPos - d;
                }
            };

            clientControl.MouseUp += (o, e) =>
            {
                if (e.Button != MouseButtons.Right) return;
                clientControl.Cursor = Cursors.Arrow;

                isPanningMap = false;
            };

            //zoom in/out
            clientControl.MouseWheel += (o, e) =>
            {
                var ratio = (1 - (double)e.Delta / 120 * zoomFactor);
                inGameWindowSize *= ratio;
                gameClient.MoveCamera(null, inGameWindowSize);
            };
        }


        void initToolHandlers()
        {
            clientControl.MouseDown += (o, e) =>
            {
                mouseScreenPosition = new Vector(e.X, e.Y);
                mouseGamePosition = gameClient.ScreenToGame(mouseScreenPosition);
                currentTool.OnMouseDown(e.Button, mouseGamePosition);
            };
            clientControl.MouseMove += (o, e) =>
            {
                mouseScreenPosition = new Vector(e.X, e.Y);
                mouseGamePosition = gameClient.ScreenToGame(mouseScreenPosition);
                currentTool.OnMouseMove(e.Button, mouseGamePosition);
            };
            clientControl.MouseUp += (o, e) =>
            {
                currentTool.OnMouseUp(e.Button, mouseGamePosition);
            };

            clientControl.OnDraw += () =>
            {
                currentTool?.OnDraw(clientControl, mouseGamePosition);
            };
        }
    }
}
