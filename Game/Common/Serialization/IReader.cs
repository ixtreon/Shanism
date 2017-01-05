namespace Shanism.Common.Serialization
{
    public interface IReader
    {
        byte ReadByte(byte oldVal);
        float ReadFloat(float oldVal);
        int ReadInt(int oldVal, int nBits = 32);
        string ReadString(string oldVal);
        int ReadVarInt(int oldVal);
        bool ReadBool(bool oldVal);

        void ReadPadBits();
        Color ReadColor(Color iconTint);
    }
}