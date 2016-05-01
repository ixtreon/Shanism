using IO.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IO.Util.Hash
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

        public static int GetInt(params int[] vals)
            => getHash(BitConverter.GetBytes, vals);

        //public static int GetInt(params uint[] vals)
        //    => getHash(BitConverter.GetBytes, vals);

        public static double GetDouble(params int[] ints)
            => (double)Math.Abs(GetInt(ints)) / int.MaxValue;


        static int getHash<T>(Func<T, byte[]> f, params T[] vals)
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
    }
}
