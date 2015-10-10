using Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the ShanoServer. ");

            var scenarioPath = args
                .SkipWhile(a => !(a == "-scenario" || a == "-s"))
                .FirstOrDefault();
            
            if(string.IsNullOrEmpty(scenarioPath))
            {
                scenarioPath = Path.GetFullPath(@"..\..\..\Scenarios\DefaultScenario");
                Console.WriteLine("No scenario path supplied. Using `{0}`. ", scenarioPath);
            }

            var z = new ShanoEngine(123, Path.GetFullPath(scenarioPath));

            z.OpenToNetwork();
            z.Start();
        }
    }
}
