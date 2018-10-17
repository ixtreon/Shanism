using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using System.Numerics;

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
            if(!Message.ReadBoolean())
                return oldVal;

            return oldVal + Message.ReadInt32(nBits);
        }

        public int ReadVarInt(int oldVal)
        {
            if (!Message.ReadBoolean())
                return oldVal;

            return oldVal + Message.ReadVariableInt32();
        }

        public uint ReadVarUInt(uint oldVal)
        {
            if (!Message.ReadBoolean())
                return oldVal;

            return oldVal + Message.ReadVariableUInt32();
        }

        public byte ReadByte(byte oldVal)
        {
            if (!Message.ReadBoolean())
                return oldVal;

            return (byte)(oldVal + Message.ReadByte());
        }

        public float ReadFloat(float oldVal)
        {
            if (!Message.ReadBoolean())
                return oldVal;

            return oldVal + Message.ReadFloat();
        }

        public string ReadString(string oldVal)
        {
            if (!Message.ReadBoolean())
                return oldVal;

            return Message.ReadString();
        }

        public Color ReadColor(Color oldVal)
        {
            if (!Message.ReadBoolean())
                return oldVal;

            return Color.FromPacked(Message.ReadInt32());
        }

        public bool ReadBool(bool oldVal)
        {
            return Message.ReadBoolean();
        }

        public void ReadPadBits()
        {
            Message.ReadPadBits();
        }

        public Vector2 ReadVector(Vector2 oldVal)
        {
            return new Vector2(ReadFloat(oldVal.X), ReadFloat(oldVal.Y));
        }

        public short ReadShort(short oldVal)
        {
            return (short)ReadVarInt(oldVal);
        }

        public IUnitStats ReadStats(IUnitStats r)
        {
            for (int i = 0; i < r.Count; i++)
                r.RawStats[i] = ReadFloat(r.RawStats[i]);
            return r;
        }

        public IHeroAttributes ReadAttributes(IHeroAttributes r)
        {
            for (int i = 0; i < r.Count; i++)
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
