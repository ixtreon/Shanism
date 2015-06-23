using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptEngine.Parsers
{
    public class ScenarioParser : BaseParser
    {
        const string ScenarioFile = "Scenario.cs";

        const string ListFilesMethod = "ListFiles";


        public ScenarioParser(string scenarioDir)
            : base(Path.Combine(scenarioDir, ScenarioFile))
        {

            this.Load();

            //get the first Scenario class declaration
            var scenarioBase = Root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault();

            //get the ListFiles method
            var listMethod = scenarioBase.GetMethod(ListFilesMethod);

            //get the lines in the method
            var listMethodBody = listMethod.ChildNodes().OfType<BlockSyntax>().FirstOrDefault();
            var listLines = listMethodBody.Statements.ToArray();

            var z = ShanoEditorSyntax.Settings.ActionExpr(listLines.First());

            var zz = new ShanoEditorSyntax.Option<int>(13);

            var y = zz.HasValue;
            var u = zz.Value;

            Console.WriteLine("Found: {0}", z);

            var nodes = exprAddFile("lapaaai");

            var str = nodes.ToFullString();
            Console.WriteLine(str);
            var hui = Root.InsertNodesAfter(listLines.Last(), new[] { nodes });
        }

        //private string exprCheck(StatementSyntax st)
        //{
        //    var expr = st as ExpressionStatementSyntax;
        //    //var invoke = st

        //}

        private ExpressionStatementSyntax exprAddFile(string path)
        {
            const string listField = "files";   //the field in scenariobase which holds the declared files
            const string addMethod = "Add";

            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(listField),
                        SyntaxFactory.IdentifierName(addMethod)
                    ),
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Argument(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal(path)
                    ))))
                ));
        }
    }

}
