using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    public class DiffWriter
    {
        public BinaryWriter Writer { get; }


        public void Write(int oldVal, int newVal)
        {
            if (checkIfDefault(oldVal, newVal))
                return;

            var d = newVal - oldVal;
            Writer.Write(d);
        }

        public void Write(float oldVal, float newVal)
        {
            if (checkIfDefault(oldVal, newVal))
                return;

            var d = newVal - oldVal;
            Writer.Write(d);
        }

        public void Write(string oldVal, string newVal)
        {
            if (checkIfDefault(oldVal, newVal))
                return;

            Writer.Write(newVal);
        }

        bool checkIfDefault<T>(T val, T defVal)
        {
            if (val.Equals(defVal))
            {
                Writer.Write(false);
                return true;
            }

            Writer.Write(true);
            return false;
        }


        [StructLayout(LayoutKind.Explicit)]
        public struct IntStruct
        {
            [FieldOffset(0)]
            public byte ByteA;
            [FieldOffset(1)]
            public byte ByteB;
            [FieldOffset(2)]
            public byte ByteC;
            [FieldOffset(3)]
            public byte ByteD;

            [FieldOffset(0)]
            public int IntValue;

            [FieldOffset(0)]
            public uint UIntValue;

            [FieldOffset(0)]
            public float FloatValue;


        }

    }
}
