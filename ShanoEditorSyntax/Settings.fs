namespace ShanoEditorSyntax

open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax

open ShanoEditorSyntax.Patterns

// Allows parsing of a ScenarioBase class
module Settings = 
    open System
 
    [<Literal>]
    let ListField = "files"   //the field in scenariobase which holds the declared files

    [<Literal>]
    let AddMethod = "Add"     // the 


    //recognizes "files.Add(<string_literal>)"
    let ActionExpr (n: StatementSyntax) =
        match n with
        | ExpressionStatementSyntax(
            _, 
            InvocationExpressionSyntax(
                _, 
                MemberAccessExpressionSyntax(
                    SyntaxKind.SimpleMemberAccessExpression, 
                    IdentifierNameSyntax(_, expField), 
                    _, 
                    IdentifierNameSyntax(_, expMethod)
                ), 
                ArgumentListSyntax(_, _, args, _)
                ), 
            _) 
            when args.Count = 1 -> 
                let arg = args.[0]
//                let z = Seq.filter (fun a -> true) args
                match arg with
                | ArgumentSyntax(SyntaxKind.Argument, _, _, LiteralExpressionSyntax(SyntaxKind.StringLiteralExpression, sArg)) 
                    -> (sArg.Value.ToString())
                | _ -> null
        | _ -> null