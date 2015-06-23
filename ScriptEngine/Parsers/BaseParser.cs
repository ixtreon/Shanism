using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptEngine.Parsers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseParser
    {
        public readonly string FilePath;

        public bool IsChanged;

        public SyntaxTree Tree { get; protected set; }

        public SyntaxNode Root { get; protected set; }

        public BaseParser(string path)
        {
            this.FilePath = path;
        }

        public void Load()
        {
            var fText = File.ReadAllText(FilePath);
            Tree = SyntaxFactory.ParseSyntaxTree(fText, null, FilePath, Encoding.Default);
            Root = Tree.GetRoot();
        }
    }
}
