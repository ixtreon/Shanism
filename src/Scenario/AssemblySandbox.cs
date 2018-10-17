using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// Permits method calls only to symbols defined in the given list of assemblies.
    /// 
    /// Fails to do much of anything as most IO, Web, etc. calls are in `System.Private.CoreLib`
    /// along with all other primitives.
    /// </summary>
    class AssemblySandbox
    {

        ISet<string> PermittedAssemblies { get; }

        public AssemblySandbox(IEnumerable<string> permittedAssemblies)
        {
            PermittedAssemblies = new HashSet<string>(permittedAssemblies);
        }


        public ICollection<Diagnostic> Check(IEnumerable<SyntaxTree> syntaxTrees, CSharpCompilation compilation)
        {
            var errors = new List<Diagnostic>();
            var mainAssembly = compilation.Assembly;

            foreach (var tree in syntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(tree, false);
                foreach (var call in tree.GetAllNodes())
                {
                    var symbol = semanticModel.GetSymbolInfo(call).Symbol;
                    if (symbol == null || symbol.Kind == SymbolKind.Namespace)
                        continue;

                    var assembly = symbol.ContainingAssembly;
                    if (assembly == null)
                        continue;

                    if (assembly == mainAssembly)
                        continue;

                    if (PermittedAssemblies.Contains(assembly.Name))
                        continue;

                    //if (symbol.ContainingNamespace.ToDisplayString().StartsWith("System.IO"))
                    //{
                    //    var life = 42;
                    //}

                    //if (symbol.ContainingNamespace.ToDisplayString().StartsWith("System.Net"))
                    //{
                    //    var life = 42;
                    //}

                    var location = call.GetLocation();
                    errors.Add(Error(location,
                        $"The symbol `{symbol.ToDisplayString()}` located in the assembly `{assembly.Name}` cannot be used in custom scenarios."
                    ));

                }
            }

            return errors;
        }

        static Diagnostic Error(Location errorLocation, string text, string errorCode = "666", string category = "ShanoError")
            => Diagnostic.Create(errorCode, category, text, DiagnosticSeverity.Error, DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                warningLevel: 0,
                location: errorLocation);
    }
}
