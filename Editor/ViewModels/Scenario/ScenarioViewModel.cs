using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Editor.ViewModels
{
    class ScenarioViewModel : IViewModel
    {
        public Scenario Scenario { get; }

        public ContentViewModel Content { get; } = new ContentViewModel();

        /// <summary>
        /// A custom flag indicating whether the scenario has changed. 
        /// </summary>
        public bool IsDirty { get; set; }

        public ScenarioViewModel(Scenario sc)
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
