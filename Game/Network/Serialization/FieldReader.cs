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
    public class FieldReader : IReader
    {
        NetBuffer Message { get; }


        public FieldReader(NetBuffer message)
        {
            Message = message;
        }


        public int ReadInt(int oldVal, int nBits = 32)
        {
            var areEqual = Message.ReadBoolean();
            if(areEqual)
                return oldVal;

            return oldVal + Message.ReadInt32(nBits);
        }

        public int ReadVarInt(int oldVal)
        {
            var areEqual = Message.ReadBoolean();
            if(areEqual)
                return oldVal;

            return oldVal + Message.ReadVariableInt32();
        }

        public byte ReadByte(byte oldVal)
        {
            var areEqual = Message.ReadBoolean();
            if(areEqual)
                return oldVal;

            return (byte)(oldVal + Message.ReadByte());
        }

        public float ReadFloat(float oldVal)
        {
            var areEqual = Message.ReadBoolean();
            if(areEqual)
                return oldVal;

            return oldVal + Message.ReadFloat();
        }

        public string ReadString(string oldVal)
        {
            var areEqual = Message.ReadBoolean();
            if(areEqual)
                return oldVal;

            return Message.ReadString();
        }

        public Color ReadColor(Color oldVal)
        {
            var areEqual = Message.ReadBoolean();
            if (areEqual)
                return oldVal;

            return new Color(Message.ReadInt32());
        }

        public bool ReadBool(bool oldVal)
        {
            return Message.ReadBoolean();
        }

        public void ReadPadBits()
        {
            Message.ReadPadBits();
        }
    }
}
