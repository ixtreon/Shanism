using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoEditor.ViewModels
{
    class ScenarioViewModel : IViewModel
    {
        public CompiledScenario Scenario { get; }

        public ContentViewModel Content { get; } = new ContentViewModel();

        /// <summary>
        /// A custom flag indicating whether the scenario has changed. 
        /// </summary>
        public bool IsDirty { get; set; }

        public ScenarioViewModel(CompiledScenario sc)
        {
            Scenario = sc;
        }

        public async Task Reload()
        {
            await Content.Load(Scenario);
        }



        /// <summary>
        /// Saves the view model to the scenario
        /// and the scenario to the disk. 
        /// </summary>
        public async Task Save()
        {
            Content.Save();
            Scenario.Config.Save();

            IsDirty = false;
        }
    }
}
