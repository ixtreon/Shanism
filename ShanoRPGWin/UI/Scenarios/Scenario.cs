using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRPGWin.UI.Scenarios
{
    /// <summary>
    /// Represents a scenario loaded by the GUI. 
    /// </summary>
    public class Scenario
    {
        /// <summary>
        /// Checks whether there is a scenario at the given path. 
        /// Should really be somewhere else. 
        /// </summary>
        public static bool ExistsAt(string path)
        {
            return Directory.Exists(path) && File.Exists(Path.Combine(path, "Scenario.cs"));
        }

        public string BaseDirectory { get; private set; }

        public Scenario(string path)
        {
            if (!ExistsAt(path))
                throw new ArgumentException("No scenario lives there!", nameof(path));

            BaseDirectory = path;
        }
    }
}
