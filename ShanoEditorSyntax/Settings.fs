namespace ShanoEditorSyntax

open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax

open ShanoEditorSyntax.Patterns

// Allows parsing of a ScenarioBase class
module Settings = 
    open System
 
    // files.Add("asdasdsad");

    // files.Add("asd", "asd", 5, 123);

    // rec "files" "Add" line

    //recognizes "field.method(<string_literal>)"
    ///<summary>
    ///nmz e
    ///</summary>
    let RecognizeMethodStringInvoke (fieldName: string) (methodName: string) (n: StatementSyntax) =
        match n with
        | ExpressionStatementSyntax(
            _, 
            InvocationExpressionSyntax(
                _, 
                MemberAccessExpressionSyntax(
                    SyntaxKind.SimpleMemberAccessExpression, 
                    IdentifierNameSyntax(_, SyntaxIdentifier(fieldName)), 
                    _, 
                    IdentifierNameSyntax(_, SyntaxIdentifier(methodName))
                ), 
                ArgumentListSyntax(_, _, args, _)
                ), 
            _) when args.Count = 1 -> 

                let arg = args.[0]
                match arg with
                | ArgumentSyntax(SyntaxKind.Argument, _, _, LiteralExpressionSyntax(SyntaxKind.StringLiteralExpression, sArg)) 
                    -> (sArg.Value.ToString())
                | _ -> null
        | _ -> null
        