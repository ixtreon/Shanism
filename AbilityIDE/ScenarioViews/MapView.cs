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

namespace AbilityIDE.ScenarioViews
{
    public partial class MapView : ScenarioView
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Map;

        public MapConfig Map { get { return Scenario?.MapConfig; } }

        protected override async Task LoadScenario()
        {
            if (Map == null) return;

            chkInfinite.Checked = Map.Infinite;

            nWidth.Value = Map.Width;
            nHeight.Value = Map.Height;

            shanoMap1.SetMap(Scenario.MapConfig);
        }

        public override void SaveScenario()
        {
            if (Map == null) return;

            Map.Infinite = chkInfinite.Checked;
            //Map.Height = (int)nHeight.Value;
        }

        public MapView()
        {
            InitializeComponent();
            shanoMap1.MapRedrawn += ShanoMap1_MapRedrawn;

            pFiniteSettings.Top =
            pInfiniteSettings.Top =
                Math.Min(pFiniteSettings.Top, pInfiniteSettings.Top);
        }

        private void ShanoMap1_MapRedrawn()
        {
            MarkAsChanged();
        }

        private void chkInfinite_CheckedChanged(object sender, EventArgs e)
        {
            pFiniteSettings.Visible = !chkInfinite.Checked;
            pInfiniteSettings.Visible = chkInfinite.Checked;

            MarkAsChanged();
        }


        private void WidthHeight_ValueChanged(object sender, EventArgs e)
        {
            btnResizeMap.Visible = (nWidth.Value != Map.Width || nHeight.Value != Map.Height);
        }
        
        private void btnResizeMap_Click(object sender, EventArgs e)
        {
            //resize the map
            Map.ResizeMap((int)nWidth.Value, (int)nHeight.Value);
            shanoMap1.Invalidate();

            //mark the scenario as changed
            MarkAsChanged();

            //hide the button
            btnResizeMap.Hide();
        }

        private void btnMinMap_Click(object sender, EventArgs e)
        {
            mapSplitter.Panel1Collapsed = true;
            btnMaxMap.Visible = true;
        }

        private void btnMaxMap_Click(object sender, EventArgs e)
        {
            mapSplitter.Panel1Collapsed = false;
            btnMaxMap.Visible = false;
        }

        private void chkFixedSeed_CheckedChanged(object sender, EventArgs e)
        {
            //edit map + mark as edited
            txtFixedSeed.Enabled = chkFixedSeed.Enabled;
        }
    }
}
