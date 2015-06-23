using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptEngine
{
    public abstract class ScenarioBase
    {
        static readonly string[] sourceTypes = new[] 
            {
                "Abilities",
                "Buffs",
                "Doodads",
                "Effects",
                "Items",
                "Scripts",
                "Units",
            };


        protected readonly List<string> files = new List<string>();


        public string Name { get; set; }

        public IEnumerable<string> Files
        {
            get { return files; }
        }

        public ScenarioBase()
        {
            ListFiles();
        }

        public abstract void ListFiles();

    }
}
