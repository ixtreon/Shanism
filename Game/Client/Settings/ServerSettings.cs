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

        /// <summary>
        /// Gets all servers ever played.
        /// </summary>
        public List<string> All = new List<string>();

        public void Add(string hostAddress)
        {
            All.Add(hostAddress);
        }

        public void SetLastPlayed(string hostAddress)
        {
            LastPlayed = hostAddress;
        }
    }
}
