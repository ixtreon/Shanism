using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.ScenarioLib;
using Shanism.Editor.MapAdapter;
using Shanism.Common.Objects;
using Shanism.Engine;

namespace Shanism.Editor.Views
{
    partial class MapView : ScenarioControl
    {


        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Map;

        bool clientLoaded = false;

        EditorControl ClientControl { get; }
        EditorController Engine { get; set; }

        public MapConfig Map => Model.Scenario.Config.Map;

        protected override async Task LoadModel()
        {
            if (Map == null) return;

            chkInfinite.Checked = Map.IsInfinite;
            chkInfinite_CheckedChanged(null, null);

            propPanel.LoadModel(Model.Content.Animations);

            nWidth.Value = Map.Width;
            nHeight.Value = Map.Height;

            if(clientLoaded)
                Engine.LoadScenario(Model);
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

            ClientControl = new EditorControl { Dock = DockStyle.Fill };
            Engine = new EditorController(ClientControl);
            Engine.MapChanged += onMapChanged;
            Engine.SelectionChanged += onSelectionChanged;

            ClientControl.ClientLoaded += () =>
            {
                clientLoaded = true;
                Engine.LoadScenario(Model);
            };

            mapSplitter.Panel2.Controls.Add(ClientControl);
        }

        void onSelectionChanged(IEnumerable<Entity> objs)
        {
            selectionWindow1.SetObjects(objs);
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

            var newSz = new Common.Point((int)nWidth.Value, (int)nHeight.Value);
            Engine.ResizeMap(newSz);
            //shanoMap1.SetMap(Map);
            //shanoMap1.Invalidate();

            //mark the scenario as changed
            MarkAsChanged();

            //hide the button
            btnResizeMap.Enabled = false;
            btnCancelMapResize.Enabled = false;
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

        private void pObjects_ObjectSelected(IEntity obj)
        {
            Engine.SetBrush(obj);
        }

        private void propPanel_BrushChanged(ObjectConstructor obj)
        {
            Engine.SetBrush(obj);
        }
    }
}
