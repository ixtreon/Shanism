using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects;
using Shanism.Common.Game;
using System.IO;
using Shanism.Engine.Objects.Entities;
using Shanism.Common;
using Shanism.Engine.Systems;
using Shanism.ScenarioLib;
using Shanism.Engine.Maps;
using Shanism.Engine.Common;

namespace Shanism.Engine
{
    public class Scenario : CompiledScenario
    {

        readonly List<CustomScript> scripts = new List<CustomScript>();

        /// <summary>
        /// Gets a list of all scripts (see <see cref="CustomScript"/>) in the scenario. 
        /// </summary>
        public IEnumerable<CustomScript> Scripts => scripts;


        public Scenario() { }


        public static new Scenario Load<T>(string path, out string errors)
            where T : Scenario, new()
        {
            var sc = CompiledScenario.Load<T>(path, out errors);

            sc?.reloadScripts();

            return sc;
        }


        void reloadScripts()
        {
            scripts.Clear();
            scripts.AddRange(ScenarioAssembly
                .GetTypesDescending<CustomScript>()
                .Select(ty => (CustomScript)Activator.CreateInstance(ty)));
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


        internal void CreateStartupObjects(MapSystem map)
        {
            var oc = new ObjectCreator(this);
            var entities = Config.Map.Objects
                .Select(oc.CreateObject)
                .Where(o => o != null)
                .ToList();

            foreach (var e in entities)
                map.Add(e);
        }
    }
}
