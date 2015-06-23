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

namespace ScriptLib
{
    /// <summary>
    /// Compiles a bunch of files referencing IO/Engine
    /// </summary>
    public class ScenarioCompiler
    {
        private static readonly SecurityPermissionFlag[] scenarioPermissions = new[]
        {
            SecurityPermissionFlag.Execution,
        };

        private static readonly string[] trustedAssemblies = new[] 
        {
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

        public string Compile()
        {
            if (string.IsNullOrEmpty(ScenarioDir))
                throw new InvalidOperationException("Please select a Scenario directory first!");

            //get all the files in AbilityDir and compile them
            var files = Directory.EnumerateFiles(ScenarioDir, "*.cs", SearchOption.AllDirectories);
            var res = compileFiles(files, Path.GetFullPath(OutputFile));

            IsCompiled = res.Success;

            return IsCompiled ? null : enumDiagnostics(res.Diagnostics, DiagnosticSeverity.Error);
        }


        private string enumDiagnostics(IEnumerable<Diagnostic> ds, DiagnosticSeverity s)
        {
            return ds.Where(d => d.Severity == s)
                .Aggregate("", (acc, d) => acc + Environment.NewLine + d.ToString());
        }

        public Assembly Load()
        {
            if (!IsCompiled)
                throw new InvalidOperationException("Please compile the scenario first!");

            var rawAssembly = File.ReadAllBytes(OutputFile);
            var rawSymbols = File.ReadAllBytes(PdbFile);
            //TODO: Load in the sandboxed assembly!
            // requires strong signing of IO, Engine
            var assembly = Assembly.Load(rawAssembly, rawSymbols);

            return assembly;
        }

        private EmitResult compileFiles(IEnumerable<string> inFiles, string outFile)
        {
            //get the syntax tree
            var syntaxTrees = inFiles.Select(f => 
                {
                    var st = SyntaxFactory.ParseSyntaxTree(File.ReadAllText(f), null, f, Encoding.Default);
                    return st;
                });

            //create the compilation unit. 
            var compilation = CSharpCompilation.Create("scenario.dll",
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug),
                    syntaxTrees: syntaxTrees,
                    references: new[]
                    {
                        MetadataReference.CreateFromFile(getAssemblyDir("mscorlib.dll")),
                        MetadataReference.CreateFromFile(getAssemblyDir("System.Core.dll")),
                        MetadataReference.CreateFromFile(getAssemblyDir("System.dll")),
                    }.Concat(trustedAssemblies.Select(s => MetadataReference.CreateFromFile(getLocalDir(s))))
                );

            EmitResult result;
            //do the compilation
            if (!Directory.Exists(ScenarioSubDir))
                Directory.CreateDirectory(ScenarioSubDir);
            using (var pdbStream = new FileStream(PdbFile, FileMode.Create))
                using (var outStream = new FileStream(OutputFile, FileMode.Create))
                {
                    result = compilation.Emit(outStream, pdbStream: pdbStream);
                }
            return result;
        }


        public byte[] PackageContent()
        {
            return null;
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