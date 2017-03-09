using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using System;
using System.Reflection;
using Shanism.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            //"Microsoft.Csharp.dll",
        };

        static readonly HashSet<string> whitelistedNamespaces = new HashSet<string>
        {
            "System",

            //System.Core
            "System.Linq",

            //mscorlib
            "System.Threading.Tasks",

            //System
            "System.Collections",
            "System.Collections.Generic",
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

            List<Diagnostic> errors;
            IsCompiled = compile(files, Path.GetFullPath(OutputFilePath), out errors);

            return errors;
        }

        /// <summary>
        /// Uses Roslyn to compile the given files. 
        /// </summary>
        /// <param name="inFiles"></param>
        /// <param name="outFile">The path where the compiled file should be written. </param>
        /// <returns>The result of the compilation. </returns>
        bool compile(IEnumerable<string> inFiles, string outFile, out List<Diagnostic> errors)
        {
            //get the syntax trees
            //List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            //foreach (var f in inFiles)
            //{
            //    var tree = SyntaxFactory.ParseSyntaxTree(File.ReadAllText(f), null, f, Encoding.Default);

            //}
            var syntaxTrees = inFiles
                .Select(f => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(f), null, f, Encoding.Default))
                .ToList();




            //create the compilation unit using the trusted assemblies
            var systemRefs = systemAssemblies
                .Select(a => MetadataReference.CreateFromFile(getAssemblyDir(a)));
            var customRefs = customAssemblies
                .Select(s => MetadataReference.CreateFromFile(getLocalDir(s)));
            var compilation = CSharpCompilation.Create(
                assemblyName: OutputFileName,
                syntaxTrees: syntaxTrees,
                references: systemRefs.Concat(customRefs),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            errors = compilation.GetDiagnostics()
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .ToList();

            if (errors.Any())
                return false;

            //check no custom namespaces 
            //starting with whitelisted namespace names
            var namespaces = syntaxTrees
                .SelectMany(t => t.GetNamespaces())
                .ToList();

            errors = namespaces
                .Where(n => whitelistedNamespaces.Contains(n.Name.ToString()))
                .Select(n => customError(n.GetLocation(), $"Naming your namespace `{n.Name}` is not allowed."))
                .ToList();

            if (errors.Any())
                return false;

            //check only method calls from whitelisted namespaces
            var wlns = new HashSet<string>(whitelistedNamespaces);
            foreach (var ns in namespaces)
                wlns.Add(ns.Name.ToString());

            errors = syntaxTrees.Select(t => new
                {
                    Invocations = t.GetInvocations(),
                    Model = compilation.GetSemanticModel(t, false),
                })
                .SelectMany(s => s.Invocations.Select(i => new
                {
                    Namespace = s.Model
                        .GetSymbolInfo(i)
                        .Symbol
                        .ContainingNamespace
                        .ToString(),
                    Invocation = i,
                }))
                .Where(s
                    => !s.Namespace.StartsWith("Shanism", StringComparison.Ordinal)
                    && !wlns.Contains(s.Namespace))
                .Select(s => customError(
                    s.Invocation.GetLocation(),
                    $"The namespace `{s.Namespace}` cannot be used in custom scenarios."))
                .ToList();

            if (errors.Any())
                return false;

            //foreach (var tree in syntaxTrees)
            //{
            //    var model = compilation.GetSemanticModel(tree, false);
            //    var methodCalls = tree.GetInvocations()
            //        .ToList();

            //    foreach (var mi in methodCalls)
            //    {
            //        var memberSymbol = (IMethodSymbol)model.GetSymbolInfo(mi).Symbol;
            //        var nspace = memberSymbol.ContainingNamespace;
            //        var s_nspace = nspace.ToString();

            //        if (s_nspace.StartsWith("Shanism", StringComparison.Ordinal))
            //            continue;

            //        if (!wlns.Contains(s_nspace))
            //            errors.Add(customError(mi.GetLocation(), $"The namespace `{s_nspace}` cannot be used in custom scenarios."));
            //    }
            //}

            //if (errors.Any())
            //    return false;


            //create directory
            if (!Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);

            //compile
            EmitResult result;
            using (var pdbStream = new FileStream(OutputPdbPath, FileMode.Create))
            using (var outStream = new FileStream(OutputFilePath, FileMode.Create))
                result = compilation.Emit(outStream, pdbStream: pdbStream);

            errors = result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .ToList();
            return result.Success;
        }

        static Diagnostic customError(Location errorLocation, string text)
            => Diagnostic.Create("666", "ShanoError",
                    text,
                    DiagnosticSeverity.Error, DiagnosticSeverity.Error,
                    true, 0,
                    location: errorLocation);

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