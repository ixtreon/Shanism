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
        static readonly Regex formatRegex = new Regex(@"{(\w+):([^}\s]*)?}");

        /// <summary>
        /// You are a guy with name Pesho and height of 1.72? 
        /// 
        /// Pass me:
        ///     "Hello {name}, you are {height:0.0} meters high"
        /// 
        /// You get back:
        ///     "Hello Pesho, you are 1.7 meters high"
        /// 
        /// Also uses reflection so it's slooooooooooow. 
        /// </summary>
        public static string FormatWith<T>(this string s, T obj)
        {
            MatchCollection patterns;
            if(s == null || (patterns = formatRegex.Matches(s)).Count == 0)
                return s;

            var output = new StringBuilder(s);
            foreach(Match m in patterns)
            {
                var name = m.Groups[1].Value.ToLower();
                var format = m.Groups[2].Value;
                var pi = typeof(T).GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if(pi != null)
                {
                    var val = string.Format($"{{0:{format}}}", pi.GetValue(obj));

                    output.Replace(m.Value, val);
                }
            }

            return output.ToString();
        }
    }
}
