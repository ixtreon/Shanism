namespace Shanism.Common.Serialization
{
    public interface IWriter
    {
        void WriteByte(byte oldVal, byte newVal);
        void WriteFloat(float oldVal, float newVal);
        void WriteInt(int oldVal, int newVal, int nBits = 32);
        void WriteString(string oldVal, string newVal);
        void WriteVarInt(int oldVal, int newVal);
        void WriteBool(bool oldVal, bool newVal);
        void WriteColor(Color oldVal, Color newVal);

        void WritePadBits();
    }
}