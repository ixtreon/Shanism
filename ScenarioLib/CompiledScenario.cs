using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using Newtonsoft.Json;

namespace ScenarioLib
{
    /// <summary>
    /// A compiled scenario that contains information about the types of objects defined in it
    /// as well as being a <see cref="ScenarioFile"/> object. 
    /// </summary>
    public class CompiledScenario : ScenarioFile
    {
        /// <summary>
        /// Gets the assembly that contains the compiled scenario. 
        /// </summary>
        public Assembly ScenarioAssembly { get; private set; }


        List<Type> objectTypes = new List<Type>();
        List<IGameObject> objects = new List<IGameObject>();

        /// <summary>
        /// Gets the types of game objects defined in this scenario. 
        /// </summary>
        public IEnumerable<Type> DefinedObjectTypes
        {
            get { return objectTypes; }
        }

        /// <summary>
        /// Gets an instance of each game object defined in this scenario. 
        /// </summary>
        public IEnumerable<IGameObject> DefinedObjects
        {
            get { return objects; }
        }

        /// <summary>
        /// Gets all units defined in this scenario. 
        /// </summary>
        public IEnumerable<IUnit> DefinedUnits
        {
            get { return objects.OfType<IUnit>(); }
        }

        /// <summary>
        /// Gets all doodads defined in this scenario. 
        /// </summary>
        public IEnumerable<IDoodad> DefinedDoodads
        {
            get { return objects.OfType<IDoodad>(); }
        }


        protected CompiledScenario() { }

        public CompiledScenario(string scenarioPath)
            : base(scenarioPath)
        {
        }


        public static new T Load<T>(string scenarioPath)
            where T : CompiledScenario
        {
            //call base
            var sc = ScenarioFile.Load<T>(scenarioPath);

            //compile
            var cmp = new ScenarioCompiler(scenarioPath);
            var errors = cmp.Compile();
            if (errors.Any())
                throw new AggregateException(errors.Select(err => new CompilerException(err)));
            
            //load assembly to memory
            cmp.LoadCompiledAssembly();

            //load objects from assembly
            sc.loadAssembly(cmp.Assembly);

            return sc;
        }

        void loadAssembly(Assembly scAssembly)
        {
            ScenarioAssembly = scAssembly;

            objectTypes = scAssembly.GetTypesDescending<IGameObject>()
                .Where(ty => ty.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            objects = objectTypes
                .Select(ty => (IGameObject)Activator.CreateInstance(ty))
                .ToList();
        }
    }
}
