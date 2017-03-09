using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network
{
    static class Ext
    {
        const uint mask = 0xff;
        public static void WriteUint24(this MemoryStream ms, uint val)
        {
            ms.WriteByte((byte)((val >> 16 & mask)));
            ms.WriteByte((byte)((val >> 8 & mask)));
            ms.WriteByte((byte)((val >> 0 & mask)));
        }

        public static uint ReadUint24(this MemoryStream ms)
        {
            var val = (uint)(byte)ms.ReadByte();
            val = ((val << 8) | (byte)ms.ReadByte());
            val = ((val << 8) | (byte)ms.ReadByte());
            return val;
        }
    }
}
