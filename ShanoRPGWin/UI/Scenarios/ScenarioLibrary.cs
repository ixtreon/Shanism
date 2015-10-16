using ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRPGWin.UI.Scenarios
{
    class ScenarioLibrary
    {
        private List<ScenarioBase> _scenarios;

        public string DirectoryPath { get; private set; }

        public IEnumerable<ScenarioBase> Scenarios
        {
            get { return _scenarios; }
        }

        public ScenarioLibrary(string directory)
        {
            DirectoryPath = directory;
            Refresh();
        }

        public void Refresh()
        {
            //get the sub-directories
            var dirs = Directory.EnumerateDirectories(DirectoryPath);
            _scenarios = dirs
                .Select(d => ScenarioBase.Load(d))
                .Where(sc => sc != null)
                .ToList();

        }
    }
}
