using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using IO;

namespace ScriptLib
{
    /// <summary>
    /// Compiles a bunch of files referencing IO/Engine
    /// </summary>
    public class ScenarioCompiler
    {
        //TODO: gotta sign the exe...
        private static readonly SecurityPermissionFlag[] scenarioPermissions = new[]
        {
            SecurityPermissionFlag.Execution,
        };

        private static readonly string[] systemAssemblies = new[]
        {
            //system
            "System.dll",
            "System.Core.dll",
            "mscorlib.dll",
            "Microsoft.Csharp.dll",
        };

        private static readonly string[] customAssemblies = new[]
        {
            //shano
            "IO.dll",
            "Engine.dll",
            "scriptengine.dll",
        };



        private static readonly AppDomain ScenarioAppDomain;

        public const string ScenarioSubDir = "current/";
        public const string OutputFile = ScenarioSubDir + "scenario.dll";
        public const string PdbFile = OutputFile + ".pdb";

        public string ScenarioDir
        {
            get { return _scenarioDir; }
            set
            {
                if (_scenarioDir != value)
                {
                    IsCompiled = false;
                    _scenarioDir = value;
                }
            }
        }

        public bool IsCompiled { get; private set; }

        public Assembly Assembly { get; private set; }

        private string _scenarioDir;


        /// <summary>
        /// Sets up the sandboxed AppDomain for the compiled scenarios. 
        /// </summary>
        static ScenarioCompiler()
        {
            //apply the permissions
            var permissions = new PermissionSet(PermissionState.None);
            foreach (var p in scenarioPermissions)
                permissions.AddPermission(new SecurityPermission(p));

            //add extra assemblies. (i.e. this one)
            //var fullTrustAssemblies = trustedAssemblies
            //    .Select(s => Assembly.LoadFile(getLocalDir(s)).Evidence)
            //    .ToArray();

            //var fullTrustAssembly = Assembly.GetExecutingAssembly().Evidence.GetHostEvidence<StrongName>();
            var ass = Assembly.GetEntryAssembly();
            //the ApplicationBase should be different to this one. 
            var adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(ScenarioSubDir);
            
            ScenarioAppDomain = AppDomain.CreateDomain("ScenarioSandbox", null, adSetup, permissions);
        }

        public ScenarioCompiler()
        {
            //setup the sandboxed app domain for custom-made scenarios  

        }

        public ScenarioCompiler(string scenarioDir)
            : base()
        {
            this.ScenarioDir = scenarioDir;
        }

        /// <summary>
        /// Compiles all files in the <see cref="ScenarioDir"/> directory. 
        /// If successful returns null, otherwise returns a string containing the compile errors as returned by the compiler. 
        /// </summary>
        public string Compile()
        {
            var files = Directory.EnumerateFiles(ScenarioDir, "*.cs", SearchOption.AllDirectories);
            return Compile(files);
        }
        /// <summary>
        /// Compiles the given files. 
        /// If successful returns null, otherwise returns a string containing the compile errors as returned by the compiler. 
        /// </summary>
        public string Compile(IEnumerable<string> files)
        {
            if (string.IsNullOrEmpty(ScenarioDir))
                throw new InvalidOperationException("Please select a Scenario directory first!");

            //get all the files in AbilityDir and compile them
            var res = compileFiles(files, Path.GetFullPath(OutputFile));

            IsCompiled = res.Success;

            return IsCompiled ? null : enumDiagnostics(res.Diagnostics, DiagnosticSeverity.Error);
        }

        /// <summary>
        /// Loads the already compiled assembly. 
        /// Throws an <see cref="InvalidOperationException"/> if <see cref="IsCompiled"/> is false. 
        /// </summary>
        /// <returns></returns>
        void LoadAssembly()
        {
            if (!IsCompiled)
                throw new InvalidOperationException("Please compile the scenario first!");

            var rawAssembly = File.ReadAllBytes(OutputFile);
            var rawSymbols = File.ReadAllBytes(PdbFile);
            //TODO: Load in the sandboxed assembly!
            // requires strong signing of IO, Engine
            Assembly = Assembly.Load(rawAssembly, rawSymbols);
        }

        /// <summary>
        /// Tries to compile the given scenario and prints the errors, if any, to the console. 
        /// Returns true if successful. 
        /// </summary>
        /// <returns></returns>
        public T TryCompile<T>()
            where T : class
        {
            string errors;
            return TryCompile<T>(out errors);
        }

        public T TryCompile<T>(out string errors)
            where T : class
        {
            var result = Compile();

            if (result != null)
            {
                errors = result;
                return null;
            }

            LoadAssembly();

            var scenarios = Assembly.CreateInstanceOfEach<T>();

            if (!scenarios.Any())
            {
                errors = "No scenarios found in the assembly!";
                return null;
            }

            if (scenarios.Count() > 1)
                Console.WriteLine("Found 2 scenarios. Picking a random one...");

            errors = null;
            return scenarios.First();

        }


        /// <summary>
        /// Returns the diagnostics in a human-readable format. 
        /// </summary>
        private string enumDiagnostics(IEnumerable<Diagnostic> ds, DiagnosticSeverity s)
        {
            return ds.Where(d => d.Severity == s)
                .Aggregate("", (acc, d) => acc + Environment.NewLine + d.ToString());
        }

        /// <summary>
        /// Uses Roslyn to compile the given files. 
        /// </summary>
        private EmitResult compileFiles(IEnumerable<string> inFiles, string outFile)
        {
            //get the syntax tree
            var syntaxTrees = inFiles.Select(f => 
                {
                    var st = SyntaxFactory.ParseSyntaxTree(File.ReadAllText(f), null, f, Encoding.Default);
                    return st;
                });

            //create the compilation unit using the trusted assemblies
            var compilation = CSharpCompilation.Create(
                assemblyName: "scenario.dll",
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug),
                    syntaxTrees: syntaxTrees,
                    references: systemAssemblies.Select(a => MetadataReference.CreateFromFile(getAssemblyDir(a)))
                        .Concat(customAssemblies.Select(s => MetadataReference.CreateFromFile(getLocalDir(s))))
                );

            //create directory
            if (!Directory.Exists(ScenarioSubDir))
                Directory.CreateDirectory(ScenarioSubDir);

            //compile
            EmitResult result;
            using (var pdbStream = new FileStream(PdbFile, FileMode.Create))
                using (var outStream = new FileStream(OutputFile, FileMode.Create))
                    result = compilation.Emit(outStream, pdbStream: pdbStream);

            return result;
        }


        protected static string getAssemblyDir(string path)
        {
            var assemblyDir = Path.GetDirectoryName(typeof(object).Assembly.Location);
            return Path.Combine(assemblyDir, path);
        }

        protected static string getLocalDir(string path)
        {
            var currentDir = Directory.GetCurrentDirectory();
            return Path.Combine(currentDir, path);
        }
    }
}