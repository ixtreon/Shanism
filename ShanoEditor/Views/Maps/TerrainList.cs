using IO;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.Views.Maps
{
    partial class TerrainList : ScenarioControl
    {
        public event Action<TerrainType> BrushTypeSelected;

        public event Action<int> BrushSizeSelected;


        TerrainTypeButton[] terrainButtons { get; }
        TerrainSizeButton[] sizeButtons { get; }

        public TerrainList()
        {
            InitializeComponent();

            sizeButtons = Enumerable.Range(1, 8)
                .Select(sz =>
                {
                    var btn = new TerrainSizeButton(sz);
                    toolTip.SetToolTip(btn, "{0}x{0}".F(sz));
                    btn.CheckedChanged += sizeBtn_CheckedChanged;
                    return btn;
                })
                .ToArray();
            sizeButtons[0].Checked = true;
            pSizes.Controls.AddRange(sizeButtons);


            terrainButtons = Enum.GetValues(typeof(TerrainType))
                .Cast<TerrainType>()
                .Select(tty =>
                {
                    var btn = new TerrainTypeButton(tty);
                    toolTip.SetToolTip(btn, tty.ToString());
                    btn.CheckedChanged += terrainBtn_CheckedChanged;
                    return btn;
                })
                .ToArray();
            terrainButtons[0].Checked = true;

            pTerrain.Controls.AddRange(terrainButtons);
        }

        private void terrainBtn_CheckedChanged(object sender, EventArgs e)
        {
            var btn = (TerrainTypeButton)sender;

            if(btn.Checked)
                BrushTypeSelected?.Invoke(btn.Object);
        }

        private void sizeBtn_CheckedChanged(object sender, EventArgs e)
        {
            var btn = (TerrainSizeButton)sender;

            if(btn.Checked)
                BrushSizeSelected?.Invoke(btn.BrushSize);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if(Visible)
            {
                if (sizeButtons.All(b => !b.Checked))
                    sizeButtons[0].Checked = true;

                if (terrainButtons.All(b => !b.Checked))
                    terrainButtons[0].Checked = true;
            }
        }


        class TerrainTypeButton : ObjectButton<TerrainType>
        {
            public TerrainTypeButton(TerrainType tty)
                : base(tty, 48)
            {
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                Text = tty.ToString();
                Margin = new Padding(2);
            }
        }

        class TerrainSizeButton : ObjectButton<int>
        {
            const int ButtonSize = 34;
            const int SquareSize = 3;

            public int BrushSize { get { return Object; } }

            public TerrainSizeButton(int brushSz)
                : base(brushSz, ButtonSize)
            {
                Margin = new Padding(2);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                var sz = (SquareSize) * BrushSize;
                var pos = (float)(ButtonSize - sz) / 2;

                //draw fancy brush image
                foreach(var ix in Enumerable.Range(0, BrushSize))
                    foreach(var iy in Enumerable.Range(0, BrushSize))
                    {
                        var x = pos + ix * (SquareSize);
                        var y = pos + iy * (SquareSize);
                        var br = ((x + y) % 2 == 0) 
                            ? System.Drawing.Brushes.Black 
                            : System.Drawing.Brushes.DarkGray;
                        e.Graphics.FillRectangle(br, x, y, SquareSize, SquareSize);
                    }
            }
        }
    }
}
