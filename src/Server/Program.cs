using Shanism.Engine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Threading;

using static System.Console;

namespace ShanoServer
{

    class Program
    {
        const string runCommand = "start";

        static ShanoEngine engine;

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
                        WriteLine("Just 'stop' for now...");
                        break;

                    case "stop":
                        engine.Stop();
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

            if (!int.TryParse(mapSeedString, out var mapSeed))
            {
                WriteLine($"Unable to parse the given map seed: {mapSeedString}");
                return false;
            }

            //start the server
            WriteLine("Starting the ShanoServer...");
            WriteLine($"    Scenario: {scenarioPath}");
            WriteLine($"    Map Seed: {mapSeed}");

            var _engine = new ShanoEngine(LoadAssembly);
            if (!_engine.TryLoadScenario(scenarioPath, out var errors))
            {
                WriteLine($"Unable to load the scenario at `{scenarioPath}`");
                WriteLine(errors);

                return false;
            }

            _engine.OpenToNetwork();

            engine = _engine;

            var thr = new Thread(() =>
            {
                long start, end;
                const double delay = 1000 / 60;
                double elapsed;
                while (engine.State == Shanism.Common.ServerState.Playing)
                {
                    start = Stopwatch.GetTimestamp();

                    engine.Update((int)delay);

                    end = Stopwatch.GetTimestamp();
                    elapsed = (double)(end - start) / Stopwatch.Frequency * 1000;
                    if (elapsed < delay)
                        Thread.Sleep((int)(delay - elapsed));

                }
            });

            return true;
        }

        static Assembly LoadAssembly(byte[] assemblyBytes, byte[] pdbBytes)
        {
            using (var msAssembly = new MemoryStream(assemblyBytes))
            using (var msPdb = new MemoryStream(pdbBytes))
                return AssemblyLoadContext.Default.LoadFromStream(msAssembly, msPdb);
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
            WriteLine("");
            WriteLine("Usage:");
            WriteLine("\tShanoServer <scenario-path>");
            WriteLine("\t\tStarts the given scenario.");
            WriteLine("");
        }

    }
}
