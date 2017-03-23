using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Scenarios
{
    /// <summary>
    /// Lists the scenarios from multiple libraries (directories).
    /// </summary>
    public class ScenarioList
    {
        public IReadOnlyList<ScenarioLibrary> Libraries { get; private set; }

        public IEnumerable<ScenarioConfig> Scenarios
            => Libraries.SelectMany(lib => lib.Scenarios);


        public async void LoadList(string listFile)
        {
            if (!File.Exists(listFile))
            {
                Libraries = Array.Empty<ScenarioLibrary>();
                return;
            }

            Libraries = File.ReadAllLines(listFile)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l) && l[0] != '#')
                .Select(l => new ScenarioLibrary(l))
                .ToList();

            Reload();
        }

        public bool HasLoaded { get; private set; }

        public async void Reload()
        {
            HasLoaded = false;

            await Task.WhenAll(Libraries.Select(lib =>
            {
                return lib.Refresh();
            }));

            HasLoaded = true;
        }
    }
}
