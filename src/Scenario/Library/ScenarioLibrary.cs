using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// An index of all scenarios in a given directory.
    /// </summary>
    public class ScenarioLibrary
    {
        readonly ScenarioConfigReader reader = new ScenarioConfigReader();

        readonly List<ScenarioConfig> scenarios = new List<ScenarioConfig>();

        Dictionary<string, ScenarioConfig> scenarioLookupTable = new Dictionary<string, ScenarioConfig>();

        /// <summary>
        /// The path to this scenario library's directory. 
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// Gets all scenarios found in the given directory. 
        /// </summary>
        public IReadOnlyList<ScenarioConfig> Scenarios => scenarios;

        public bool DirectoryExists { get; private set; }

        /// <summary>
        /// Creates a new library in the given directory. 
        /// </summary>
        /// <param name="directory">The base directory of the library. </param>
        public ScenarioLibrary(string directory)
        {
            DirectoryPath = directory;
        }

        /// <summary>
        /// Re-discovers the scenarios in the base directory. 
        /// </summary>
        public async Task Refresh()
        {
            //get the sub-directories
            await Task.Run(() =>
            {
                DirectoryExists = Directory.Exists(DirectoryPath);
                if (!DirectoryExists)
                    return;

                scenarios.Clear();
                var dirs = Directory.EnumerateDirectories(DirectoryPath);
                foreach(var dir in dirs)
                {
                    var result = reader.TryReadFromDisk(dir);
                    if(result.IsSuccessful)
                        scenarios.Add(result.Value);

                    var convs = Newtonsoft.Json.JsonConvert.DefaultSettings().ContractResolver;
                }

                scenarioLookupTable = scenarios
                    .ToDictionary(sc => sc.BaseDirectory, sc => sc);

                Console.WriteLine($"Found {scenarios.Count} maps in {Path.GetFullPath(DirectoryPath)}");
            });
        }

        public bool TryGet(string scenarioPath, out ScenarioConfig sc)
            => scenarioLookupTable.TryGetValue(scenarioPath, out sc);
    }
}
