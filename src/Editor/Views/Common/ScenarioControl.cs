using Shanism.Common.StubObjects;
using Shanism.ScenarioLib;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.Views
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

        
        public static void ApplyModel(Form frm, ScenarioViewModel model)
        {
            var kidz = enumerateChildren(frm).ToList();

            foreach (var c in kidz.OfType<IScenarioControl>())
                c.SetModel(model);
        }

        public async void SetModel(ScenarioViewModel model)
        {
            if (Model != model)
            {
                Model = model;

                Loading = true;
                await LoadModel();
                Loading = false;
            }
        }
        
        static IEnumerable<Control> enumerateChildren(Control c)
        {
            var kidz = c.Controls.Cast<Control>();
            return kidz.Concat(kidz.SelectMany(enumerateChildren));
        }

        public void Save()
        {
            SaveModel();
        }

        /// <summary>
        /// Override to implement custom loading logic. 
        /// </summary>
        /// <returns></returns>
        protected virtual Task LoadModel() { return Task.CompletedTask; }

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
