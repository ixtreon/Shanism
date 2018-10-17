using Shanism.ScenarioLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shanism.Client.Scenarios
{
    /// <summary>
    /// A list of multiple scenario libraries with all of their scenarios.
    /// </summary>
    public class ScenarioList
    {
        public IReadOnlyList<ScenarioLibrary> Libraries { get; }

        public bool HasLoaded { get; private set; }

        public IEnumerable<ScenarioConfig> Scenarios
            => Libraries.SelectMany(lib => lib.Scenarios);


        public ScenarioList(string listFile)
            : this(ParseShit(listFile)) { }

        static IEnumerable<string> ParseShit(string listFile)
        {
            if (!File.Exists(listFile))
                throw new FileNotFoundException($"Could not find the `{listFile}` file.");

            return File.ReadAllLines(listFile)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l) && l[0] != '#');
        }

        public ScenarioList(IEnumerable<string> directories)
        {
            Libraries = directories
                .Select(l => new ScenarioLibrary(l))
                .ToList();

            Task.Run(Reload);
        }

        public static ScenarioList FromFile(string listFile)
        {
            var libraries = File.ReadAllLines(listFile)
                .Select(l => l.Trim())
                .Where(l => l?.Length > 0 && l[0] != '#');

            return new ScenarioList(libraries);
        }


        public async Task Reload()
        {
            HasLoaded = false;

            await Task.WhenAll(Libraries.Select(lib => lib.Refresh()));

            HasLoaded = true;
        }
    }
}
