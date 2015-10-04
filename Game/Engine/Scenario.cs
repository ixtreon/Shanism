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

namespace Engine
{
    public abstract class Scenario
    {
        /// <summary>
        /// Used to list all models used by the scenario. 
        /// </summary>
        readonly internal ModelManager Models = new ModelManager();

        /// <summary>
        /// Custom scripts (see <see cref="CustomScript"/>) get loaded here. 
        /// </summary>
        readonly private List<CustomScript> customScripts = new List<CustomScript>();


        protected readonly List<string> files = new List<string>();

        /// <summary>
        /// Gets the base directory of the scenario. 
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// Gets the name of the scenario. 
        /// </summary>
        public string Name { get; protected set; } = "ShanoScenario";

        /// <summary>
        /// Gets the description of the scenario. 
        /// </summary>
        public string Description { get; protected set; } = "Shano description of a shano scenario.";

        public string MessageOfTheDay { get; protected set; } = "Welcome to the shano world!";

        /// <summary>
        /// Gets all files in the scenario. 
        /// </summary>
        //public IEnumerable<string> Files
        //{
        //    get { return files; }
        //}

        /// <summary>
        /// A convenient place to list all files used in the scenario. 
        /// </summary>
        public abstract void ListFiles();


        /// <summary>
        /// Runs an action on all loaded scripts. 
        /// </summary>
        /// <param name="act"></param>
        internal void RunScripts(Action<CustomScript> act)
        {
            foreach (var s in customScripts)
                act(s);
        }


        /// <summary>
        /// Loads the generated assembly. 
        /// </summary>
        internal void LoadTypes(Assembly currentAssembly)
        {
            loadScripts(currentAssembly);
        }

        /// <summary>
        /// Loads all objects of type <see cref="CustomScript"/> from the given assembly into <see cref="customScripts"/>. 
        /// </summary>
        /// <param name="assembly"></param>
        void loadScripts(Assembly assembly)
        {
            customScripts.AddRange(assembly.CreateInstanceOfEach<CustomScript>());
        }

    }
}
