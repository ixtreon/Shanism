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
using ShanoEditor.MapAdapter;

namespace ShanoEditor.Views
{
    partial class MapView : ScenarioControl
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Map;

        public MapConfig Map { get { return Model.Scenario.Config.Map; } }


        EditorControl Client { get; }
        EditorEngine Engine { get; set; }

        protected override async Task LoadModel()
        {
            if (Map == null) return;

            chkInfinite.Checked = Map.IsInfinite;
            chkInfinite_CheckedChanged(null, null);

            propPanel.LoadModel(Model.Content.Animations);

            nWidth.Value = Map.Width;
            nHeight.Value = Map.Height;
        }


        protected override void SaveModel()
        {
            if (Map == null) return;

            Map.IsInfinite = chkInfinite.Checked;
        }

        public MapView()
        {
            InitializeComponent();

            //post-designer fixes
            pFiniteSettings.Top =
            pInfiniteSettings.Top =
                Math.Min(pFiniteSettings.Top, pInfiniteSettings.Top);

            //shanoMap1.MapModified += ShanoMap1_MapRedrawn;

            Client = new EditorControl { Dock = DockStyle.Fill };
            Engine = new EditorEngine(Client);
            Engine.MapChanged += onMapChanged;

            Client.GameLoaded += () =>
            {
                Engine.LoadScenario(Model);
            };

            mapSplitter.Panel2.Controls.Add(Client);
        }

        void onMapChanged()
        {
            MarkAsChanged();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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

        void btnResizeMap_Click(object sender, EventArgs e)
        {
            //resize the map

            var newSz = new IO.Common.Point((int)nWidth.Value, (int)nHeight.Value);
            Engine.ResizeMap(newSz);
            //shanoMap1.SetMap(Map);
            //shanoMap1.Invalidate();

            //mark the scenario as changed
            MarkAsChanged();

            //hide the button
            btnResizeMap.Enabled = false;
            btnCancelMapResize.Enabled = false;
        }


        private void btnMaxTools_Click(object sender, EventArgs e)
        {
            if (mapSplitter.Panel1Collapsed)
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

        private void onTerrainBrushChanged()
        {
            Engine.SetBrush(pTerrain.TerrainType, pTerrain.TerrainSize, pTerrain.IsCircle);
        }

        private void pObjects_ObjectSelected(IO.Objects.IEntity obj)
        {
            Engine.SetBrush(obj);
        }

        private void propPanel_BrushChanged(ObjectConstructor obj)
        {
            Engine.SetBrush(obj);
        }
    }
}
