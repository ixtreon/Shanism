using IO.Objects;
using ScenarioLib;
using ShanoEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.Views
{
    class ScenarioControl : UserControl, IScenarioControl
    {
        public virtual ScenarioViewType ViewType { get; }

        public event Action ScenarioChanged;

        /// <summary>
        /// Gets or sets whether changes to the model are ignored. 
        /// Gets useful when we start loading the scenario. 
        /// </summary>
        protected bool Loading;

        public ScenarioViewModel Model { get; private set; }
        

        public void SetModel(ScenarioViewModel model)
        {
            if (Model != model)
            {
                Model = model;

                Task.Run(async () =>
                {
                    Loading = true;
                    await LoadModel();
                    Loading = false;
                });
            }
        }

        public async void Save()
        {
            await SaveModel();
        }

        /// <summary>
        /// Override to implement custom loading logic. 
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadModel() { }

        /// <summary>
        /// Made for controls to update the ViewModel 
        /// in case they are being lazy about it. 
        /// </summary>
        protected virtual async Task SaveModel() { }


        public void MarkAsChanged()
        {
            if (!Loading)
            {
                Model.IsDirty = true;
                ScenarioChanged?.Invoke();
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is IScenarioControl && Model != null)
            {
                var scView = (IScenarioControl)e.Control;
                scView.SetModel(Model);
            }
        }
    }
}
