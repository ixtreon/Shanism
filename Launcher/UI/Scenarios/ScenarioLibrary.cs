﻿using Shanism.ScenarioLib;
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
        List<ScenarioConfig> scenarios = new List<ScenarioConfig>();

        Dictionary<string, ScenarioConfig> scenarioLookupTable = new Dictionary<string, ScenarioConfig>();

        /// <summary>
        /// The path to this scenario library's directory. 
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// Gets all scenarios found in the given directory. 
        /// </summary>
        public IEnumerable<ScenarioConfig> Scenarios => scenarios;
        
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
                if (!Directory.Exists(DirectoryPath))
                    return;
                string errors;
                var dirs = Directory.EnumerateDirectories(DirectoryPath);
                scenarios = dirs
                    .Select(d => ScenarioConfig.Load(d, out errors))
                    .Where(sc => sc != null)
                    .ToList();
                scenarioLookupTable = scenarios
                    .ToDictionary(sc => sc.BaseDirectory, sc => sc);
            });
        }

        public bool TryGet(string scenarioPath, out ScenarioConfig sc)
        {
            return scenarioLookupTable.TryGetValue(scenarioPath, out sc);
        }
    }
}