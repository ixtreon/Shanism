using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;
using System.IO;
using Engine.Objects.Entities;
using IO;
using Engine.Systems;
using ScenarioLib;
using Engine.Maps;
using Engine.Common;

namespace Engine
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
                .ToList();

            foreach (var e in entities)
                map.Add(e);
        }
    }
}
