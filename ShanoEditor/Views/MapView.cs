using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScenarioLib;

namespace ShanoEditor.Views
{
    partial class MapView : ScenarioControl
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Map;

        public MapConfig Map { get { return Model.Scenario.MapConfig; } }

        protected override async Task LoadModel()
        {
            if (Map == null) return;

            shanoMap1.SetModel(Model);

            chkInfinite.Checked = Map.Infinite;
            chkInfinite_CheckedChanged(null, null);

            nWidth.Value = Map.Width;
            nHeight.Value = Map.Height;
            
            shanoMap1.SetMap(Model.Scenario.MapConfig);
        }


        protected override async Task SaveModel()
        {
            if (Map == null) return;

            Map.Infinite = chkInfinite.Checked;
        }

        public MapView()
        {
            InitializeComponent();

            //post-designer fixes
            pFiniteSettings.Top =
            pInfiniteSettings.Top =
                Math.Min(pFiniteSettings.Top, pInfiniteSettings.Top);

            shanoMap1.MapModified += ShanoMap1_MapRedrawn;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            listPanel1.Dock = DockStyle.Fill;
            listPanel1.Width = mapSplitter.Panel1.Width;

            listPanel1.AddPanel(pMapSettings, "Settings");
            listPanel1.AddPanel(pTerrain, "Terrain");
            listPanel1.AddPanel(pObjects, "Objects");
        }

        private void ShanoMap1_MapRedrawn()
        {
            MarkAsChanged();
        }

        private void chkInfinite_CheckedChanged(object sender, EventArgs e)
        {
            pFiniteSettings.Visible = !chkInfinite.Checked;
            pInfiniteSettings.Visible = chkInfinite.Checked;

            if (chkInfinite.Checked)
                pMapSettings.Height = pInfiniteSettings.Bottom + 12;
            else
                pMapSettings.Height = pFiniteSettings.Bottom + 12;
            MarkAsChanged();
        }


        private void WidthHeight_ValueChanged(object sender, EventArgs e)
        {
            btnResizeMap.Enabled = 
            btnCancelMapResize.Enabled =
                (nWidth.Value != Map.Width || nHeight.Value != Map.Height);
        }
        
        private void btnResizeMap_Click(object sender, EventArgs e)
        {
            //resize the map
            Map.ResizeMap((int)nWidth.Value, (int)nHeight.Value);
            shanoMap1.SetMap(Map);
            shanoMap1.Invalidate();

            //mark the scenario as changed
            MarkAsChanged();

            //hide the button
            btnResizeMap.Enabled = false;
            btnCancelMapResize.Enabled = false;
        }
        

        private void btnMaxTools_Click(object sender, EventArgs e)
        {
            if(mapSplitter.Panel1Collapsed)
            {
                mapSplitter.Panel1Collapsed = false;
                btnMaxTools.Text = "◀";
            }
            else
            {
                mapSplitter.Panel1Collapsed = true;
                btnMaxTools.Text = "▶";
            }
        }

        private void chkFixedSeed_CheckedChanged(object sender, EventArgs e)
        {
            //edit map + mark as edited
            txtFixedSeed.Enabled = chkFixedSeed.Enabled;
        }

        private void btnCancelMapResize_Click(object sender, EventArgs e)
        {
            nWidth.Value = Map.Width;
            nHeight.Value = Map.Height;
        }

        private void pTerrain_BrushTypeSelected(IO.Common.TerrainType val)
        {
            shanoMap1.ObjectBrush = null;
            shanoMap1.TerrainBrush = val;
        }

        private void pTerrain_BrushSizeSelected(int val)
        {
            shanoMap1.TerrainBrushSize = val;
        }

        private void pObjects_ObjectSelected(IO.Objects.IGameObject obj)
        {
            shanoMap1.TerrainBrush = null;
            shanoMap1.ObjectBrush = obj;
        }

        private void pTerrain_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void pObjects_VisibleChanged(object sender, EventArgs e)
        {

        }
    }
}
