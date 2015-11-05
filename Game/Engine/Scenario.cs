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
    public class Scenario : CompiledScenario
    {
        /// <summary>
        /// Used to list all models used by the scenario. 
        /// </summary>
        [Obsolete]
        readonly internal ModelManager Models = new ModelManager();


        readonly List<CustomScript> scripts = new List<CustomScript>();

        /// <summary>
        /// Gets a list of all scripts (see <see cref="CustomScript"/>) in the scenario. 
        /// </summary>
        public IEnumerable<CustomScript> Scripts
        {
            get { return scripts; }
        }


        protected Scenario() { }
        

        public static new Scenario Load(string path)
        {
            return Load<Scenario>(path);
        }


        public static new T Load<T>(string path)
            where T : Scenario
        {
            var sc = CompiledScenario.Load<T>(path);

            sc.addScripts();

            return sc;
        }

        void addScripts()
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
    }
}
