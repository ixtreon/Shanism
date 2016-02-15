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
using Engine.Entities;
using IO.Common;
using ScenarioLib;
using System.Windows.Forms;

namespace ShanoEditor.MapAdapter
{
    /// <summary>
    ///  
    /// </summary>
    class EditorEngine : IReceptor
    {
        double PanSpeed { get; set; } = 5;

        ScenarioViewModel ScenarioView { get; }

        IClientEngine Client { get; }


        public event Action<IOMessage> MessageSent;

        public Vector ScreenSize { get; set; } = Constants.Client.WindowSize;

        GodBuilder God { get; } = new GodBuilder();


        public EditorEngine(EditorClient c, ScenarioViewModel sc)
        {
            ScenarioView = sc;
            Client = c.Engine;
            c.MouseWheel += onMouseWheel;
            c.MouseDown += onMouseDown;
            c.MouseUp += onMouseUp;
            c.KeyDown += onKeyDown;


            var mapSize = sc.Scenario.MapConfig.Size;

            God = new GodBuilder
            {
                Position = mapSize / 2,
                Id = 100,
            };

            c.Engine.SetServer(this);

            var scData = ScenarioView.Scenario.GetBytes();
            var contentData = ScenarioView.Scenario.ZipContent();


            IOMessage msg = new HandshakeReplyMessage(true, scData, contentData);
            MessageSent(msg);

            //send obj owned
            msg = new PlayerStatusMessage(God.Id);
            MessageSent(msg);

            //send obj seen
            msg = new ObjectSeenMessage(God);
            MessageSent(msg);


            //send map
            foreach(var ch in MapChunkId.ChunksBetween(Vector.Zero, mapSize))
            {
                var datas = getChunkData(sc.Scenario.MapConfig, ch);
                msg = new MapDataMessage(ch, datas);
                MessageSent(msg);
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            const double zoomFactor = 1.05;

            switch(e.KeyCode)
            {
                case Keys.Subtract:
                    ScreenSize = ScreenSize * zoomFactor;
                    PanSpeed *= zoomFactor;
                    Client.SetCameraParams(null, ScreenSize);
                    break;
                case Keys.Add:
                    ScreenSize = ScreenSize / zoomFactor;
                    PanSpeed /= zoomFactor;
                    Client.SetCameraParams(null, ScreenSize);
                    break;
            }
        }

        void onMouseUp(object sender, MouseEventArgs e)
        {
            var life = 42;
        }

        void onMouseDown(object sender, MouseEventArgs e)
        {
            var life = 42;
        }

        void onMouseWheel(object sender, MouseEventArgs e)
        {
            ScreenSize *= (1 - e.Delta / 120);
            Client.SetCameraParams(null, ScreenSize);
        }

        TerrainType[] getChunkData(MapConfig map, MapChunkId ch)
        {
            var tty = new TerrainType[MapChunkId.ChunkSize.X * MapChunkId.ChunkSize.Y];
            var id = 0;
             for (var y = ch.BottomLeft.Y; y < ch.TopRight.Y; y++)
                for (var x = ch.BottomLeft.X; x < ch.TopRight.X; x++)
                    if(x < map.Width && y < map.Height)
                        tty[id++] = map.Terrain[x, y];

            return tty;
        }

        public void UpdateServer(int msElapsed)
        {

            Keyboard.Update();

            //no updates lel
            updateMovement(msElapsed);
        }

        void updateMovement(int msElapsed)
        {
            var x = b2i(Keyboard.GetKeyState(Keys.D)) - b2i(Keyboard.GetKeyState(Keys.A));
            var y = b2i(Keyboard.GetKeyState(Keys.S)) - b2i(Keyboard.GetKeyState(Keys.W));

            if (x == 0 && y == 0)
                return;

            var ang = new Vector(x, y).Angle;
            var dist = PanSpeed * msElapsed / 1000;
            God.Position = God.Position.PolarProjection(ang, dist);
        }


        static int b2i(bool b) { return b ? 1 : 0; }



        public string GetPerfData()
        {
            return "All is OK!";
        }
    }
}
