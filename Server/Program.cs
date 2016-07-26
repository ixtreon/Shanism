using Shanism.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;

namespace ShanoServer
{
    //  -sc:"D:\Games\War3"
    class Program
    {
        const string runCommand = "start";

        static ShanoEngine engine;
        static Thread engineThread;

        static Arg scenario = new Arg
        {
            RegexPattern = @"^(-|/)(sc|scenario):(.+)$",
            CaptureGroup = 3,
            DefaultValue = @"D:/Shanism/Scenarios/DefaultScenario",
        };

        static void Main(string[] args)
        {

#if !DEBUG
            var cmd = args.LastOrDefault();

            if (cmd != runCommand)
            {
                printUsage();
                return;
            }
#endif

            engineThread = startTheEngine(args, out engine);

            Read();
        }

        static Thread startTheEngine(string[] args, out ShanoEngine engine)
        {
            var scenarioPath = Path.GetFullPath(scenario.Find(args));

            WriteLine("Starting the ShanoServer");
            WriteLine($"Scenario path: {scenarioPath}");


            engine = new ShanoEngine();
            engine.LoadScenario(scenarioPath);
            engine.OpenToNetwork();

            var daemon = engine.StartBackground();

            WriteLine("done!");

            return daemon;
        }

        struct Arg
        {
            public string DefaultValue;
            public string RegexPattern;
            public int CaptureGroup;

            public string Find(string[] args)
            {
                var r = new Regex(RegexPattern);
                var match = args
                    .Select(arg => r.Match(arg))
                    .FirstOrDefault(m => m.Success);

                if (match != null)
                    return match.Captures[CaptureGroup].Value;
                return DefaultValue;
            }
        }


        static void printUsage()
        {
            WriteLine("Usage:");
            WriteLine("\tshanoserver start");
            WriteLine("\t\tStarts the server");
            WriteLine("");
            WriteLine("Optional Parameters:");
            WriteLine("\t(-|/)(sc|scenario):<path>");
            WriteLine("\t\tUse the scenario found at the specified path. ");
        }

    }
}
