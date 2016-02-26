using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IxSerializer
{
    public class Diff2
    {
        public static void Diff(BinaryWriter writer, byte[] oldObject, byte[] newObject)
        {
            //get shorter of the two
            var xorLen = Math.Min(oldObject.Length, newObject.Length);

            //xor using its length
            Xor(writer, oldObject, newObject, xorLen);

            //copy the remainder if newer one has it
            var copyLen = newObject.Length - xorLen;
            if (copyLen > 0)
                writer.Write(newObject, xorLen, copyLen);


        }

        public static byte[] Patch(BinaryReader reader, byte[] obj)
        {
            using (var ms = new MemoryStream())
            {
                return ms.ToArray();
            }
        }

        public static void Xor(BinaryWriter writer, byte[] a, byte[] b, int byteCount)
        {
            if (byteCount == 0) return;

            var unA = new UnionArray { Bytes = a };
            var unB = new UnionArray { Bytes = b };

            /* xor as `long` as long as we can */
            var longCount = byteCount * sizeof(byte) / sizeof(long);
            var longPos = 0;
            do
                writer.Write(unA.Longs[longPos] ^ unB.Longs[longPos]);
            while (++longPos < longCount);

            /* xor the remainder as bytes */
            var bytePos = longPos * sizeof(long) / sizeof(byte);
            while (bytePos < byteCount)
            {
                writer.Write((byte)(unA.Bytes[bytePos] ^ unB.Bytes[bytePos]));
                bytePos++;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        struct UnionArray
        {
            [FieldOffset(0)]
            public byte[] Bytes;

            [FieldOffset(0)]
            public long[] Longs;

            [FieldOffset(0)]
            public int[] Ints;
        }
    }
}
