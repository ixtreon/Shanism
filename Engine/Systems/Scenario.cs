using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;
using ScriptLib;

namespace Engine.Systems
{
    class Scenario : ScenarioCompiler
    {
        const int SCENARIO_ITEMS = 6;

        readonly Dictionary<Type, Dictionary<string, Type>> prototypes = new Dictionary<Type, Dictionary<string, Type>>();


        readonly internal ModelManager Models = new ModelManager();

        private readonly List<CustomScript> customScripts = new List<CustomScript>();
        internal IEnumerable<CustomScript> CustomScripts
        {
            get { return customScripts; }
        }

        /// <summary>
        /// An engine can create a scenario from some directory. 
        /// </summary>
        /// <param name="fileDir"></param>
        internal Scenario(string fileDir)
            : base(fileDir)
        {

        }

        public bool TryCompile()
        {
            var result = this.Compile();

            if (result != null)
            {
                Console.WriteLine(result);
                throw new Exception();
            }

            loadTypes();
            RunScripts(s => s.LoadModels(Models));
            return true;
        }

        public bool IsStarted { get; private set; }
        public void StartScenario()
        {
            if (IsStarted)
                throw new Exception("Scenario is already running!");
            IsStarted = true;

        }

        /// <summary>
        /// Loads the generated Ability.dll assembly
        /// </summary>
        private void loadTypes()
        {
            var assembly = Assembly.LoadFile(getLocalDir(OutputFile));

            createPrototypes<Ability>(assembly);
            createPrototypes<Unit>(assembly);
            createPrototypes<Buff>(assembly);

            loadScripts(assembly);
            createPrototypes<CustomScript>(assembly);
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


        private T createThing<T>(string name)
            where T : ScenarioObject
        {
            var proto = prototypes[typeof(T)][name];
            var obj = Activator.CreateInstance(proto) as T;
            return obj;
        }

        private void createPrototypes<T>(Assembly a)
            where T : ScenarioObject
        {
            var t = typeof(T);
            var declaredTypes = a.GetTypes()
                .Where(ty => t.IsAssignableFrom(ty));

            if (!prototypes.ContainsKey(t))
                prototypes.Add(t, new Dictionary<string, Type>());

            var protoDict = prototypes[t];
            foreach (var ty in declaredTypes)
                protoDict.Add(ty.ToString(), ty);
        }
    }
}
