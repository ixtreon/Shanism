using IO.Objects;
using ScenarioLib;
using ShanoEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.ScenarioViews
{
    class ScenarioView : UserControl
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

        public void Save()
        {
            SaveModel();
        }

        protected virtual async Task LoadModel() { }

        /// <summary>
        /// Made for controls to update the ViewModel 
        /// in case they are being lazy about it. 
        /// </summary>
        protected virtual void SaveModel() { }


        public void MarkAsChanged()
        {
            if (!Loading)
            {
                Model.IsDirty = true;
                ScenarioChanged?.Invoke();
            }
        }

    }
}
