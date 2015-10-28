using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;

namespace ScenarioLib
{
    /// <summary>
    /// A compiled scenario that contains information about the types of objects defined in it
    /// as well as being a <see cref="ScenarioFile"/> object. 
    /// </summary>
    public class CompiledScenario<TScript> : ScenarioFile
        where TScript : IScript
    {
        /// <summary>
        /// Gets the assembly that contains the compiled scenario. 
        /// </summary>
        public Assembly ScenarioAssembly { get; private set; }


        List<Type> objectTypes = new List<Type>();
        List<IGameObject> objects = new List<IGameObject>();
        List<TScript> scripts = new List<TScript>();

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

        public IEnumerable<TScript> Scripts
        {
            get { return scripts; }
        }

        public CompiledScenario(string scenarioPath)
            : base(scenarioPath)
        {
            //compile
            var cmp = new ScenarioCompiler(scenarioPath);
            var errors = cmp.Compile();
            if (errors.Any())
                throw new AggregateException(errors.Select(err => new CompilerException(err)));
            //load
            cmp.LoadCompiledAssembly();
            //parse
            loadAssembly(cmp.Assembly);
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

            scripts = scAssembly.GetTypesDescending<TScript>()
                .Select(ty => (TScript)Activator.CreateInstance(ty))
                .ToList();
        }
    }


    public interface IScript
    {
    }
}
