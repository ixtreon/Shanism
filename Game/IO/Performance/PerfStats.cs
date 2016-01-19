using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Performance
{
    public class PerfStats
    {
        public static readonly PerfStats Default = new PerfStats();

        readonly Dictionary<string, long> stats = new Dictionary<string, long>();


        public void Reset()
        {
            stats.Clear();
        }

        public void Log(string category, long msTaken)
        {

        }

        public IEnumerable<KeyValuePair<string, long>> Stats
        {
            get { return stats; }
        }
    }
}
