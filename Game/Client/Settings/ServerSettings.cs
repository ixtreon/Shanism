using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    class ServerSettings
    {
        public string LastPlayed { get; set; }

        public SortedSet<string> All = new SortedSet<string>();

        public void Add(string serverName)
        {
            All.Add(serverName);
        }

        public void SetLast(string serverName)
        {
            LastPlayed = serverName;
        }
    }
}
