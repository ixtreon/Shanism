using System;
using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Shanism.Network.Serialization
{
    public class PageWriter2
    {
        const int BitsPerLevel = 64;
        const int BitPerLevelShift = 6;     //log2(BitsPerLine)

        readonly int levels;
        readonly int[] levelSize;
        readonly ulong[][] lines;

        int next(int sz)
            => (int)Math.Ceiling(sz / (decimal)BitsPerLevel);   // round up

        public PageWriter2(int nBits)
        {
            levels = (int)Math.Ceiling(Math.Log(nBits, BitsPerLevel));
            levelSize = new int[levels];
            lines = new ulong[levels][];

            var bitsLeft = nBits;
            var i = 0;
            while (bitsLeft > 1)
            {
                bitsLeft = next(bitsLeft);

                levelSize[i] = bitsLeft;
                lines[i] = new ulong[bitsLeft];

                i++;
            }

            Debug.WriteLine($"Input: {nBits}\tLevels: {levels}\t[{string.Join(", ", levelSize)}]");
        }

        public void Add(int id)
        {
            int page = id, offset;
            for (int lvl = 0; lvl < levels; lvl++)
            {
                offset = (page % BitsPerLevel);
                page >>= BitPerLevelShift;

                lines[lvl][page] |= (1ul << offset);
            }
        }

        public void Write(NetBuffer buf)
        {
            //write the first line completely
            for (var id = 0; id < levelSize[0]; id++)
                buf.Write(lines[0][id]);

            //write all other lines only if they exist
            for (int lvl = 1; lvl < levels; lvl++)
                for (var id = 0; id < levelSize[lvl]; id++)
                {
                    var page = lines[lvl][id];
                    if (page != 0)
                        buf.Write(lines[lvl][id]);
                }
        }
    }
}
