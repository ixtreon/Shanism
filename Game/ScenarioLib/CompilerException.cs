using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Shanism.ScenarioLib
{
    internal class CompilerException : Exception
    {
        public CompilerException(Diagnostic err)
            : base(err.GetMessage())
        {

        }
    }
}