using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Serialization
{
    public class PageReader2 : PageBase2
    {
        public List<uint> ChangedGuids { get; private set; } = new List<uint>();

        public PageReader2(int nBits) : base(nBits)
        {
        }

        public void Read(NetBuffer msg)
        {
            ChangedGuids.Clear();

            var offsets = new List<int> { 0 };
            for (int lvl = 0; lvl < levels - 1; lvl++)
            {
                var newOffsets = new List<int>();
                readLevel(msg, offsets, lvl, newOffsets);

                if (!newOffsets.Any())
                    break;

                offsets = newOffsets;
            }
        }

        void readLevel(NetBuffer msg, List<int> offsets, int lvl, List<int> newOffsets)
        {
            foreach (var offset in offsets)
            {
                var mask = msg.ReadUInt64();
                for (int i = 0; i < BitsPerLevel; i++)
                    if ((mask & (1ul << i)) != 0)
                        newOffsets.Add(offset + i);
            }
        }
    }
}
