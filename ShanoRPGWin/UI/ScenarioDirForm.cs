using ScenarioLib;
using ShanoRPGWin.UI.Scenarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoRPGWin.UI
{
    public partial class ScenarioDirForm : Form
    {
        ScenarioBase selectedScenario;

        ScenarioLibrary selectedLibrary;

        /// <summary>
        /// Gets the scenario which the user ultimately chose. 
        /// </summary>
        public ScenarioBase ChosenScenario { get; private set; }

        public ScenarioDirForm()
        {
            InitializeComponent();

            //setup right pane
            scenarioDetails.Dock = DockStyle.Fill;
            libraryDetails.Dock = DockStyle.Fill;
            LibTree_SelectionCleared();

            //tree events for right pane
            libTree.SelectedScenario += LibTree_SelectedScenario;
            libTree.SelectedLibrary += LibTree_SelectedLibrary;
            libTree.SelectionCleared += LibTree_SelectionCleared;
        }

        void ScenarioDirForm_VisibleChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Tries to find the given scenario. 
        /// </summary>
        public ScenarioBase FindScenario(string path)
        {
            return libTree.FindScenario(path);
        }


        void LibTree_SelectedLibrary(ScenarioLibrary lib)
        {
            //show library
            libraryDetails.Show();
            selectedLibrary = lib;
            //hide scenario
            scenarioDetails.Hide();
            selectedScenario = null;
        }

        void LibTree_SelectionCleared()
        {
            //hide scenario
            libraryDetails.Hide();
            selectedLibrary = null;
            //hide library
            scenarioDetails.Hide();
            selectedScenario = null;
        }

        void LibTree_SelectedScenario(ScenarioBase sc, ScenarioLibrary lib)
        {
            //show scenario
            scenarioDetails.Scenario = sc;
            scenarioDetails.Show();
            selectedScenario = sc;
            //hide library
            libraryDetails.Hide();
            selectedLibrary = lib;
        }

        void btnAddLibrary_Click(object sender, EventArgs e)
        {
            var result = folderDialog.ShowDialog();
            if(result == DialogResult.OK && Directory.Exists(folderDialog.SelectedPath))
            {
                libTree.AddLibrary(folderDialog.SelectedPath);
            }
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {
            libTree.RefreshLibs();
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedScenario != null)
            {
                if (MessageBox.Show(
                    "This will DELETE the '" + selectedScenario.Name + "' scenario from your LOCAL DRIVE. \nAre you sure you want to proceed?",
                    "Remove a scenario",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    libTree.RemoveScenario(selectedScenario.ScenarioDirectory);
                    libTree.RefreshLibs();
                }
            }
            else if (selectedLibrary != null)
            {
                if (MessageBox.Show(
                    "This will remove the library '" + selectedLibrary.DirectoryPath + "' from your local list of libraries. \nAre you sure you want to proceed?",
                    "Remove a library",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    libTree.RemoveLibrary(selectedLibrary.DirectoryPath);
                    libTree.RefreshLibs();
                }
            }
        }

        void scenarioDetails_ScenarioSelected(ScenarioBase sc)
        {
            ChosenScenario = sc;
            DialogResult = DialogResult.OK;
            Hide();
        }
    }
}
