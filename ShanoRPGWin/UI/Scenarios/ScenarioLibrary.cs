using ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRPGWin.UI.Scenarios
{
    public class ScenarioLibrary
    {
        private List<ScenarioBase> scenarios;

        private Dictionary<string, ScenarioBase> scenarioLookupTable = new Dictionary<string, ScenarioBase>();

        /// <summary>
        /// The path to this scenario library's directory. 
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// Gets all scenarios found in the given directory. 
        /// </summary>
        public IEnumerable<ScenarioBase> Scenarios
        {
            get { return scenarios ?? Enumerable.Empty<ScenarioBase>(); }
        }
        
        /// <summary>
        /// Creates a new library in the given directory. 
        /// Automatically calls <see cref="Refresh"/> to discover scenarios inside the directory. 
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
                var dirs = Directory.EnumerateDirectories(DirectoryPath);
                scenarios = dirs
                    .Select(d => ScenarioBase.Load(d))
                    .Where(sc => sc != null)
                    .ToList();
                scenarioLookupTable = scenarios.ToDictionary(sc => sc.ScenarioDirectory, sc => sc);
            });
        }

        public bool TryGet(string scenarioPath, out ScenarioBase sc)
        {
            return scenarioLookupTable.TryGetValue(scenarioPath, out sc);
        }
    }
}
