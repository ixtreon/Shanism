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
    partial class DetailsView : ScenarioControl
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Details;

        protected override async Task LoadModel()
        {
            txtName.Enabled =
            txtDescription.Enabled = (Model != null);

            txtName.Text = Model.Scenario.Name ?? "";
            txtDescription.Text = Model.Scenario.Description ?? "";
        }

        protected override async Task SaveModel()
        {
            Model.Scenario.Name = txtName.Text;
            Model.Scenario.Description = txtDescription.Text;
        }

        public DetailsView()
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
