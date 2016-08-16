using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Shanism.Common.Interfaces.Entities;

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

        public IEnumerable<IEntity> DefinedEntities { get; private set; } = Enumerable.Empty<IEntity>();


        public IEnumerable<Type> DefinedEntityTypes => DefinedEntities?.Select(e => e.GetType()) ?? Enumerable.Empty<Type>();

        [JsonConstructor]
        public Scenario() { }

        /// <summary>
        /// Creates a new empty scenario at the given path. 
        /// </summary>
        /// <param name="scenarioPath"></param>
        public Scenario(string scenarioPath,
            bool createDefaultFolders = false)
        {
            Config = new ScenarioConfig(scenarioPath);

            if(createDefaultFolders)
                foreach (var folder in FolderNames)
                {
                    var fullFolderName = Path.Combine(scenarioPath, folder);
                    if (!Directory.Exists(fullFolderName))
                        Directory.CreateDirectory(fullFolderName);
                }
        }

        /// <summary>
        /// The collection of outcomes from a scenario compilation.
        /// See <see cref="Scenario.Load(string, out string, out Scenario)"/>.
        /// </summary>
        public enum ScenarioCompilationResult
        {
            InvalidConfig,
            CompileErrors,
            InvalidAssembly,

            Success,
        }

        public static ScenarioCompilationResult Load(string scenarioPath, 
            out string errors, out Scenario sc)
        {
            sc = null;

            //load the config
            string configLoadErrors;
            var config = ScenarioConfig.LoadFromDisk(scenarioPath, out configLoadErrors);
            if (config == null)
            {
                errors = configLoadErrors;
                return ScenarioCompilationResult.InvalidConfig;
            }

            //compile..
            var cmp = new ScenarioCompiler(scenarioPath);
            var compileErrors = cmp.Compile();
            if (compileErrors.Any())
            {
                errors = string.Join("\n", compileErrors.Select(e => $"{e.Location} {e.GetMessage()}"));
                return ScenarioCompilationResult.CompileErrors;
            }

            //..and load the assembly
            string assemblyLoadErrors;
            if (!cmp.LoadCompiledAssembly(out assemblyLoadErrors))
            {
                errors = "Unable to load the compiled scenario:"
                    + "\n" + assemblyLoadErrors;
                return ScenarioCompilationResult.InvalidAssembly;
            }

            var definedObjs = cmp.Assembly
                    .GetTypesDescending<IEntity>()
                    .Where(hasParameterlessCtor)
                    .Select(t => (IEntity)Activator.CreateInstance(t))
                    .ToList()
                    ?? Enumerable.Empty<IEntity>();

            errors = string.Empty;
            sc = new Scenario
            {
                Config = config,
                Assembly = cmp.Assembly,
                DefinedEntities = definedObjs,
            };
            return ScenarioCompilationResult.Success;
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
