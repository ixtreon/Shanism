using Shanism.Common;
using Shanism.Engine.Systems;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Engine.Scripting
{
    class ScriptingSystem : GameSystem, IScriptRunner
    {
        public override string SystemName => "Scripts";

        readonly SingleThreadedSynchronizationContext syncContext;

        readonly List<CustomScript> loadedScripts = new List<CustomScript>();

        public SynchronizationContext Context => syncContext;

        public ScriptingSystem(Thread runner)
        {
            syncContext = new SingleThreadedSynchronizationContext(runner);

        }


        /// <summary>
        /// Clears all loaded scripts and then loads all scripts 
        /// from the given scenario. 
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public void LoadScenario(Scenario scenario)
        {
            loadedScripts.Clear();
            loadedScripts.AddRange(scenario.Assembly
                .GetTypesDescending<CustomScript>()
                .Select(ty => (CustomScript)Activator.CreateInstance(ty)));
        }

        /// <summary>
        /// Runs the specified action on all loaded scripts.
        /// </summary>
        /// <param name="act">The action to execute on each script.</param>
        public void Run(Action<CustomScript> act)
        {
            foreach (var sc in loadedScripts)
                act(sc);
        }

        internal override void Update(int msElapsed)
        {
            SynchronizationContext.SetSynchronizationContext(syncContext);
            syncContext.ExecutePendingWorkItems();
        }

        public void Enqueue(Action act)
        {
            syncContext.Post(act);
        }
    }
}
