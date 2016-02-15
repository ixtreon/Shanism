using Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShanoServer
{
    class Program
    {
        const string defaultPath = @"..\..\..\Scenarios\DefaultScenario";

        static readonly Regex argRegexHelp = new Regex(@"(-|\/)(help|h|\?)");
        static readonly Regex argRegexScenario = new Regex(@"(-|\/)(sc|scenario)");


        static void Main(string[] args)
        {

            if(args.Any(argRegexHelp.IsMatch))
            {
                Console.WriteLine("ShanoServer help...");
                Console.WriteLine("Parameters:");
                Console.WriteLine("\t-? | -h | -help");
                Console.WriteLine("\t\t\tShow this help dialog. ");
                Console.WriteLine("\t-sc | -scenario <path>");
                Console.WriteLine("\t\t\tUse the scenario found at the specified <path>. ");
                return;
            }

            var scenarioPath = args
                .SkipWhile(a => !argRegexScenario.IsMatch(a))
                .Skip(1)
                .FirstOrDefault();
            
            if(string.IsNullOrEmpty(scenarioPath))
            {
                Console.WriteLine("No scenario path supplied!");
                scenarioPath = Path.GetFullPath(defaultPath);
                Console.WriteLine("Trying to use `{0}` (which probably won't work). ", scenarioPath);
            }

            Console.WriteLine("Starting the ShanoServer. ");

            var z = new ShanoEngine(123, Path.GetFullPath(scenarioPath));

            z.OpenToNetwork();
            z.Start();
        }
    }
}
