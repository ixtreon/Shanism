using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Shanism.ScenarioLib
{
    static class Ext
    {

        public static MemberDeclarationSyntax GetMethod(this ClassDeclarationSyntax cls, string name)
        {
            return cls.Members
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.GetMethodName() == name);
        }

        public static PropertyDeclarationSyntax GetProperty(this ClassDeclarationSyntax cls, string name)
        {
            return cls.Members
                .OfType<PropertyDeclarationSyntax>()
                .FirstOrDefault(m => m.GetMethodName() == name);
        }

        public static IEnumerable<AssignmentExpressionSyntax> FindAssignmentExpressions(this BaseMethodDeclarationSyntax method)
        {
            return method.Body
                .ChildNodes()
                .OfType<AssignmentExpressionSyntax>();
        }

        public static ConstructorDeclarationSyntax GetParameterlessConstructor(this ClassDeclarationSyntax expr)
        {
            return expr.Members
                .OfType<ConstructorDeclarationSyntax>()
                .FirstOrDefault(c => c.ParameterList.Parameters.Count == 0);
        }

        public static string GetMethodName(this MemberDeclarationSyntax member)
        {
            return member.ChildTokens().First(tk => tk.Kind() == SyntaxKind.IdentifierToken).Text;
        }

        public static string GetAssignmentIdentifierName(this AssignmentExpressionSyntax expr)
        {
            var identifier = (expr.Left as IdentifierNameSyntax);
            return identifier?.Identifier.Value.ToString();
        }

        public static string GetAssignmentLiteralValue(this AssignmentExpressionSyntax expr)
        {
            var identifier = (expr.Right as LiteralExpressionSyntax);
            return identifier?.Token.Value.ToString();
        }
    }
}
