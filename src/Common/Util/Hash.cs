using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Contains methods for hashing integers and object types. 
    /// </summary>
    public static class Hash
    {
        public static int GetInt(params int[] vals)
        {
            if (vals == null || vals.Length == 0)
                return 0;

            var ans = vals[0].GetHashCode();
            unchecked
            {
                for (int i = 1; i < vals.Length; i++)
                {
                    uint rol5 = ((uint)ans << 5) | ((uint)ans >> 27);
                    ans = ((int)rol5 + ans) ^ vals[i].GetHashCode();
                }
            }
            return ans;
        }


        public static double GetDouble(params int[] ints)
            => (double)Math.Abs(GetInt(ints)) / int.MaxValue;

        public static float GetFloat(params int[] ints)
            => (float)Math.Abs(GetInt(ints)) / int.MaxValue;
    }
}
