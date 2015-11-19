using ShanoEditor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO;
using System.IO;
using ScenarioLib;
using ShanoEditor.ViewModels;

namespace ShanoEditor
{
    public partial class ShanoEditorForm : Form
    {
        const string WindowTitle = "ShanoEditor";

        ScenarioViewModel Model { get; set; }



        /// <summary>
        /// Opens an existing scenario. 
        /// </summary>
        private async void open(string filePath = null)
        {
            var dialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                Filter = "Folders|%",
                FileName = "Pick a folder",
            };

            //ask the user to pick a file if none given
            if(string.IsNullOrEmpty(filePath))
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = Path.GetDirectoryName(dialog.FileName);
                }
            }



            //wtf
            if (string.IsNullOrEmpty(filePath))
                return;

            //load a scenario, if any
            StatusLoading = true;
            Model = null;

            CompiledScenario scenario = null;
            string _error = string.Empty;
            try
            {
                scenario = await Task.Run(() => CompiledScenario.Load(filePath));
                if(scenario == null)
                    _error = "Cannot find a scenario at `{0}`. ".F(filePath);
            }
            catch(Exception e)
            {
                _error = e.Message;
            }

            if(scenario == null)
            {
                MessageBox.Show("Unable to load the scenario: \n\n{0}".F(_error), "ShanoEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //why set the directory?..
            Directory.SetCurrentDirectory(filePath);

            //create the viewmodel
            Model = new ScenarioViewModel(scenario);
            await Model.Load();

            //set the views' model
            scenarioTree.Scenario = Model.Scenario;
            foreach (var view in scenarioViews)
                view.SetModel(Model);

            //update UI
            Settings.Default.UpdateRecentFiles(filePath);

            updateCaption();
            StatusLoading = false;
        }


        void save()
        {
            if (Model == null || !Model.IsDirty)
                return;

            //inform views
            foreach (var view in scenarioViews)
                view.Save();

            //save model
            Model.Save();
            Model.IsDirty = false;

            //update ui
            updateCaption();
        }

        void updateCaption()
        {
            if(Model != null)
            {
                var editPostfix = Model.IsDirty ? " *" : "";
                Text = "{0} - {1}{2}".F(Model.Scenario.Name, WindowTitle, editPostfix);
            }
            else
            {
                Text = WindowTitle;
            }
        }
    }
}
