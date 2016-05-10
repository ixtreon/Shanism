using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shanism.Common
{
    public static class StringsExt
    {
        /// <summary>
        /// Replaces the format item in a specified string with the string representation
        /// of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.
        /// </returns>
        public static string F(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Have a guy with name Pesho and height of 1.72? 
        /// 
        /// Pass me 
        ///     "Hello {Name}, you are {height:0.0} meters high"
        /// 
        /// I pass u 
        ///     "Hello Pesho, you are 1.7 meters high"
        /// 
        /// Also uses reflection so it's slooooooooooow. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string FormatWith<T>(this string s, T obj)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            var patterns = Regex.Matches(s, @"{(\w+)(:[^}\s]*)?}");
            if (patterns.Count == 0)
                return s;

            var t = typeof(T);

            var output = s;
            foreach (Match m in patterns)
            {
                var name = m.Groups[1].Value.ToLower();
                var format = m.Groups[2].Value;
                var pi = t.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pi != null)
                {
                    var ty = pi.PropertyType;
                    var val = pi.GetValue(obj).ToString();
                    if (ty == typeof(int))
                        val = string.Format("{0" + format + "}", pi.GetValue(obj));
                    output = output.Replace(m.Value, val);
                }
                //if (!parseArgs.ContainsKey(name))
                //    continue;
                //var val = parseArgs[name](obj);
                //output = output.Replace(m.Value, val);
            }

            return output;
        }
    }
}
