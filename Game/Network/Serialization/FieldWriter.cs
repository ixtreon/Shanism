using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Serialization;
using Shanism.Common;

namespace Shanism.Network.Serialization
{
    public class FieldWriter : IWriter
    {
        NetBuffer Message { get; }

        public FieldWriter(NetBuffer message)
        {
            Message = message;
        }

        public void WriteInt(int oldVal, int newVal, int nBits = 32)
        {
            var areEqual = (oldVal == newVal);
            Message.Write(areEqual);

            if(areEqual)
                return;

            Message.Write(newVal - oldVal, nBits);
        }

        public void WriteVarInt(int oldVal, int newVal)
        {
            var areEqual = (oldVal == newVal);
            Message.Write(areEqual);

            if(areEqual)
                return;

            Message.WriteVariableInt32(newVal - oldVal);
        }

        public void WriteByte(byte oldVal, byte newVal)
        {
            var areEqual = (oldVal == newVal);
            Message.Write(areEqual);

            if(areEqual)
                return;

            Message.Write((byte)(newVal - oldVal));
        }

        public void WriteFloat(float oldVal, float newVal)
        {
            var areEqual = oldVal.Equals(newVal);
            Message.Write(areEqual);

            if(areEqual)
                return;

            Message.Write(newVal - oldVal);
        }

        public void WriteString(string oldVal, string newVal)
        {
            var areEqual = oldVal.Equals(newVal);
            Message.Write(areEqual);

            if(areEqual)
                return;

            Message.Write(newVal);
        }

        public void WriteColor(Color oldVal, Color newVal)
        {
            var areEqual = oldVal.Equals(newVal);
            Message.Write(areEqual);

            if (areEqual)
                return;

            Message.Write(newVal.Pack());
        }

        public void WriteBool(bool oldVal, bool newVal)
        {
            Message.Write(newVal);
        }

        public void WritePadBits()
        {
            Message.WritePadBits();
        }
    }
}
