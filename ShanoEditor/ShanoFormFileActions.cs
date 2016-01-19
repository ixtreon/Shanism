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


        async void create()
        {
            var dialog = new SaveFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                Filter = "Folders|%",
                FileName = "CustomScenario",
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            await create(dialog.FileName);
        }

        async Task create(string filePath)
        {
            CompiledScenario sc;
            try
            {
                sc = new CompiledScenario(filePath);
            }
            catch
            {
                MessageBox.Show("Unable to create a scenario at the given directory! ('{0}')".F(filePath));
                return;
            }

            await open(sc);
        }

        private async Task open()
        {
            var dialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                Filter = "Folders|%",
                FileName = "Pick a folder",
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var filePath = Path.GetDirectoryName(dialog.FileName);
            await open(filePath);
        }

        /// <summary>
        /// Opens an existing scenario. 
        /// </summary>
        private async Task open(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            //load a scenario, if any
            StatusLoading = true;
            Model = null;

            CompiledScenario scenario = null;
            string _error = string.Empty;
            try
            {
                scenario = await Task.Run(() => CompiledScenario.Load(filePath));
                if (scenario == null)
                    _error = "Cannot find a scenario at `{0}`. ".F(filePath);
            }
            catch (Exception e)
            {
                //_error = e.Message;
                _error = "Bad scenario data. ";
            }

            if (scenario == null)
            {
                MessageBox.Show("Unable to load the scenario: \n\n{0}".F(_error), "ShanoEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //update UI
            Settings.Default.UpdateRecentFiles(filePath);

            await open(scenario);
        }

        private async Task open(CompiledScenario scenario)
        {
            //create the viewmodel
            Model = new ScenarioViewModel(scenario);
            await Model.Load();

            //set the views' model
            scenarioTree.Scenario = Model.Scenario;
            foreach (var view in scenarioViews)
                view.SetModel(Model);

            updateCaption();
            StatusLoading = false;
        }


        async Task save()
        {
            if (Model == null || !Model.IsDirty)
                return;

            //inform views
            foreach (var view in scenarioViews)
                view.Save();

            //save model
            await Model.Save();

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
