using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Util.Hash
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

        public static int GetSlow(params int[] vals)
            => getHash(BitConverter.GetBytes, vals);

        public static int Get(params int[] vals)
        {
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < vals.Length; i++)
                    hash = hash * 23 + vals[i];
                return hash;
            }
        }


        public static double GetDouble(params int[] ints)
            => (double)Math.Abs(GetSlow(ints)) / int.MaxValue;


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
