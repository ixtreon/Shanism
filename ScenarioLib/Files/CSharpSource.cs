using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioLib.Files
{
    public class CSharpSourceFile
    {
        /// <summary>
        /// Gets the path of the file. 
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets whether the file has any uncommited changes. 
        /// </summary>
        public bool IsChanged { get; protected set; }

        /// <summary>
        /// Gets the syntax tree of this source code file. 
        /// </summary>
        public SyntaxTree Tree { get; protected set; }

        /// <summary>
        /// Gets the syntax root of this source code file. 
        /// </summary>
        public SyntaxNode Root { get; protected set; }

        public CSharpSourceFile(string path)
        {
            FilePath = path;
            Load();
        }

        public virtual void Load()
        {
            var fText = File.ReadAllText(FilePath);
            Tree = SyntaxFactory.ParseSyntaxTree(fText, null, FilePath, Encoding.Default);
            Root = Tree.GetRoot();
        }

        public virtual void Save()
        {
            var fileText = Root.ToFullString();
            File.WriteAllText(FilePath, fileText);
        }
    }
}
