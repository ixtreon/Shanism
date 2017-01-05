using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Serialization
{
    public class PageBase
    {
        public const int BookLength = 64;
        public const int PageLength = 64;
        public const int NBooks = 2;
        public const int NPages = NBooks * PageLength;

        public static void SetBit(ref ulong mask, int bit)
            => mask |= (1ul << bit);

        public static bool GetBit(ulong mask, int bit)
            => (mask & (1ul << bit)) != 0;
    }
}
