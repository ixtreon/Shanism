using IO;
using IO.Common;
using IO.Objects;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShanoEditor;

using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using Graphics = System.Drawing.Graphics;
using Font = System.Drawing.Font;
using SolidBrush = System.Drawing.SolidBrush;
using ShanoEditor.ViewModels;
using ShanoEditor.Views.Maps.Layers;

namespace ShanoEditor.Views.Maps
{
     partial class ShanoMap : UserControl, IScenarioControl
    {

        static readonly Color BrushShadowColor = Color.SkyBlue.SetAlpha(0.5f);


        WorldView View { get; } = new WorldView();

        public TerrainType? TerrainBrush { get; set; }

        public IGameObject ObjectBrush { get; set; }

        public int TerrainBrushSize { get; set; } = 1;


        public event Action MapModified;


        public MapConfig Map { get; private set; }

        Vector? cursorMapLocation = null;  //in in-game coordinates
        Point? lastCursorPanLocation = null; //in pixels


        Point BrushShadowPosition;
        Point BrushShadowSize;

        TerrainLayer terrain { get; }
        ObjectLayer objects { get; }


        #region IScenarioControl implementation
        public ScenarioViewModel Model { get; private set; }

        public void SetModel(ScenarioViewModel model)
        {
            Model = model;

            objects.SetModel(model);
        }
        #endregion


        public ShanoMap()
        {
            InitializeComponent();

            DoubleBuffered = true;
            ResizeRedraw = true;

            terrain = new TerrainLayer(View);
            objects = new ObjectLayer(View);
        }


        public void SetMap(MapConfig map)
        {
            //inifite maps are not supported
            if (map.Infinite)
            {
                Map = null;
                Invalidate();
                return;
            }
            Map = map;

            //world view
            View.ResizeMap(map.Width, map.Height);

            //terrain layer
            terrain.Load(map);
            objects.Load(map);

            Invalidate();
        }

        void doCursorAction()
        {
            if (cursorMapLocation == null)
                return;

            //check brush type
            if (TerrainBrush != null)
            {
                //modify the underlying map
                foreach(var x in Enumerable.Range(BrushShadowPosition.X, BrushShadowSize.X))
                foreach(var y in Enumerable.Range(BrushShadowPosition.Y, BrushShadowSize.Y))
                    Map.Terrain[x, y] = TerrainBrush.Value;

                //and our internal model/rendering of it
                terrain.SetPixels(TerrainBrush.Value, BrushShadowPosition, BrushShadowSize);
                
                //invoke the event
                MapModified?.Invoke();
            }
            else if (ObjectBrush != null)
            {
                Map.ObjectList.Add(new ObjectConstructor(ObjectBrush, cursorMapLocation.Value));

                //invoke the event
                MapModified?.Invoke();
            }

            Invalidate();
        }

        void updateCursorPosition()
        {
            if (Map == null)
                return;
            //get cursor position
            var p = PointToClient(Cursor.Position);
            Vector? inGame = View.ControlToGame(new Vector(p.X, p.Y));
            if (!inGame.Value.Inside(Point.Zero, Map.Size))
                inGame = null;

            cursorMapLocation = inGame;

            if (cursorMapLocation != null)
            {
                //brush shadow
                BrushShadowPosition = (cursorMapLocation.Value - TerrainBrushSize / 2.0)
                        .Round()
                        .Clamp(Point.Zero, Map.Size - 1);

                var brushFar = (cursorMapLocation.Value + TerrainBrushSize / 2.0)
                        .Round()
                        .Clamp(Point.Zero, Map.Size);

                BrushShadowSize = brushFar - BrushShadowPosition;
            }
            else
                BrushShadowSize = Point.Zero;
        }


        #region Drawing stuff

        static readonly Font InfoBoxFont = 
            new Font(DefaultFont.FontFamily, 12, System.Drawing.FontStyle.Regular);

        static readonly Color InfoBoxBackColor = Color.DarkGray.SetAlpha(150);
        static readonly Color InfoBoxForeColor = Color.White;


        void drawInfoBox(Graphics g)
        {
            var text = ("Cursor: {0}"
                + "\n" + "Terrain: {1}"
                + "\n" + "FPS: {2:00.0}"
                ).F(cursorMapLocation, "???", curFps);

            //text size
            var sz = g.MeasureString(text, InfoBoxFont);
            sz += new System.Drawing.SizeF(6, 6);

            //background
            var bgPos = (System.Drawing.PointF)(Size - sz);
            using (var br = new SolidBrush(InfoBoxBackColor))
                g.FillRectangle(br, new System.Drawing.RectangleF(bgPos, sz));

            //foreground
            var txtPos = bgPos + new System.Drawing.SizeF(3, 3);
            using (var br = new SolidBrush(InfoBoxForeColor))
                g.DrawString(text, InfoBoxFont, br, txtPos);
        }

        //void drawObjects(Graphics g)
        //{
        //    foreach(var obj in Map.ObjectList)
        //    {
        //        var go = Model.Scenario.TryGet(obj.TypeName);
        //        var anim = Model.Content.ModelDefaultAnimations.TryGet(go?.Model);

        //        var pos = View.GameToControl(obj.Location);
        //        var sz = go.Size * View.UnitSize;

        //        anim.Paint(g, new RectangleF(pos - sz / 2, new Vector(sz)));
        //    }
        //}

        //draws mouse cursor hover
        void drawCursor(Graphics g)
        {
            if (cursorMapLocation != null)
            {
                // if terrain, draw shaded tile background 
                if (TerrainBrush != null)
                {
                    var pos = View.GameToControl(BrushShadowPosition);
                    var sz = (Vector)BrushShadowSize * View.UnitSize;

                    using (var br = new SolidBrush(BrushShadowColor))
                        g.FillRectangle(br, new RectangleF(pos, sz).ToNetRectangle());
                }

                // if object, draw it under cursor. 
                if (ObjectBrush != null)
                {
                    var anim = Model?.Content.ModelDefaultAnimations.TryGet(ObjectBrush.ModelName);

                    if (anim != null)
                    {
                        var ll = View.GameToControl(cursorMapLocation.Value - new Vector(ObjectBrush.Scale) / 2);
                        var ur = View.GameToControl(cursorMapLocation.Value + new Vector(ObjectBrush.Scale) / 2);
                        anim.Paint(g, new RectangleF(ll, ur - ll));
                    }
                }
            }
        }
        #endregion


        #region Event Handlers

        private void ShanoMap_Click(object sender, EventArgs e)
        {
            label1.Focus();
        }

        //mouse panning + continuous object placement
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //pan
            if (lastCursorPanLocation != null)
            {
                var mouseNow = new Point(e.X, e.Y);
                var diff = mouseNow - lastCursorPanLocation.Value;

                View.Pan(diff, Vector.Zero, Map.Size);

                lastCursorPanLocation = mouseNow;
            }

            //mouse hover
            updateCursorPosition();

            //action
            if (e.Button == MouseButtons.Left)
            {
                doCursorAction();
            }

            Invalidate();
            base.OnMouseMove(e);
        }

        //map panning + one-time object placement
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //draw
                doCursorAction();
            }
            else
            {
                //pan!
                Cursor = Cursors.SizeAll;
                lastCursorPanLocation = new Point(e.X, e.Y);
            }
            base.OnMouseDown(e);
        }

        //map panning stop
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Cursor = Cursors.Cross;
            lastCursorPanLocation = null;
            base.OnMouseUp(e);
        }

        //focus on mouse enter
        protected override void OnMouseEnter(EventArgs e)
        {
            Focus();
            base.OnMouseEnter(e);
        }

        //zoom in or out
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            View.Scale(e.Delta);

            updateCursorPosition();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            View.ResizeScreen(Width, Height);
        }

        double curFps = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (Map == null)
                return;

            var sw = System.Diagnostics.Stopwatch.StartNew();

            terrain.Draw(g);

            objects.Draw(g);
            //drawObjects(g);

            drawCursor(g);

            drawInfoBox(g);

            sw.Stop();

            curFps = 1000 / sw.ElapsedMilliseconds;
        }

        #endregion
    }
}
