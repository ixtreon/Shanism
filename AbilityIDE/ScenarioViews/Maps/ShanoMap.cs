using IO.Common;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbilityIDE.ScenarioViews.Maps
{
    public partial class ShanoMap : UserControl
    {
        static readonly Dictionary<TerrainType, System.Drawing.Color> terrainColors = new Dictionary<TerrainType, System.Drawing.Color>
        {
            { TerrainType.DeepWater, System.Drawing.Color.DarkBlue },
            { TerrainType.Dirt, System.Drawing.Color.SaddleBrown },
            { TerrainType.Grass, System.Drawing.Color.LightGreen },
            { TerrainType.None, System.Drawing.Color.Black },
            { TerrainType.Sand, System.Drawing.Color.Yellow },
            { TerrainType.Snow, System.Drawing.Color.White },
            { TerrainType.Stone, System.Drawing.Color.Gray },
            { TerrainType.Water, System.Drawing.Color.Blue },
        };

        /// <summary>
        /// Gets the in-game point that is in the middle of the screen. 
        /// </summary>
        public Vector CenterPoint { get; private set; }

        public TerrainType Brush { get; set; } = TerrainType.Grass;

        public int BrushSize { get; set; } = 1;

        public event Action MapRedrawn;

        /// <summary>
        /// Gets the size in pixels of one in-game unit. 
        /// </summary>
        public double UnitSize { get; private set; } = 5;

        public MapConfig Map { get; private set; }



        Vector HalfSize {  get { return new Vector(Width, Height) / 2; } }

        Vector LowLeftPoint
        {
            get
            {
                return CenterPoint - HalfSize / UnitSize;
            }
        }

        Vector HighRightPoint
        {
            get
            {
                return CenterPoint + HalfSize / UnitSize;
            }
        }

        Vector gameToControl(Vector gamePos)
        {
            return HalfSize + (gamePos - CenterPoint) * UnitSize;
        }

        Vector controlToGame(Vector controlPos)
        {
            return CenterPoint + (controlPos - HalfSize) / UnitSize;
        }

        public ShanoMap()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;

            //populate the color picker
            ToolStripItem selectedColor = null;
            foreach(var clr in terrainColors)
            {
                var item = colorPickerMenu.DropDownItems.Add(clr.Key.ToString());
                item.Click += (o, e) =>
                {
                    colorPickerMenu.BackColor = clr.Value;
                    item.Font = new System.Drawing.Font(item.Font, System.Drawing.FontStyle.Bold);

                    if(selectedColor != null && selectedColor != item) 
                        selectedColor.Font = new System.Drawing.Font(item.Font, System.Drawing.FontStyle.Regular);

                    selectedColor = item;
                    Brush = clr.Key;
                };
            }
            colorPickerMenu.DropDownItems[0].PerformClick();

            tsBrushSize.SelectedIndex = 0;
        }

        public void SetMap(MapConfig map)
        {
            if(map.Infinite)
            {
                Map = null;
                return;
            }

            Map = map;
            CenterPoint = new Vector(map.Width / 2.0, map.Height / 2.0);
            UnitSize = 5;

            Invalidate();
        }

        Point? cursorHover = null;
        Point? mousePanning = null;

        void draw()
        {
            var cp = PointToClient(Cursor.Position);
            var gp = (controlToGame(new Vector(cp.X, cp.Y)) - BrushSize / 2).ToPoint();


            foreach(var ix in Enumerable.Range(gp.X, BrushSize))
            foreach(var iy in Enumerable.Range(gp.Y, BrushSize))
                if (ix >= 0 && ix < Map.Width && iy >= 0 && iy < Map.Height)
                    Map.Map[ix, iy] = Brush;

            MapRedrawn?.Invoke();
            Invalidate();
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            var mouseNow = new Point(e.X, e.Y);

            if (mousePanning != null)
            {
                var diff = (Vector)(mouseNow - mousePanning.Value) / UnitSize;
                CenterPoint -= diff;
                mousePanning = mouseNow;
            }

            if (e.Button == MouseButtons.Left)
                draw();

            Invalidate();
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                //draw
                draw();
            }
            else
            {
                //pan!
                Cursor = Cursors.SizeAll;
                mousePanning = new Point(e.X, e.Y);
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Cursor = Cursors.Cross;
            mousePanning = null;
            base.OnMouseUp(e);
        }

        void recalculateHoverPoint()
        {
            var p = PointToClient(Cursor.Position);
            var inGame = controlToGame(new Vector(p.X, p.Y) - BrushSize / 2);
            if (0 <= inGame.X && inGame.X <= Map.Width
                && 0 <= inGame.Y && inGame.Y <= Map.Height)
                cursorHover = inGame.Floor();
            else
                cursorHover = null;
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(System.Drawing.Color.Black);

            if (Map == null) return;

            //mouse hover
            recalculateHoverPoint();


            //map
            var llGame = LowLeftPoint.Floor().Clamp(Point.Zero, Map.Size - 1);
            var urGame = HighRightPoint.Ceiling().Clamp(Point.Zero, Map.Size - 1);
            foreach (var p in llGame.IterateToInclusive(urGame))
            {
                var tt = Map.Map[p.X, p.Y];
                var c = terrainColors[tt];
                var lowLeft = gameToControl(p);

                //mouse hover
                if (p == cursorHover)
                    c = System.Drawing.Color.Aqua;

                using(var br = new System.Drawing.SolidBrush(c))
                    g.FillRectangle(br, (float)lowLeft.X, (float)lowLeft.Y, (float)UnitSize, (float)UnitSize);
            }

        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            const double scale = 0.002;

            if(e.Delta > 0)
                UnitSize *= (1 + scale * e.Delta);
            else
                UnitSize /= (1 + scale * -e.Delta);

            Invalidate();
        }

        private void tsBrushSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrushSize = (tsBrushSize.SelectedIndex + 1);
        }

        private void ShanoMap_Click(object sender, EventArgs e)
        {
            label1.Focus();
        }
    }
}
