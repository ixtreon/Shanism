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
    public partial class DetailsView : ScenarioView
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Details;

        protected override void LoadScenario()
        {
            txtName.Enabled =
            txtDescription.Enabled = (Scenario != null);

            txtName.Text = Scenario.Name ?? "";
            txtDescription.Text = Scenario.Description ?? "";
        }

        protected override void SaveScenario()
        {
            throw new NotImplementedException();
        }

        public DetailsView()
        {
            InitializeComponent();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            Scenario.Name = txtName.Text;
            MarkAsChanged();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            Scenario.Description = txtDescription.Text;
            MarkAsChanged();
        }
    }
}
