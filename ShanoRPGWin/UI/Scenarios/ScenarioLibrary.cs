using ShanoRPGWin.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRPGWin.UI.Scenarios
{
    public class ScenarioLibrary : Component
    {
        private HashSet<string> loadedScenarios = new HashSet<string>();

        public bool AutoSave { get; set; }

        public event Action<string> ItemAdded;

        public event Action<string> ItemRemoved;

        public ScenarioLibrary()
        {
            Refresh();
        }

        public void Refresh()
        {
            loadedScenarios = new HashSet<string>(Settings.Default.ScenarioLibrary.Split('\t'));
        }

        public void Save()
        {
            Settings.Default.ScenarioLibrary = loadedScenarios.Aggregate((a, b) => a + '\t' + b);
            Settings.Default.Save();
        }

        /// <summary>
        /// Removes the given path from the library. Returns whether the operation was successful. 
        /// </summary>
        public bool Remove(string path)
        {
            var removed = loadedScenarios.Remove(path);
            if (AutoSave && removed) Save();
            ItemRemoved?.Invoke(path);
            return removed;
        }

        /// <summary>
        /// Adds the given path to the library. Returns whether the operation was successful. 
        /// </summary>
        public bool Add(string path)
        {
            var added = loadedScenarios.Add(path);
            if (AutoSave && added) Save();
            ItemAdded?.Invoke(path);
            return added;
        }
    }
}
