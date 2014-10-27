using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ScriptLib
{
    /// <summary>
    /// Compiles a bunch of files referencing IO/Engine
    /// </summary>
    public class ScenarioCompiler
    {
        public const string OutputFile = "scenario.dll";

        public readonly string ScenarioDir;


        public ScenarioCompiler(string scenarioDir)
        {
            this.ScenarioDir = scenarioDir;
        }

        protected string Compile()
        {
            //get all the files in AbilityDir and compile them
            var files = Directory.EnumerateFiles(ScenarioDir, "*.cs", SearchOption.AllDirectories);
            var res = CompileFiles(files, OutputFile);
            return res.Success ? null : res.Diagnostics.Aggregate("", (acc, d) => acc + "\r\n" + d.ToString());
        }

        public EmitResult CompileFiles(IEnumerable<string> inFiles, string outFile)
        {
            //get the syntax tree
            
            var syntaxTrees = inFiles.Select(f => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(f)));

            //create the compilation unit. 
            var compilation = CSharpCompilation.Create(outFile,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                    syntaxTrees: syntaxTrees,
                    references: new[] {
                        new MetadataFileReference(getAssemblyDir("mscorlib.dll")),
                        new MetadataFileReference(getAssemblyDir("System.Core.dll")),
                        new MetadataFileReference(getAssemblyDir("System.dll")),
                        new MetadataFileReference(getLocalDir("Engine.dll")),
                        new MetadataFileReference(getLocalDir("IO.dll")),
                    }
                );

            //do the compilation
            using (var file = new FileStream(outFile, FileMode.Create))
            {
                return compilation.Emit(file);
            }
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