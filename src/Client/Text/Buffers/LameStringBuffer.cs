using System.Text;

namespace Shanism.Client.Models.Text
{
    class LameStringBuffer : ICharBuffer<char>
    {
        readonly StringBuilder value = new StringBuilder();

        public char this[int i] => value[i];

        public int Length => value.Length;

        public void Reset()
            => value.Clear();

        public void Delete(int start, int count)
            => value.Remove(start, count);

        public void Insert(int pos, char c)
            => value.Insert(pos, c);

        public void Reset(string text)
        {
            value.Clear();
            value.Append(text);
        }

        public override string ToString()
            => value.ToString();
    }
}
