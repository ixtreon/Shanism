using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;

namespace ScenarioLib.File
{
    /// <summary>
    /// Parses the Scenario.cs file in the base directory. 
    /// </summary>
    public class ScenarioFile : CSharpSourceFile
    {
        const string ScenarioFileName = "Scenario.cs";

        const string ListFilesMethodName = "ListFiles";

        const string fileListName = "files";
        const string listAddMethod = "Add";

        const string scenarioNameField = "Name";
        const string scenarioDescriptionField = "Description";

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<string> IncludedFiles { get; private set; } = new HashSet<string>();

        MethodDeclarationSyntax listMethod;

        ClassDeclarationSyntax scenarioClassDeclaration;

        public ScenarioFile(string scenarioDir)
            : base(Path.Combine(scenarioDir, ScenarioFileName))
        {

        }


        public void IncludeFile(string fileName)
        {
            IncludedFiles.Add(fileName);
        }

        public void ExcludeFile(string fileName)
        {
            IncludedFiles.Remove(fileName);
        }

        public override void Load()
        {
            base.Load();

            //load the scenario
            scenarioClassDeclaration = Root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault();
            if (scenarioClassDeclaration == null)
                throw new Exception("The '{0}' file does not contain a class declaration!".F(ScenarioFileName));
            //TODO: handle fucked up class declaration (i.e. wrong base)

            loadFiles();
        }

        public override void Save()
        {
            saveFiles();
            base.Save();
        }

        void loadFiles()
        {
            //get the ListFiles methodss
            listMethod = (MethodDeclarationSyntax)scenarioClassDeclaration
                .GetMethod(ListFilesMethodName);

            if (listMethod == null)
                return;

            var addedFiles = listMethod.Body.Statements
                .Select(ln =>
                    ShanoEditorSyntax.Settings.RecognizeMethodStringInvoke(fileListName, listAddMethod, ln))
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();

            Console.WriteLine("Found {0} added files. ", addedFiles.Count());
            foreach (var fn in addedFiles)
                IncludedFiles.Add(fn);
        }

        void loadAssignments()
        {
            var constr = scenarioClassDeclaration.GetDefaultConstructor();

            //TODO: constr = null?

            var assignments = constr?.FindAssignmentExpressions();

            var nameAssignment = assignments?.FirstOrDefault(a => a.GetAssignmentIdentifierName() == "Name");
            var descriptionAssignment = assignments?.FirstOrDefault(a => a.GetAssignmentIdentifierName() == "Description");

            Name = nameAssignment?.GetAssignmentLiteralValue();
            Description = nameAssignment?.GetAssignmentLiteralValue();
        }

        //rewrite files.add declarations
        void saveFiles()
        {
            Root = Root
                .ReplaceNode(listMethod, listMethod
                    .WithBody(
                        SyntaxFactory.Block(IncludedFiles
                            .Select(f => exprAddFile(f)))))
                .NormalizeWhitespace();
        }

        void saveAssignments()
        {
            throw new NotImplementedException();
        }

        ExpressionStatementSyntax exprAddFile(string path)
        {
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(fileListName),
                        SyntaxFactory.IdentifierName(listAddMethod)
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
