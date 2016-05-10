using Shanism.Common;
using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.Views.Maps
{
    partial class TerrainList : ScenarioControl
    {

        readonly TerrainTypeButton[] terrainButtons;


        /// <summary>
        /// True if circle, false if square. 
        /// </summary>
        public bool IsCircle { get; private set; }

        public int TerrainSize { get; private set; } = 1;

        public TerrainType TerrainType { get; private set; } = TerrainType.Dirt;


        /// <summary>
        /// Raised whenever a different TerrainType is selected. 
        /// </summary>
        public event Action TerrainBrushChanged;

        public TerrainList()
        {
            InitializeComponent();

            terrainButtons = Enum<TerrainType>.Values
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

        void terrainBtn_CheckedChanged(object sender, EventArgs e)
        {
            var btn = (TerrainTypeButton)sender;

            if (btn.Checked)
            {
                TerrainType = btn.Object;
                TerrainBrushChanged?.Invoke();
            }
        }

        void onSizeTrackbarValueChanged(object sender, EventArgs e)
        {
            lblBrushSize.Text = trackBar1.Value.ToString();

            TerrainSize = trackBar1.Value;
            TerrainBrushChanged?.Invoke();
        }

        private void TerrainList_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible) return;
            TerrainBrushChanged?.Invoke();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            IsCircle = btnCircleShape.Checked;
            TerrainBrushChanged?.Invoke();
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

        private void TerrainList_Load(object sender, EventArgs e)
        {
            TerrainBrushChanged?.Invoke();
        }
    }
}
