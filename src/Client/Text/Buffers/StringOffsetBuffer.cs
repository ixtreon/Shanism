using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shanism.Client.Models.Text
{

    class StringOffsetBuffer : ICharBuffer<float>
    {
        readonly Font font;
        readonly List<float> offsets = new List<float> { 0 };

        public StringOffsetBuffer(Font font)
        {
            this.font = font;
        }

        public int IndexOf(float pos)
        {
            var rawIndex = IndexOfRaw(pos);
            var baseID = (int)rawIndex;
            var addOne = (rawIndex - baseID) > 0.5f;

            return addOne ? (baseID + 1) : baseID;
        }

        public float IndexOfRaw(float pos)
        {
            var index = offsets.BinarySearch(pos);

            // exactly at some position
            if (index >= 0)
                return index;

            // between 2 values; set index to the larger
            index = ~index;

            // before first or after last
            if (index == 0)
                return index;

            if (index == offsets.Count)
                return index - 1;

            var dCurToSecond = offsets[index] - pos;
            var dFirstToSecond = offsets[index] - offsets[index - 1];
            return index - dCurToSecond / dFirstToSecond;
        }

        public float this[int i]
            => offsets[i];

        public int Length
            => offsets.Count;

        public void Reset()
        {
            offsets.Clear();
            offsets.Add(0);
        }

        public void Delete(int pos, int count)
        {
            var d = offsets[pos + count] - offsets[pos];
            offsets.RemoveRange(pos + 1, count);

            for (int i = pos + 1; i < offsets.Count; i++)
                offsets[i] -= d;
        }

        public void Insert(int pos, char c)
        {
            var d = font.GetWidth(c);
            offsets.Insert(pos + 1, offsets[pos]);

            for (int i = pos + 1; i < offsets.Count; i++)
                offsets[i] += d;
        }

        public void Reset(string text)
        {
            Reset();

            if (string.IsNullOrEmpty(text))
                return;

            var w = 0f;
            for (int i = 0; i < text.Length; i++)
            {
                w += font.GetWidth(text[i]);
                offsets.Add(w);
            }
        }
    }
}
