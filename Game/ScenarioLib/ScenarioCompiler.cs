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
using Shanism.Common;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// Compiles a bunch of files referencing IO/Engine
    /// </summary>
    internal class ScenarioCompiler
    {
        #region Static and const members
        public const string OutputFileName = "scenario.dll";
        public const string OutputDirectory = "Scenario/";

        public const string OutputFilePath = OutputDirectory + OutputFileName;
        public const string OutputPdbPath = OutputFilePath + ".pdb";


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
        static readonly string[] customAssemblies = 
        {
            "Shanism.Common.dll",
            "Shanism.Engine.dll",
        };


        //private static readonly AppDomain ScenarioAppDomain;
        #endregion


        /// <summary>
        /// Gets whether we made a compilation in the given directory. 
        /// </summary>
        public bool IsCompiled { get; private set; }

        public Assembly Assembly { get; private set; }

        public string ScenarioDir { get; private set; }



        /// <summary>
        /// Sets up the sandboxed AppDomain for the compiled scenarios. 
        /// </summary>
        static ScenarioCompiler()
        {
            Sandboxer.Init();
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
        public bool LoadCompiledAssembly(out string errors)
        {
            if (!IsCompiled)
                throw new InvalidOperationException("Please compile the scenario first!");

            var rawAssembly = File.ReadAllBytes(OutputFilePath);
            var rawSymbols = File.ReadAllBytes(OutputPdbPath);

            /// Using a sandboxed assembly
            //Sandboxer.Init();
            //var loader = Sandboxer.Create();
            //loader.LoadAssembly(rawAssembly, rawSymbols);

            /// Using the current assembly
            try
            {
                Assembly = Assembly.Load(rawAssembly, rawSymbols);
            }
            catch (Exception e)
            {
                errors = e.Message;
                return false;
            }

            errors = string.Empty;
            return true;
        }

        /// <summary>
        /// Compiles all files in the <see cref="ScenarioDir"/> directory. 
        /// If successful returns null, otherwise returns the compile errors. 
        /// </summary>
        public IEnumerable<Diagnostic> Compile()
        {
            if (string.IsNullOrEmpty(ScenarioDir))
                throw new InvalidOperationException("Please select a Scenario directory first!");

            var files = Directory.EnumerateFiles(ScenarioDir, "*.cs", SearchOption.AllDirectories);
            var compilationResult = compile(files, Path.GetFullPath(OutputFilePath));

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
                assemblyName: OutputFileName,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug),
                    syntaxTrees: syntaxTrees,
                    references: systemRefs.Concat(customRefs)
                );

            //create directory
            if (!Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);

            //compile
            EmitResult result;
            using (var pdbStream = new FileStream(OutputPdbPath, FileMode.Create))
            using (var outStream = new FileStream(OutputFilePath, FileMode.Create))
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