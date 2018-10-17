using Shanism.Common;
using Shanism.Engine.Models.Systems;
using Shanism.Engine.Systems.Scripts;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Shanism.Engine.Systems
{
    class ScriptingSystem : GameSystem, IScriptRunner
    {
        public override string Name => "Scripts";

        readonly SingleThreadedSynchronizationContext syncContext;
        readonly IReadOnlyList<CustomScript> allScripts;

        public SynchronizationContext Context => syncContext;

        public ScriptingSystem(Thread runner, Scenario scenario)
        {
            syncContext = new SingleThreadedSynchronizationContext(runner);
            allScripts = scenario.CreateSpecimens<CustomScript>();
        }

        /// <summary>
        /// Runs the specified action on all loaded scripts.
        /// </summary>
        /// <param name="act">The action to execute on each script.</param>
        public void Run(Action<CustomScript> act)
        {
            foreach (var sc in allScripts)
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
