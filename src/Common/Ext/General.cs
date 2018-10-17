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

        public static double RoundToNearest(this double d, double val)
            => Math.Round(d / val) * val;

        public static float RoundToNearest(this float d, float val)
            => (float)Math.Round(d / val) * val;
    }
}
