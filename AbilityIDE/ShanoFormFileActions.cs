using AbilityIDE.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO;
using System.IO;
using ScenarioLib;

namespace AbilityIDE
{
    public partial class ShanoEditor : Form
    {
        const string WindowTitle = "ShanoEditor";

        public ScenarioFile Scenario { get; private set; }



        /// <summary>
        /// Opens an existing scenario. 
        /// </summary>
        private async void open(string filePath = null)
        {
            //ask the user to pick a file if none given
            if(string.IsNullOrEmpty(filePath) && openDialog.ShowDialog() == DialogResult.OK)
                filePath = openDialog.SelectedPath;

            //wtf
            if (string.IsNullOrEmpty(filePath))
                return;

            StatusLoading = true;
            //load a scenario, if any
            Scenario = await Task.Run(() => ScenarioFile.Load(filePath));
            if(Scenario == null)
            {
                MessageBox.Show("Unable to find a scenario in the directory '{0}'".F(filePath), "ShanoEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Settings.Default.UpdateRecentFiles(filePath);
            Directory.SetCurrentDirectory(filePath);

            //load up UI changes
            scenarioTree.Scenario = Scenario;
            foreach (var view in scenarioViews)
                view.Scenario = Scenario;

            updateUi();
            StatusLoading = false;
        }


        void save()
        {
            if (Scenario == null || !Scenario.IsDirty)
                return;
            foreach (var view in scenarioViews)
                view.SaveScenario();

            Scenario.Save();
            Scenario.IsDirty = false;

            updateUi();
        }

        void updateUi()
        {
            if(Scenario != null)
            {
                var editPostfix = Scenario.IsDirty ? " *" : "";
                Text = "{0} - {1}{2}".F(Scenario.Name, WindowTitle, editPostfix);
            }
            else
            {
                Text = WindowTitle;
            }
        }
    }
}
