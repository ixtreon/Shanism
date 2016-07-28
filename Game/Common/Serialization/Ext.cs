using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    static class Ext
    {
        const int nBytesInInt = sizeof(int) * 8;

        public static void WriteBits(this BinaryWriter w, int val, int nBits)
        {
            Debug.Assert(nBits > 0 && nBits <= nBytesInInt);

            //write int maybe?
            if (nBits == nBytesInInt)
            {
                w.Write(val);
                return;
            }

            w.Write(val < 0);
            nBits--;


            //write byte words
            var nBytes = nBits / 8;
            for (int i = 0, shift = 0; i < nBytes; i++, shift += 8)
            {
                var bits = unchecked((byte)(val >> shift));
                w.Write(bits);
            }

            //write remaining bits 1 by 1
            var endBytesBit = nBytes * 8;
            for (int i = endBytesBit; i < nBits; i++)
            {
                var bit = ((val >> i) & 1) == 1;
                w.Write(bit);
            }
        }

        public static int ReadBits(this BinaryReader r, int nBits)
        {
            Debug.Assert(nBits > 0 && nBits <= nBytesInInt);

            //write int maybe?
            if (nBits == nBytesInInt)
            {
                return r.ReadInt32();
            }

            int val = 0;

            if (r.ReadBoolean())
                val = ~val;
            nBits--;

            //read byte words
            var nBytes = nBits / 8;
            for (int i = 0, shift = 0; i < nBytes; i++, shift += 8)
            {
                var bits = r.ReadByte();
                val |= (bits << shift);
            }

            //read remaining bits 1 by 1
            var endBytesBit = nBytes * 8;
            for (int i = endBytesBit; i < nBits; i++)
            {
                var bit = r.ReadBoolean() ? 1 : 0;
                val |= (bit << i);
            }

            return val;
        }
    }
}
