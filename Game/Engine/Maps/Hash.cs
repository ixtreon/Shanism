using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    /// <summary>
    /// Contains extension methods for hashing integers and object types. 
    /// </summary>
    public static class Hash
    {
        public static int GetInt(params int[] ints)
        {
            if (ints.Length == 0)
                throw new ArgumentNullException();

            byte[] bytes;
            if (ints.Length == 1)
                bytes = BitConverter.GetBytes(ints[0]);
            else
                bytes = ints.SelectMany(o => BitConverter.GetBytes(o)).ToArray();

            HashAlgorithm hashGuy = new MD5CryptoServiceProvider();
            hashGuy.Initialize();
            var hashCode = hashGuy.ComputeHash(bytes);

            var val = BitConverter.ToInt32(hashCode, 0);
            return val;
        }

        public static double GetDouble(params int[] ints)
        {
            return (double)Math.Abs(GetInt(ints)) / int.MaxValue;
        }

        public static int GetIntInRage(int range, params int[] ints)
        {
            return (int)(GetDouble(ints) * range);
        }
    }
}
