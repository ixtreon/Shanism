using IO.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    /// <summary>
    /// Contains methods for hashing integers and object types. 
    /// </summary>
    public static class Hash
    {
        static readonly HashAlgorithm hashGuy = new MD5CryptoServiceProvider();

        static Hash()
        {
            hashGuy.Initialize();
        }

        public static int GetInt(params int[] ints)
        {
            return getHash(BitConverter.GetBytes, ints);
        }

        public static int GetInt(params uint[] uints)
        {
            return getHash(BitConverter.GetBytes, uints);
        }

        public static int getHash<T>(Func<T, byte[]> f, params T[] vals)
        {
            if (vals.Length == 0)
                throw new ArgumentNullException();

            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                foreach (var el in vals)
                {
                    var iBytes = f(el);
                    ms.Write(iBytes, 0, iBytes.Length);
                }

                buffer = ms.ToArray();
            }

            var hashCode = hashGuy.ComputeHash(buffer);
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
