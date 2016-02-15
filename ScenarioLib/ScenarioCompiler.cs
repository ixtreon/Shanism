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

namespace ScenarioLib
{
    /// <summary>
    /// Compiles a bunch of files referencing IO/Engine
    /// </summary>
    internal class ScenarioCompiler
    {
        #region Static and const members
        public const string _OutputFileName = "scenario.dll";
        public const string _ScenarioSubDir = "scenarios/";

        public const string _OutputFilePath = _ScenarioSubDir + _OutputFileName;
        public const string _OutputPdbPath = _OutputFilePath + ".pdb";


        //TODO: gotta sign the exe...
        static readonly SecurityPermissionFlag[] scenarioPermissions = 
        {
            SecurityPermissionFlag.Execution,
        };

        /// <summary>
        /// The system assemblies used to compile scenarios. 
        /// </summary>
        static readonly string[] systemAssemblies = 
        {
            "System.dll",
            "System.Core.dll",
            "mscorlib.dll",
            "Microsoft.Csharp.dll",
        };

        /// <summary>
        /// The custom assemblies used to compile scenarios. 
        /// </summary>
        private static readonly string[] customAssemblies = new[]
        {
            "IO.dll",
            "Engine.dll",
            "ScenarioLib.dll",
        };


        //private static readonly AppDomain ScenarioAppDomain;
        #endregion


        private string _scenarioDir;
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

        /// <summary>
        /// Gets whether we made a compilation in the given directory. 
        /// </summary>
        public bool IsCompiled { get; private set; }

        public Assembly Assembly { get; private set; }


        /// <summary>
        /// Sets up the sandboxed AppDomain for the compiled scenarios. 
        /// </summary>
        static ScenarioCompiler()
        {
            //apply the permissions
            var permissions = new PermissionSet(PermissionState.None);
            foreach (var p in scenarioPermissions)
                permissions.AddPermission(new SecurityPermission(p));

            var policyLevel = PolicyLevel.CreateAppDomainLevel();
            policyLevel.RootCodeGroup.PolicyStatement = new PolicyStatement(permissions);
        }

        public ScenarioCompiler()
        {
            //setup the sandboxed app domain for custom-made scenarios  

        }

        public ScenarioCompiler(string scenarioDir)
        {
            this.ScenarioDir = scenarioDir;
        }


        /// <summary>
        /// Loads the already compiled assembly. 
        /// Throws an <see cref="InvalidOperationException"/> if <see cref="IsCompiled"/> is false. 
        /// </summary>
        /// <returns></returns>
        public void LoadCompiledAssembly()
        {
            if (!IsCompiled)
                throw new InvalidOperationException("Please compile the scenario first!");

            var rawAssembly = File.ReadAllBytes(_OutputFilePath);
            var rawSymbols = File.ReadAllBytes(_OutputPdbPath);

            //TODO: Load in the sandboxed assembly!
            Assembly = AppDomain.CurrentDomain.Load(rawAssembly, rawSymbols);
        }

        public T CompileAndLoad<T>(out string errors)
            where T : ScenarioFile
        {
            //compile the assemblies

            var compileErrors = Compile();

            if(compileErrors.Any())
            {
                errors = compileErrors
                    .Select(d => d.ToString())
                    .Aggregate((a, b) => a + Environment.NewLine + b);
                return null;
            }

            //parse the scenario config
            var scenario = ScenarioFile.Load<T>(ScenarioDir);
            if (scenario == null)
            {
                errors = "Invalid scenario declaration. ";
                return null;
            }

            //load the assembly
            LoadCompiledAssembly();

            errors = string.Empty;
            return scenario;
        }

        /// <summary>
        /// Compiles all files in the <see cref="ScenarioDir"/> directory. 
        /// If successful returns null, otherwise returns a string containing the compile errors as returned by the compiler. 
        /// </summary>
        public IEnumerable<Diagnostic> Compile()
        {
            if (string.IsNullOrEmpty(ScenarioDir))
                throw new InvalidOperationException("Please select a Scenario directory first!");

            var files = Directory.EnumerateFiles(ScenarioDir, "*.cs", SearchOption.AllDirectories);
            var compilationResult = compile(files, Path.GetFullPath(_OutputFilePath));

            IsCompiled = compilationResult.Success;
            if (IsCompiled)
                return Enumerable.Empty<Diagnostic>();
            return compilationResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error);
        }

        /// <summary>
        /// Uses Roslyn to compile the given files. 
        /// </summary>
        /// <param name="inFiles"></param>
        /// <param name="outFile">The path where the compiled file should be written. </param>
        /// <returns>The result of the compilation. </returns>
        EmitResult compile(IEnumerable<string> inFiles, string outFile)
        {
            //get the syntax tree
            var syntaxTrees = inFiles
                .Select(f => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(f), null, f, Encoding.Default));

            //create the compilation unit using the trusted assemblies
            var systemRefs = systemAssemblies
                .Select(a => MetadataReference.CreateFromFile(getAssemblyDir(a)));
            var customRefs = customAssemblies
                .Select(s => MetadataReference.CreateFromFile(getLocalDir(s)));
            var compilation = CSharpCompilation.Create(
                assemblyName: "scenario.dll",
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug),
                    syntaxTrees: syntaxTrees,
                    references: systemRefs.Concat(customRefs)
                );

            //create directory
            if (!Directory.Exists(_ScenarioSubDir))
                Directory.CreateDirectory(_ScenarioSubDir);

            //compile
            EmitResult result;
            using (var pdbStream = new FileStream(_OutputPdbPath, FileMode.Create))
            using (var outStream = new FileStream(_OutputFilePath, FileMode.Create))
                result = compilation.Emit(outStream, pdbStream: pdbStream);

            return result;
        }
        

        static string getAssemblyDir(string path)
        {
            var assemblyDir = Path.GetDirectoryName(typeof(object).Assembly.Location);
            return Path.Combine(assemblyDir, path);
        }

        static string getLocalDir(string path)
        {
            var currentDir = Directory.GetCurrentDirectory();
            return Path.Combine(currentDir, path);
        }
    }
}