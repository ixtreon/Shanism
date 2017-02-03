using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

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

        public Vector ReadVector(Vector oldVal)
        {
            return new Vector(ReadFloat((float)oldVal.X), ReadFloat((float)oldVal.Y));
        }

        public short ReadShort(short oldVal)
        {
            return (short)ReadVarInt(oldVal);
        }

        public uint ReadVarUInt(uint oldVal)
        {
            var areEqual = Message.ReadBoolean();
            if(areEqual)
                return oldVal;

            return oldVal + Message.ReadVariableUInt32();
        }

        public IUnitStats ReadStats(IUnitStats r)
        {
            for (int i = 0; i < r.RawStats.Length; i++)
                r.RawStats[i] = ReadFloat(r.RawStats[i]);
            return r;
        }

        public IHeroAttributes ReadAttributes(IHeroAttributes r)
        {
            for (int i = 0; i < r.RawStats.Length; i++)
                r.RawStats[i] = ReadFloat(r.RawStats[i]);
            return r;
        }

        public MovementState ReadMovementState(MovementState ms)
        {
            var b = ReadByte(ms.GetDirectionByte());
            return new MovementState(b);
        }
    }
}
