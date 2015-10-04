using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioLib.File
{
    public class CSharpSourceFile
    {
        /// <summary>
        /// Gets the path of the file. 
        /// </summary>
        public readonly string FilePath;

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
            var fText = System.IO.File.ReadAllText(FilePath);
            Tree = SyntaxFactory.ParseSyntaxTree(fText, null, FilePath, Encoding.Default);
            Root = Tree.GetRoot();
        }

        public virtual void Save()
        {
            var fileText = Root.ToFullString();
            System.IO.File.WriteAllText(FilePath, fileText);
        }
    }
}
