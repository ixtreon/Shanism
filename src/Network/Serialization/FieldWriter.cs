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
    public class FieldWriter
    {
        NetBuffer Message { get; }

        public FieldWriter(NetBuffer message)
        {
            Message = message;
        }

        public void WriteInt(int oldVal, int newVal, int nBits = 32)
        {
            var hasChanged = (oldVal != newVal);

            Message.Write(hasChanged);
            if (hasChanged)
                Message.Write(newVal - oldVal, nBits);
        }

        public void WriteVarInt(int oldVal, int newVal)
        {
            var hasChanged = (oldVal != newVal);

            Message.Write(hasChanged);
            if (hasChanged)
                Message.WriteVariableInt32(newVal - oldVal);
        }

        public void WriteVarUInt(uint oldVal, uint newVal)
        {
            var hasChanged = (oldVal != newVal);

            Message.Write(hasChanged);
            if (hasChanged)
                Message.WriteVariableUInt32(newVal - oldVal);
        }

        public void WriteByte(byte oldVal, byte newVal)
        {
            var hasChanged = (oldVal != newVal);

            Message.Write(hasChanged);
            if (hasChanged)
                Message.Write((byte)(newVal - oldVal));
        }

        public void WriteFloat(float oldVal, float newVal)
        {
            var hasChanged = !oldVal.Equals(newVal);

            Message.Write(hasChanged);
            if (hasChanged)
                Message.Write(newVal - oldVal);
        }

        public void WriteString(string oldVal, string newVal)
        {
            var hasChanged = (oldVal != newVal);

            Message.Write(hasChanged);
            if (hasChanged)
                Message.Write(newVal);
        }

        public void WriteColor(Color oldVal, Color newVal)
        {
            var hasChanged = !oldVal.Equals(newVal);

            Message.Write(hasChanged);
            if (hasChanged)
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

        public void WriteVector(Vector2 oldVal, Vector2 newVal)
        {
            WriteFloat(oldVal.X, newVal.X);
            WriteFloat(oldVal.Y, newVal.Y);
        }

        public void WriteShort(short oldVal, short newVal)
        {
            WriteVarInt(oldVal, newVal);
        }

        public void WriteStats(IUnitStats oldVals, IUnitStats newVals)
        {
            for (int i = 0; i < newVals.Count; i++)
                WriteFloat(oldVals.RawStats[i], newVals.RawStats[i]);
        }

        public void WriteAttributes(IHeroAttributes oldVals, IHeroAttributes newVals)
        {
            for (int i = 0; i < newVals.Count; i++)
                WriteFloat(oldVals.RawStats[i], newVals.RawStats[i]);
        }

        public void WriteMovementState(MovementState oldVal, MovementState newVal)
        {
            WriteByte(oldVal.GetDirectionByte(), newVal.GetDirectionByte());
        }
    }
}
