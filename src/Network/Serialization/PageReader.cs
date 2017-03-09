using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Serialization
{
    public class PageReader : PageBase
    {
        
        public List<uint> ChangedGuids { get; } = new List<uint>();

        readonly ulong[] pages = new ulong[PageCount];


        public void Read(NetBuffer msg)
        {
            ChangedGuids.Clear();

            for (int i = 0; i < PageCount; i++)
                pages[i] = msg.ReadUInt64();

            for (int i = 0; i < PageCount; i++)
                readPage(msg, i, pages[i]);
        }

        void readPage(NetBuffer msg, int pageId, ulong page)
        {
            var pageStart = pageId * PageLength;

            for(int i = 0; i < PageLength; i++)
                if(GetBit(page, i))
                    readLine(pageStart + i, msg.ReadUInt64());
        }
        void readLine(int lineId, ulong line)
        {
            var lineStart = lineId * LineLength;

            for (int i = 0; i < LineLength; i++)
                if (GetBit(line, i))
                {
                    ChangedGuids.Add((uint)(lineStart + i));
                }
        }
    }
}
