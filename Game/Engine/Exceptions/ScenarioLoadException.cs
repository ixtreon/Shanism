using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Exceptions
{
    class ScenarioLoadException : FileLoadException
    {
        public ScenarioLoadException(string fileName, string errors)
            : base($"Unable to load the scenario at `{fileName}`:\n\n{errors}", fileName) { }

    }
}
