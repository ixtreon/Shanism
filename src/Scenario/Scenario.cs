using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Shanism.Common.Entities;
using Shanism.Common.Scenario;

namespace Shanism.ScenarioLib
{

    public delegate Assembly AssemblyLoaderDelegate(byte[] byteStream, byte[] symbolStream);


    /// <summary>
    /// A compiled scenario that contains information about the types of objects defined in it. 
    /// </summary>
    public class Scenario
    {

        /// <summary>
        /// Gets the assembly that contains the compiled scenario. 
        /// </summary>
        readonly Assembly assembly;

        public ScenarioConfig Config { get; }


        public IEnumerable<Type> DefinedEntityTypes => CreateSpecimens<IEntity>().Select(e => e.GetType());

        [JsonConstructor]
        public Scenario() { }

        ///// <summary>
        ///// Creates a new empty scenario at the given path. 
        ///// </summary>
        ///// <param name="scenarioPath"></param>
        //public Scenario(string scenarioPath)
        //{
        //    Config = new ScenarioConfig(scenarioPath);
        //}


        internal Scenario(ScenarioConfig config, Assembly assembly)
        {
            this.assembly = assembly;

            Config = config;
        }

        static bool hasParameterlessCtor(Type ty) => ty
            .GetConstructors()
            .Any(c => c.GetParameters().Length == 0);

        /// <summary>
        /// Creates a single instance of each type descending from the provided type.
        /// </summary>
        public List<T> CreateSpecimens<T>()
            => assembly
                .GetTypesDescending<T>()
                .Where(hasParameterlessCtor)
                .Select(ty => (T)Activator.CreateInstance(ty))
                .ToList();
    }
}
