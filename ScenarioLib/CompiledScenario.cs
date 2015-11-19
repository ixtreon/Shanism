using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using Newtonsoft.Json;
using IO.Common;

namespace ScenarioLib
{
    /// <summary>
    /// A compiled scenario that contains information about the types of objects defined in it. 
    /// </summary>
    public class CompiledScenario : ScenarioFile
    {
        /// <summary>
        /// Gets the assembly that contains the compiled scenario. 
        /// </summary>
        public Assembly ScenarioAssembly { get; private set; }


        List<IUnit> units = new List<IUnit>();
        List<IDoodad> doodads = new List<IDoodad>();

        /// <summary>
        /// Gets the types of game objects defined in this scenario. 
        /// </summary>
        public IEnumerable<IDoodad> DefinedDoodads
        {
            get { return doodads; }
        }

        /// <summary>
        /// Gets an instance of each game object defined in this scenario. 
        /// </summary>
        public IEnumerable<IUnit> DefinedUnits
        {
            get { return units; }
        }


        [JsonConstructor]
        protected CompiledScenario() { }

        public CompiledScenario(string scenarioPath)
            : base(scenarioPath)
        {
        }

        public static new CompiledScenario Load(string scenarioPath)
        {
            return Load<CompiledScenario>(scenarioPath);
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

            var unitTypes = scAssembly.GetTypesDescending<IUnit>()
                .Where(canMakeUnit)
                .ToList();

            units = unitTypes
                .Select(ty => (IUnit)Activator.CreateInstance(ty, null, Vector.Zero))
                .ToList();


            var doodadTypes = scAssembly.GetTypesDescending<IDoodad>()
                .Where(canMakeDoodad)
                .ToList();

            doodads = doodadTypes
                .Select(ty => (IDoodad)Activator.CreateInstance(ty, Vector.Zero))
                .ToList();

        }

        //must have new T(Player, Vector)
        bool canMakeUnit(Type ty)
        {
            return ty.GetConstructors()
                .Select(c => c.GetParameters())
                .Any(ps => ps.Length == 2
                    && typeof(Vector).IsAssignableFrom(ps[1].ParameterType)
                    && typeof(IPlayer).IsAssignableFrom(ps[0].ParameterType));
        }

        //must have new T(Vector)
        bool canMakeDoodad(Type ty)
        {
            return ty.GetConstructors()
                .Select(c => c.GetParameters())
                .Any(ps => ps.Length == 1
                    && typeof(Vector).IsAssignableFrom(ps[0].ParameterType));
        }
    }
}
