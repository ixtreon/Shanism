using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Scenario
{
    /// <summary>
    /// Only permits a number of pre-defined types. 
    /// The safest of them sandboxes so far, and still quite possibly shit.
    /// </summary>
    public class TypeSandbox
    {

        readonly HashSet<string> permittedTypeNames;

        public TypeSandbox(IEnumerable<Type> permittedTypes)
        {
            permittedTypeNames = permittedTypes
                .Select(x => x.FullName)
                .ToHashSet();
        }

        public ICollection<Diagnostic> Check(IEnumerable<SyntaxTree> syntaxTrees, CSharpCompilation compilation)
        {
            var errors = new List<Diagnostic>();
            var mainAssembly = compilation.Assembly;

            var wats = compilation.GetSymbolsWithName(x => true)
                .OfType<ITypeSymbol>()
                .Select(x => GetFullName(x));

            var typeLookup = permittedTypeNames
                .Concat(wats)
                .ToHashSet();


            foreach (var tree in syntaxTrees)
            {
                if (tree.FilePath.Contains("GetExecutingAssembly"))
                {
                    var ss = 424123;
                }

                var semanticModel = compilation.GetSemanticModel(tree, false);

                var root = tree.GetRoot() as CompilationUnitSyntax;

                // check root's attributes - don't let any in..
                foreach (var call in root.AttributeLists)
                    errors.Add(Error(call.GetLocation(),
                        $"Adding attributes on the global level is not cool."
                    ));

                // check the descendants
                foreach (var call in root.DescendantNodes())
                {
                    var symbol = semanticModel.GetSymbolInfo(call).Symbol;
                    if (symbol == null)
                        continue;

                    var assembly = symbol.ContainingAssembly;
                    if (assembly == mainAssembly)
                        continue;

                    if (IsOK(symbol))
                        continue;

                    errors.Add(Error(call.GetLocation(),
                        $"The symbol `{symbol.ToDisplayString()}` located in the assembly `{(assembly?.Name ?? "???")}` cannot be used in custom scenarios."
                    ));

                }
            }

            return errors;

            bool IsOK(ISymbol symbol)
            {
                switch (symbol)
                {
                    case INamespaceSymbol ns:
                        return true;
                        
                    case INamedTypeSymbol named:
                        return typeLookup.Contains(GetFullName(named));

                    case IPropertySymbol prop:
                        return IsOK(prop.ContainingType)
                            && IsOK(prop.Type);

                    case IFieldSymbol field:
                        return IsOK(field.ContainingType)
                            && IsOK(field.Type);

                    case IMethodSymbol method:
                        return IsOK(method.ContainingType)
                            && (method.ReturnsVoid || IsOK(method.ReturnType))
                            && method.TypeArguments.All(IsOK);

                    case ILocalSymbol local:
                        return IsOK(local.Type);

                    case IParameterSymbol param:
                        return IsOK(param.Type);

                    case IEventSymbol @event:
                        return IsOK(@event.Type);

                    case IArrayTypeSymbol array:
                        return IsOK(array.ElementType);

                    default:
                        return false;
                }
            }
        }

        static string GetFullName(ITypeSymbol type)
        {
            if (type is INamedTypeSymbol named && named.IsGenericType)
                return $"{GetFullName(type.ContainingNamespace)}.{type.Name}`{named.TypeArguments.Count()}";


            return $"{GetFullName(type.ContainingNamespace)}.{type.Name}";
        }

        static string GetFullName(INamespaceSymbol s)
        {
            if (s.ContainingNamespace == null || string.IsNullOrEmpty(s.ContainingNamespace.Name))
                return s.Name;
            return $"{GetFullName(s.ContainingNamespace)}.{s.Name}";
        }

        static Diagnostic Error(Location errorLocation, string text, string errorCode = "666", string category = "ShanoError")
            => Diagnostic.Create(errorCode, category, text, DiagnosticSeverity.Error, DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                warningLevel: 0,
                location: errorLocation);
    }
}
