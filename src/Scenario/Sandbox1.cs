using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// The old sandbox which would allow only some namespaces. 
    /// Doesn't do anything else though.
    /// </summary>
    class Sandbox1
    {

        static string[] BannedUserNamespaceStarts { get; } =
        {
            "System",
            "Microsoft",
            "Shanism",
            "Ix",
        };

        static ISet<string> SystemNamespaces = new HashSet<string>
        {
            "System",

            //System.Core
            "System.Linq",

            //mscorlib
            "System.Threading.Tasks",
            "System.Numerics",

            //System
            "System.Collections",
            "System.Collections.Generic",
            "System.Text.RegularExpressions",
        };


        public bool Check(List<SyntaxTree> syntaxTrees, CSharpCompilation compilation, out List<Diagnostic> errors)
        {
            // user namespaces should not be named 
            // as system or shanism ones

            var userNamespaces = syntaxTrees
                .SelectMany(extractNamespaces)
                .ToList();
            errors = userNamespaces
                .Where(n => isForbiddenNamespace(n.Name))
                .Select(n => customError(n.Location, $"Naming your namespace `{n.Name}` is not allowed."))
                .ToList();

            if (errors.Any())
                return false;

            // check all invocations are to methods within 
            // whitelisted OR scenario-declared namespaces

            var userNamespaceSet = userNamespaces
                .Select(ns => ns.Name)
                .ToHashSet();

            errors = syntaxTrees
                .SelectMany(t => extractInvocations(compilation, t))
                .Where(s => !isInvocationAllowed(userNamespaceSet, s.Name))
                .Select(s => customError(s.Location,
                    $"The namespace `{s.Name}` cannot be used in custom scenarios."))
                .ToList();

            return !errors.Any();
        }

        static IEnumerable<(string Name, Location Location)> extractNamespaces(SyntaxTree t)
            => t.GetNamespaces()
                .Select(n => (n.Name.ToString(), n.GetLocation()));


        static IEnumerable<(string Name, Location Location)> extractInvocations(Compilation c, SyntaxTree t)
        {
            var semanticModel = c.GetSemanticModel(t, false);
            foreach (var i in t.GetInvocations())
            {
                var symInfo = semanticModel.GetSymbolInfo(i);
                var name = semanticModel
                    .GetSymbolInfo(i)
                    .Symbol
                    .ContainingNamespace
                    .ToString();

                if (name.StartsWith("System.IO"))
                    throw new Exception();

                yield return (name, i.GetLocation());
            }
        }

        static bool isInvocationAllowed(HashSet<string> userNamespaces, string name)
        {
            if (name.StartsWith("Shanism", StringComparison.Ordinal))
                return true;

            if (SystemNamespaces.Contains(name))
                return true;

            if (userNamespaces.Contains(name))
                return true;

            return false;
        }

        static bool isForbiddenNamespace(string name)
            => BannedUserNamespaceStarts.Any(st => name.StartsWith(st, StringComparison.Ordinal));

        static Diagnostic customError(Location errorLocation, string text)
            => Diagnostic.Create("666", "ShanoError",
                    text,
                    DiagnosticSeverity.Error, DiagnosticSeverity.Error,
                    true, 0,
                    location: errorLocation);

    }
}
