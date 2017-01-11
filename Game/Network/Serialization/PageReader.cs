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
        
        public readonly List<uint> VisibleGuids = new List<uint>();

        readonly ulong[] pages = new ulong[PageCount];

        public PageReader(NetBuffer msg)
        {
            VisibleGuids.Clear();

            for (int i = 0; i < PageCount; i++)
                pages[i] = msg.ReadUInt64();

            for (int i = 0; i < PageCount; i++)
                readPage(msg, i, pages[i]);
        }

        void readPage(NetBuffer msg, int bookId, ulong book)
        {
            var basePageId = bookId * PageLength;

            for(int i = 0; i < PageLength; i++)
                if(GetBit(book, i))
                    readLine(basePageId + i, msg.ReadUInt64());
        }
        void readLine(int pageId, ulong page)
        {
            var baseGuid = pageId * LineLength;

            for(int i = 0; i < LineLength; i++)
                if(GetBit(page, i))
                    VisibleGuids.Add((uint)(baseGuid + i));
        }
    }
}
