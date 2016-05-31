using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Objects;
using Newtonsoft.Json;
using Shanism.Common.Game;
using System.IO;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// A compiled scenario that contains information about the types of objects defined in it. 
    /// </summary>
    public class Scenario
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
        public Assembly Assembly { get; private set; }

        public ScenarioConfig Config { get; private set; }

        public IEnumerable<IEntity> DefinedEntities { get; private set; }


        public IEnumerable<Type> DefinedEntityTypes => DefinedEntities?.Select(e => e.GetType()) ?? Enumerable.Empty<Type>();

        [JsonConstructor]
        public Scenario() { }

        /// <summary>
        /// Creates a new scenario at the given path. 
        /// </summary>
        /// <param name="scenarioPath"></param>
        public Scenario(string scenarioPath)
        {
            Config = new ScenarioConfig(scenarioPath);

            foreach (var folder in FolderNames)
            {
                var fullFolderName = Path.Combine(scenarioPath, folder);
                if (!Directory.Exists(fullFolderName))
                    Directory.CreateDirectory(fullFolderName);
            }
        }


        public static Scenario Load(string scenarioPath, out string errors)
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
                errors = "Unable to compile the scenario:" +
                    "\n" + string.Join("\n", compileErrors.Select(e => $"{e.Location} {e.GetMessage()}"));
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

            var sc = new Scenario
            {
                Config = config,
                Assembly = cmp.Assembly,
                DefinedEntities = definedObjs,
            };

            errors = string.Empty;
            return sc;
        }

        void loadAssembly(Assembly scAssembly)
        {
            Assembly = scAssembly;
        }
        static bool hasParameterlessCtor(Type ty)
        {
            return ty.GetConstructors()
                .Any(c => c.GetParameters().Length == 0);
        }
    }
}
