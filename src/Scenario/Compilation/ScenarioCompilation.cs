using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Text;
using System;
using System.Reflection;
using Shanism.Common;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// A compilation of a bunch of *.cs files 
    /// as implemented by Shanism scenarios.
    /// </summary>
    class ScenarioCompilation
    {

        public byte[] AssemblyBytes { get; internal set; }

        public byte[] AssemblySymbols { get; internal set; }

        public bool Success { get; internal set; }

        public IEnumerable<Diagnostic> Errors { get; internal set; }

    }
}