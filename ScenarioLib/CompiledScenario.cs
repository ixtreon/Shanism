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
using System.IO;

namespace ScenarioLib
{
    /// <summary>
    /// A compiled scenario that contains information about the types of objects defined in it. 
    /// </summary>
    public class CompiledScenario
    {
        static readonly string[] FolderNames =
        {
            "Abilities",
            "Buffs",
            "Doodads",
            "Effects",
            "Items",
            "Scripts",
            "Units",
        };

        /// <summary>
        /// Gets the assembly that contains the compiled scenario. 
        /// </summary>
        public Assembly ScenarioAssembly { get; internal set; }

        public ScenarioConfig Config { get; private set; }
        public IEnumerable<IEntity> DefinedEntities { get; private set; }


        public IEnumerable<Type> DefinedEntityTypes => DefinedEntities.Select(e => e.GetType());

        [JsonConstructor]
        public CompiledScenario() { }

        /// <summary>
        /// Creates a new scenario at the given path. 
        /// </summary>
        /// <param name="scenarioPath"></param>
        public CompiledScenario(string scenarioPath)
        {
            Config = new ScenarioConfig(scenarioPath);

            foreach (var folder in FolderNames)
            {
                var fullFolderName = Path.Combine(scenarioPath, folder);
                if (!Directory.Exists(fullFolderName))
                    Directory.CreateDirectory(fullFolderName);
            }
        }


        public static T Load<T>(string scenarioPath, out string errors)
            where T : CompiledScenario, new()
        {
            //load the config
            string configLoadErrors;
            var config = ScenarioConfig.Load(scenarioPath, out configLoadErrors);
            if (config == null)
            {
                errors = "Invalid config file:"
                    + "\n" + configLoadErrors;
                return null;
            }

            //compile..
            var cmp = new ScenarioCompiler(scenarioPath);
            var compileErrors = cmp.Compile();
            if (compileErrors.Any())
            {
                errors = "Unable to comiple the scenario:" +
                    "\n" + string.Join("\n", compileErrors.Select(e => e.GetMessage()));
                return null;
            }

            //..and load the assembly
            string assemblyLoadErrors;
            if (!cmp.LoadCompiledAssembly(out assemblyLoadErrors))
            {
                errors = "Unable to load the compiled scenario:"
                    + "\n" + assemblyLoadErrors;
                return null;
            }

            var definedObjs = cmp.Assembly
                    .GetTypesDescending<IEntity>()
                    .Where(hasParameterlessCtor)
                    .Select(t => (IEntity)Activator.CreateInstance(t))
                    .ToList()
                    ?? Enumerable.Empty<IEntity>();

            var sc = new T
            {
                Config = config,
                ScenarioAssembly = cmp.Assembly,
                DefinedEntities = definedObjs,
            };

            errors = string.Empty;
            return sc;
        }

        void loadAssembly(Assembly scAssembly)
        {
            ScenarioAssembly = scAssembly;
        }
        static bool hasParameterlessCtor(Type ty)
        {
            return ty.GetConstructors()
                .Any(c => c.GetParameters().Length == 0);
        }
    }
}
