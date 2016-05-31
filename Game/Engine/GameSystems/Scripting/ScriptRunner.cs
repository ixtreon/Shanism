using Shanism.Common;
using Shanism.Engine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Engine.Scripting
{
    class ScriptRunner : GameSystem, IScriptRunner
    {
        readonly SingleThreadedSynchronizationContext syncContext;

        readonly List<CustomScript> loadedScripts = new List<CustomScript>();


        public ScriptRunner(Thread runner)
        {
            syncContext = new SingleThreadedSynchronizationContext(runner);
        }


        public void ReloadScripts(Assembly scenarioAssembly)
        {
            loadedScripts.Clear();
            loadedScripts.AddRange(scenarioAssembly
                .GetTypesDescending<CustomScript>()
                .Select(ty => (CustomScript)Activator.CreateInstance(ty)));
        }

        /// <summary>
        /// Runs the specified action on all loaded scripts.
        /// </summary>
        /// <param name="act">The action to execute on each script.</param>
        public void Run(Action<CustomScript> act)
        {
            foreach(var sc in loadedScripts)
                syncContext.Post(new SendOrPostCallback((_) => act(sc)), null);
        }

        internal override void Update(int msElapsed)
        {

            //run all of the 
            syncContext.ExecutePendingWorkItems();
        }
    }
}
