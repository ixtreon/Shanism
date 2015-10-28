using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;
using System.IO;
using Engine.Objects.Game;
using IO;
using Engine.Systems;
using ScenarioLib;

namespace Engine
{
    public class Scenario : CompiledScenario<CustomScript>
    {
        /// <summary>
        /// Used to list all models used by the scenario. 
        /// </summary>
        [Obsolete]
        readonly internal ModelManager Models = new ModelManager();

        /// <summary>
        /// Custom scripts (see <see cref="CustomScript"/>) get loaded here. 
        /// </summary>
        readonly private List<CustomScript> customScripts = new List<CustomScript>();




        public Scenario(string path)
            : base(path)
        {

        }

        /// <summary>
        /// Runs an action on all loaded scripts. 
        /// </summary>
        /// <param name="act"></param>
        internal void RunScripts(Action<CustomScript> act)
        {
            foreach (var s in Scripts)
                act(s);
        }

    }
}
