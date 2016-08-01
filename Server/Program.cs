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

        static Arg scenarioArg = new Arg
        {
            RegexPattern = @"^(-|/)(sc|scenario):(.+)$",
            CaptureGroup = 3,
            DefaultValue = @"D:/Shanism/Scenarios/MechanicsTests"
        };

        static Arg seedArg = new Arg
        {
            RegexPattern = @"^(-|/)(seed):(\d+)$",
            CaptureGroup = 3,
            DefaultValue = "0"
        };

        static void Main(string[] args)
        {

#if !DEBUG  
            //require the presence of the "start" command
            var cmd = args.LastOrDefault();

            if (cmd != runCommand)
            {
                printUsage();
                return;
            }
#endif

            if (!tryStartEngine(args))
                return;

            //start the repl
            string ln;
            while (true)
            {
                Write(">");
                ln = ReadLine();

                switch (ln)
                {
                    case "help":
                        WriteLine("No commands yet");
                        break;

                    case "stop":
                        engineThread.Abort();
                        return;

                    default:
                        WriteLine("Type `help` for commands, or `stop` to kill the server.");
                        break;
                }
            }
        }

        static bool tryStartEngine(string[] args)
        {
            //parse input
            var scenarioPath = Path.GetFullPath(scenarioArg.Find(args));
            var mapSeedString = seedArg.Find(args);
            int mapSeed;

            if (!int.TryParse(mapSeedString, out mapSeed))
            {
                WriteLine($"Unable to parse the given map seed: {mapSeedString}");

                return false;
            }

            //start the server
            WriteLine("Starting the ShanoServer...");
            WriteLine($"    Scenario: {scenarioPath}");
            WriteLine($"    Map Seed: {mapSeed}");

            var _engine = new ShanoEngine();
            string errors;
            if (!_engine.TryLoadScenario(scenarioPath, mapSeed, out errors))
            {
                WriteLine($"Unable to load the scenario at `{scenarioPath}`");
                WriteLine(errors);

                return false;
            }

            _engine.OpenToNetwork();

            engine = _engine;
            engineThread = _engine.StartBackground();
            return true;
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
