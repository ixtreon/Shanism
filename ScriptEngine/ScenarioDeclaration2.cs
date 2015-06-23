using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptEngine
{
    /// <summary>
    /// Represents a scenario as seen by the editor or the game engine. 
    /// </summary>
    class ScenarioDeclaration2
    {
        public readonly string Path;
        public readonly ScenarioBase Base;

        public ScenarioDeclaration2(string path)
        {
            this.Path = path;

        }

    }
}
