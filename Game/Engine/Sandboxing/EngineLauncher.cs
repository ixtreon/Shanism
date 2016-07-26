using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine
{
    /// <summary>
    /// Creates an instance of a ShanoEngine that runs in a restricted appdomain. 
    /// </summary>
    class EngineLauncher
    {

        public void loadAssembly()
        {
            var assName = typeof(EngineLauncher).Assembly.GetName();

            var sand = new Sandboxer();

        }
    }


}
