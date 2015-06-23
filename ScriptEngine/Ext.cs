using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ScriptEngine
{
    static class Ext
    {

        public static MemberDeclarationSyntax GetMethod(this ClassDeclarationSyntax cls, string name)
        {
            return cls.Members
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.GetMethodName() == name);
        }

        public static string GetMethodName(this MemberDeclarationSyntax member)
        {
            return member.ChildTokens().First(tk => tk.Kind() == SyntaxKind.IdentifierToken).Text;
        }
    }
}
