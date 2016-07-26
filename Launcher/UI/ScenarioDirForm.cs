using Shanism.ScenarioLib;
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
        ScenarioConfig selectedScenario;

        ScenarioLibrary selectedLibrary;

        /// <summary>
        /// Gets the scenario which the user ultimately chose. 
        /// </summary>
        public ScenarioConfig ChosenScenario { get; private set; }

        public event Action ScenariosLoaded;

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

        /// <summary>
        /// Tries to find the given scenario. 
        /// </summary>
        public ScenarioConfig FindScenario(string path)
        {
            return libTree.FindScenario(path);
        }

        /// <summary>
        /// Reloads all libraries and their scenarios using the library list in the app config. 
        /// </summary>
        public async Task LoadScenarios()
        {
            await libTree.LoadAsync();

            ScenariosLoaded?.Invoke();
        }

        #region libTree event handlers
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

        void LibTree_SelectedScenario(ScenarioConfig sc, ScenarioLibrary lib)
        {
            //show scenario
            scenarioDetails.Scenario = sc;
            scenarioDetails.Show();
            selectedScenario = sc;
            //hide library
            libraryDetails.Hide();
            selectedLibrary = lib;
        }
        #endregion

        #region Add/Refresh/Remove event handlers
        async void btnAddLibrary_Click(object sender, EventArgs e)
        {
            var result = folderDialog.ShowDialog();
            if(result == DialogResult.OK && Directory.Exists(folderDialog.SelectedPath))
            {
                await libTree.AddLibrary(folderDialog.SelectedPath);
            }
        }

        async void btnRefresh_Click(object sender, EventArgs e)
        {
            await libTree.RefreshLibs();
        }

        async void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedScenario != null)   //delete a scenario
            {
                if (MessageBox.Show(
                    "This will DELETE the '" + selectedScenario.Name + "' scenario from your LOCAL DRIVE. \nAre you sure you want to proceed?",
                    "Remove a scenario",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    libTree.RemoveScenario(selectedScenario.BaseDirectory);
                    await libTree.RefreshLibs();
                }
            }
            else if (selectedLibrary != null)   //delete a library
            {
                if (MessageBox.Show(
                    "This will remove the library '" + selectedLibrary.DirectoryPath + "' from your local list of libraries. \nAre you sure you want to proceed?",
                    "Remove a library",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    libTree.RemoveLibrary(selectedLibrary.DirectoryPath);
                    await libTree.RefreshLibs();
                }
            }
        }
        #endregion

        #region Other event handlers
        void scenarioDetails_ScenarioSelected(ScenarioConfig sc)
        {
            //selected a scenario => set dialog result and hide
            ChosenScenario = sc;
            DialogResult = DialogResult.OK;
            Hide();
        }
        async void ScenarioDirForm_Load(object sender, EventArgs e)
        {
            await LoadScenarios();
        }
        #endregion
    }
}
