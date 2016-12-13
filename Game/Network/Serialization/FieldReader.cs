﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Serialization
{
    public class FieldReader
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
    }
}
