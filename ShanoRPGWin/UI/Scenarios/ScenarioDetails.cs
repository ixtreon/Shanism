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

namespace ShanoRPGWin.UI.Scenarios
{
    public partial class ScenarioDetails : UserControl
    {
        public event Action<ScenarioConfig> ScenarioSelected;

        ScenarioConfig _scenario;
        public ScenarioConfig Scenario
        {
            get { return _scenario; }
            set
            {
                if(value != _scenario)
                {
                    _scenario = value;
                    OnScenarioChanged();
                }
            }
        }

        private void OnScenarioChanged()
        {
            lblName.Text = Scenario?.Name ?? "_";
            lblDescription.Text = Scenario?.Description ?? "_";
        }

        public ScenarioDetails()
        {
            InitializeComponent();
            OnScenarioChanged();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            ScenarioSelected?.Invoke(Scenario);
        }
    }
}
