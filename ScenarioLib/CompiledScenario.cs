using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IO;

namespace ScenarioLib
{
    /// <summary>
    /// A compiled scenario that contains information about the types of objects defined in it
    /// as well as providing the functionality of a <see cref="ScenarioBase"/> object. 
    /// </summary>
    public class CompiledScenario : ScenarioBase
    {
        /// <summary>
        /// Gets the assembly that contains the compiled scenario. 
        /// </summary>
        public Assembly ScenarioAssembly { get; private set; }



        List<IGameObject> objects = new List<IGameObject>();

        /// <summary>
        /// Gets all game objects defined in this scenario. 
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
        public IEnumerable<IUnit> DefinedDoodads
        {
            get { return objects.OfType<IUnit>(); }
        }

        public CompiledScenario(string scenarioPath)
            : base(scenarioPath)
        {
            //TODO: use the compiler, lol
        }

        public void LoadAssembly(Assembly scAssembly)
        {
            ScenarioAssembly = scAssembly;

            objects = scAssembly.GetTypesDescending<IGameObject>()
                .Where(ty => ty.GetConstructor(Type.EmptyTypes) != null)
                .Select(ty => (IGameObject)Activator.CreateInstance(ty))
                .ToList();
        }
    }
}
