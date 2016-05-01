﻿using ShanoEditor.Properties;
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
using ShanoEditor.Views;

namespace ShanoEditor
{
    public partial class ShanoEditorForm : Form
    {
        const string WindowTitle = "ShanoEditor";

        ScenarioViewModel Model { get; set; }


        async void create()
        {
            const string placeholderText = "Folder Selection";

            //save currently opened scenario, if any
            var wasClosed = await close();
            if (!wasClosed)
                return;

            //modify the UI
            StatusLoading = true;

            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                ValidateNames = false,
                Filter = "Folders|%",
                FileName = placeholderText,
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var sc = new CompiledScenario(dialog.FileName);

            await open(sc);

            StatusLoading = false;
        }

        async Task open()
        {
            var wasClosed = await close();
            if (!wasClosed)
                return;

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
        async Task open(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            //load a scenario, if any
            StatusLoading = true;
            Model = null;

            string scenarioLoadErrors = string.Empty;
            var scenario = await Task.Run(() => CompiledScenario.Load<CompiledScenario>(filePath, out scenarioLoadErrors));
            if (scenario == null)
            {
                MessageBox.Show($"Unable to load a scenario from `{filePath}`. The error returned was: \n {scenarioLoadErrors}", "ShanoEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLoading = false;

                Settings.Default.RemoveFromRecent(filePath);
                updateRecentsMenu();
                return;
            }

            await open(scenario);

            StatusLoading = false;
        }

        async Task open(CompiledScenario scenario)
        {
            //create the viewmodel
            Model = new ScenarioViewModel(scenario);
            await Model.Reload();

            //set the views' model
            scenarioTree.SetScenario(scenario.Config);
            ScenarioControl.ApplyModel(this, Model);

            //update UI
            Settings.Default.AddToRecent(scenario.Config.BaseDirectory);
            updateRecentsMenu();
            updateCaption();
        }


        async Task<bool> close()
        {
            if (Model == null || !Model.IsDirty)
                return true;

            var z = MessageBox.Show(
                "You have not saved your changes to the scenario. Would you like to do that now?",
                "ShanoEditor",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (z == DialogResult.Cancel)
                return false;

            if (z == DialogResult.Yes)
                await save();

            updateCaption();
            return true;
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
            if (Model != null)
            {
                var editPostfix = Model.IsDirty ? " *" : "";
                Text = "{0} - {1}{2}".F(Model.Scenario.Config.Name, WindowTitle, editPostfix);
            }
            else
            {
                Text = WindowTitle;
            }
        }
    }
}
