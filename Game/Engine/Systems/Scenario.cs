using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;
using ScriptLib;
using System.IO;
using Engine.Objects.Game;

namespace Engine.Systems
{
    public class Scenario
    {

        readonly private ScenarioCompiler compiler = new ScenarioCompiler();

        /// <summary>
        /// Gets the base directory of the scenario. 
        /// </summary>
        public readonly string Directory;

        readonly internal ModelManager Models = new ModelManager();

        readonly private List<CustomScript> customScripts = new List<CustomScript>();

        internal IEnumerable<CustomScript> CustomScripts
        {
            get { return customScripts; }
        }

        /// <summary>
        /// An engine can create a scenario from a given directory. 
        /// </summary>
        /// <param name="dir"></param>
        internal Scenario(string dir)
        {
            Directory = dir;
            compiler.ScenarioDir = Directory;
        }

        public bool TryCompile()
        {

            var result = compiler.Compile();

            if (result != null)
            {
                Console.WriteLine(result);
                throw new Exception();
            }

            loadTypes();
            RunScripts(s => s.LoadModels(Models));
            return true;
        }


        /// <summary>
        /// Loads the generated Ability.dll assembly
        /// </summary>
        private void loadTypes()
        {
            var assembly = compiler.Load();


            loadScripts(assembly);
            //createPrototypes<Ability>(assembly);
            //createPrototypes<Unit>(assembly);
            //createPrototypes<Buff>(assembly);
            //createPrototypes<CustomScript>(assembly);
        }

        private void loadScripts(Assembly assembly)
        {
            var t = typeof(CustomScript);
            var declaredTypes = assembly.GetTypes()
                .Where(ty => t.IsAssignableFrom(ty));

            foreach (var ty in declaredTypes)
            {
                var script = Activator.CreateInstance(ty) as CustomScript;
                customScripts.Add(script);
            }
        }

        internal void RunScripts(Action<CustomScript> act)
        {
            foreach (var s in customScripts)
                act(s);
        }
    }
}
