using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network
{
    class Log
    {
        public static Ix.Logging.Log Default;


        public static void Init(string name = "network")
        {
           Default = new Ix.Logging.Log(name);
        }
    }
}
