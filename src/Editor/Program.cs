using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shanism.Client;
using Shanism.Common.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Shanism.Editor
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {

        static Dictionary<string, List<string>> allKnownTypes;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ColorScheme.Current = ColorScheme.Dark;
            using (var game = new EditorClient())
            {
                SetStartUpMap(game);

                game.Run();
            }
        }

        static bool TryCompile(string path)
        {
            var trees = GetSyntaxTrees(path);
            var cmp = Compile(trees);

            var cmpErrors = cmp.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error)
                .ToList();

            if (cmpErrors.Any())
                return false;


            var okAssemblyNames = new[]
            {
                "Ix.Core",
                "Shanism.Common",
                "Shanism.Engine",

                "System.Collections",
                "System.Numerics.Vectors",
                "System.Linq",
                "System.Threading.Tasks",
            };

            var loadedAssemblies = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies()
                .ToDictionary(x => x.Name);

            var okAssemblies = okAssemblyNames
                .Select(x =>
                {
                    if (loadedAssemblies.TryGetValue(x, out var a))
                        return Assembly.Load(a);
                    return Assembly.Load(x);
                })
                .ToList();

            var sandbox = new Sandbox3(okAssemblies);
            var errors = sandbox.Check(trees, cmp);

            if (errors.Any())
                return false;

            return true;
        }

        [Conditional("DEBUG")]
        static void SetStartUpMap(EditorClient game)
        {
            //game.StartupMap = "Mechanics Tests";
        }

        static CSharpCompilation Compile(List<SyntaxTree> sourceTrees)
        {
            MetadataReference RefFromPath(string fn)
                => MetadataReference.CreateFromFile(Path.GetFullPath(fn));

            var trustedAssemblyPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))
                .Split(Path.PathSeparator)
                .Select(RefFromPath)
                //.Concat(new[] { RefFromPath("Shanism.Common.dll") })
                .ToList();

            return CSharpCompilation.Create("ShanoScenario",
                sourceTrees,
                trustedAssemblyPaths,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug));
        }

        static List<SyntaxTree> GetSyntaxTrees(string scenarioDir)
        {
            return getSourceFiles(scenarioDir)
                .Select(parseFile)
                .ToList();

            IEnumerable<string> getSourceFiles(string baseDir)
            {
                var files = Directory.EnumerateFiles(baseDir, "*.cs",
                    SearchOption.AllDirectories);

                files = removeDir(baseDir, files, "bin");
                files = removeDir(baseDir, files, "obj");

                return files;
            }

            IEnumerable<string> removeDir(string baseDir, IEnumerable<string> files, string dirName)
            {
                var fullDirName = Path.Combine(baseDir, dirName);
                return files.Where(s => !s.StartsWith(fullDirName, StringComparison.Ordinal));
            }

            SyntaxTree parseFile(string fn)
                => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(fn), null, fn, Encoding.UTF8);
        }

        class AssemblyInfo
        {
            public string Name;
            public List<string> PermittedTypes;

            public AssemblyInfo(string name, Assembly assembly)
            {
                Name = name;
                PermittedTypes = assembly.GetForwardedTypes()
                    .Concat(assembly.GetExportedTypes())
                    .Select(x => x.FullName)
                    .ToList();

            }
            public AssemblyInfo(string name)
                : this(name, Assembly.Load(name))
            { }
        }


        public class Sandbox3
        {
            readonly List<Type> permittedTypes = new List<Type>
            {
                typeof(void),
                typeof(object),
                typeof(bool),

                typeof(char),
                typeof(string),

                typeof(sbyte),
                typeof(short),
                typeof(int),
                typeof(long),

                typeof(byte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),

                typeof(float),
                typeof(double),

                typeof(Nullable<>),

                typeof(IEnumerable),
                typeof(IEnumerable<>),

                typeof(ICollection),
                typeof(ICollection<>),

                typeof(IReadOnlyCollection<>),

                typeof(Action),
                typeof(Action<>),
                typeof(Action<,>),
                typeof(Action<,,>),
                typeof(Action<,,,>),
                typeof(Action<,,,,>),
                typeof(Action<,,,,,>),
                typeof(Action<,,,,,,>),
                typeof(Action<,,,,,,,>),

                typeof(Func<>),
                typeof(Func<,>),
                typeof(Func<,,>),
                typeof(Func<,,,>),
                typeof(Func<,,,,>),
                typeof(Func<,,,,,>),
                typeof(Func<,,,,,,>),
                typeof(Func<,,,,,,,>),

                // runtime.extensions
                typeof(Math),
                typeof(MathF),
                typeof(Convert),

                typeof(TimeSpan),
                typeof(DateTime),

                typeof(StringComparison),
            };

            readonly TypeSandbox sandbox;

            public Sandbox3(List<Assembly> permittedAssemblies)
            {
                var forwardedTypes = permittedAssemblies.SelectMany(x => x.GetForwardedTypes());
                var exportedTypes = permittedAssemblies.SelectMany(x => x.GetExportedTypes());

                permittedTypes.AddRange(forwardedTypes);
                permittedTypes.AddRange(exportedTypes);

                sandbox = new TypeSandbox(permittedTypes);
            }

            public ICollection<Diagnostic> Check(IEnumerable<SyntaxTree> syntaxTrees, CSharpCompilation compilation)
            {
                return sandbox.Check(syntaxTrees, compilation);
            }
        }
    }
}
