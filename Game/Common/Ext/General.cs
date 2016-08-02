using Shanism.Common.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Shanism.Common
{
    /// <summary>
    /// </summary>
    public static class GeneralExt
    {
        public static IEnumerable<Type> GetTypesDescending<T>(this Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(ty => typeof(T).IsAssignableFrom(ty));
        }
    }
}
