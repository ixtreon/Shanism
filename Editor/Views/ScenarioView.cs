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

namespace Shanism.Editor.Views
{
    partial class ScenarioView : ScenarioControl
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Details;

        protected override async Task LoadModel()
        {
            txtName.Enabled =
            txtDescription.Enabled = (Model != null);

            txtName.Text = Model.Scenario.Config.Name ?? "";
            txtDescription.Text = Model.Scenario.Config.Description ?? "";
        }

        protected override void SaveModel()
        {
            Model.Scenario.Config.Name = txtName.Text;
            Model.Scenario.Config.Description = txtDescription.Text;
        }

        public ScenarioView()
        {
            InitializeComponent();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            MarkAsChanged();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            MarkAsChanged();
        }
    }
}
