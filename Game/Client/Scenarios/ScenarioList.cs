using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Scenarios
{
    class ScenarioList
    {
        public List<ScenarioLibrary> Libraries { get; } = new List<ScenarioLibrary>();

        public IEnumerable<ScenarioConfig> Scenarios
            => Libraries.SelectMany(lib => lib.Scenarios);


        public void LoadList(string listFile)
        {
            if (!File.Exists(listFile))
            {
                Libraries.Clear();
                return;
            }

            foreach (var line in File.ReadAllLines(listFile))
            {
                var dir = line.Trim();
                if (!string.IsNullOrEmpty(dir) && dir[0] != '#')
                {
                    var lib = new ScenarioLibrary(dir);
                    Libraries.Add(lib);
                }
            }

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
