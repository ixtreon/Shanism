using IO.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace IO
{
    /// <summary>
    /// </summary>
    public static class Ext
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
        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string Format(this string format, string s, params object[] args)
        {
            return string.Format(format, args.Prepend(s).ToArray());
        }

        public static string Format(this string format, string s, object o)
        {
            return string.Format(format, new object[] { s, o });
        }



        // Extension methods for the conversion between strings and byte arrays. 

        /// <summary>
        /// Encodes all the characters in the specified string as bytes. 
        /// </summary>
        /// <param name="str">The string containing the characters to encode. </param>
        /// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static bool AlmostEqualTo(this double a, double b, double epsilon = 1E-6)
        {
            return Math.Abs(a - b) <= epsilon;
        }




        public static int Clamp(this int i, int min, int max)
        {
            return Math.Min(max, Math.Max(min, i));
        }
        public static double Clamp(this double i, double min, double max)
        {
            return Math.Min(max, Math.Max(min, i));
        }


        public static string CutOut(this string s, int cutStart, int cutLength)
        {
            var cutEnd = cutStart + cutLength;
            return s.Substring(0, cutStart) + s.Substring(cutEnd);
        }

        public static string Insert(this string s, int pos, string piece)
        {
            return s.Substring(0, pos) + piece + s.Substring(pos);
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
        /// Also uses reflection. 
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
                if(pi != null)
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

        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> e, T item)
        {
            yield return item;
            foreach (var eItem in e)
                yield return eItem;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> e, T item)
        {
            foreach (var eItem in e)
                yield return eItem;
            yield return item;
        }

        public static TVal AddOrGet<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key)
            where TVal : new()
        {
            TVal val;
            if (!dict.TryGetValue(key, out val))
            {
                val = new TVal();
                dict[key] = val;
            }
            return val;
        }


        public static TVal TryGet<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey val)
        {
            TVal result;
            dict.TryGetValue(val, out result);

            return result;
        }


        public static IEnumerable<T> ToEnumerable<T>(this T[,] arr)
        {
            foreach (var z in arr)
                yield return z;
        }

        public static IEnumerable<T> Select<T>(this IEnumerable e, Func<object, T> func)
        {
            foreach (var o in e)
                yield return func(o);
        }

        public static int Count(this IEnumerable e)
        {
            var i = 0;
            foreach (var o in e)
                i++;
            return i;
        }

        public static int GetValue(this Enum val)
        {
            //TODO FUCKING CHANGE
            return (int)Convert.ChangeType(val, TypeCode.Int32);
        }

        public static short GetShortValue(this Enum val)
        {
            //TODO FUCKING CHANGE
            return (short)Convert.ChangeType(val, TypeCode.Int16);
        }
    }
}
