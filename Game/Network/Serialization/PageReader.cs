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

        public PageReader(NetBuffer msg)
        {
            VisibleGuids.Clear();

            for(int i = 0; i < NBooks; i++)
                readBook(msg, i, msg.ReadUInt64());
        }

        void readBook(NetBuffer msg, int bookId, ulong book)
        {
            var basePageId = bookId * BookLength;

            for(int i = 0; i < BookLength; i++)
                if(GetBit(book, i))
                    readPage(basePageId + i, msg.ReadUInt64());
        }
        void readPage(int pageId, ulong page)
        {
            var baseGuid = pageId * PageLength;

            for(int i = 0; i < PageLength; i++)
                if(GetBit(page, i))
                    VisibleGuids.Add((uint)(baseGuid + i));
        }
    }
}
