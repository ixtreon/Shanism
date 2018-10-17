using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shanism.ScenarioLib
{
    public class ScenarioCompiler
    {
        static readonly string[] PermittedAssemblies =
        {
            //"System.Runtime.dll",
            //"System.Runtime.Extensions.dll",

            "System.Linq",
            "System.Private.CoreLib",
            //"System.Text.RegularExpressions.dll",
            //"System.Threading.Tasks.dll",
            //"System.Collections.dll",

            //"System.Numerics.Vectors.dll",
            //"System.ValueTuple.dll",

            "Shanism.Common",
            "Shanism.Engine",
            "Ix.Maths",
        };

        static MetadataReference RefFromPath(string fn)
            => MetadataReference.CreateFromFile(Path.GetFullPath(fn));


        readonly ScenarioConfigReader reader = new ScenarioConfigReader();

        public LoadResult<Scenario> TryCompile(AssemblyLoaderDelegate loader, string scenarioDir)
        {
            var trustedAssemblyPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))
                .Split(Path.PathSeparator)
                .Select(RefFromPath)
                .Concat(new[] { RefFromPath("Shanism.Common.dll") })
                .ToList();


            // load config
            var configResult = reader.TryReadFromDisk(scenarioDir);
            if (!configResult.IsSuccessful)
                return LoadResult<Scenario>.Fail(configResult.Errors);

            // parse the input & prepare the compilation
            var sourceTrees = getSourceFiles(scenarioDir)
                .Select(parseFile)
                .ToList();

            var compilation = CSharpCompilation.Create("ShanoScenario",
                sourceTrees,
                trustedAssemblyPaths,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug));

            var preCompileErrors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error);
            if (preCompileErrors.Any())
                return LoadResult<Scenario>.Fail(getErrorList(preCompileErrors, scenarioDir));

            var sb = new AssemblySandbox(new[]
            {
                "System.Private.CoreLib",   // the dangerous one...

                "System.Linq",
                "System.Collections",
                "System.Numerics.Vectors",

                "Shanism.Common",
                "Shanism.Engine",
                "Ix.Core",
            });

            try
            {
                var errors = sb.Check(sourceTrees, compilation);
            }
            catch (Exception e)
            {
                return LoadResult<Scenario>.Fail();
            }

            // compile
            using (var assStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var result = compilation.Emit(assStream, pdbStream);
                if (!result.Success)
                    return LoadResult<Scenario>.Fail(getErrorList(result.Diagnostics, scenarioDir));

                var ass = loader(assStream.ToArray(), pdbStream.ToArray());
                var sc = new Scenario(configResult.Value, ass);
                return LoadResult<Scenario>.Success(sc);
            }
        }


        static SyntaxTree parseFile(string fn) 
            => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(fn), null, fn, Encoding.UTF8);

        static List<string> getErrorList(IEnumerable<Diagnostic> diagnostics, string rootDir)
            => diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.GetHumanReadableString(rootDir))
                .ToList();

        static IEnumerable<string> getSourceFiles(string baseDir)
        {
            var files = Directory.EnumerateFiles(baseDir, "*.cs",
                SearchOption.AllDirectories);

            files = removeDir(baseDir, files, "bin");
            files = removeDir(baseDir, files, "obj");

            return files;
        }
        static IEnumerable<string> removeDir(string baseDir, IEnumerable<string> files, string dirName)
        {
            var fullDirName = Path.Combine(baseDir, dirName);
            return files.Where(s => !s.StartsWith(fullDirName, StringComparison.Ordinal));
        }
    }
}
