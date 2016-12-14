using Lidgren.Network;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Serialization
{
    public class PageBuilder
    {
        const int BookLength = 64;
        const int PageLength = 64;
        const int NBooks = 2;
        const int NPages = NBooks * PageLength;

        readonly HashSet<uint> oldObjects = new HashSet<uint>();

        ulong[] books = new ulong[NBooks];

        ulong[] pages = new ulong[NPages];

        public void UpdatePages(List<IGameObject> objects)
        {
            //reset pages/books
            Array.Clear(books, 0, NBooks);
            Array.Clear(pages, 0, NPages);


            //Add objects that were NOT in the old objects collection
            for(int i = 0; i < objects.Count; i++)
            {
                var objId = objects[i].Id;
                if(!oldObjects.Remove(objId))
                    Add(objId);
            }

            //Add objects that were in the old collection but NOT in the new 
            foreach(var objId in oldObjects)
                Add(objId);


            //reset the old objects collection
            oldObjects.Clear();
            for(int i = 0; i < objects.Count; i++)
                oldObjects.Add(objects[i].Id);
        }

        public void Add(uint id)
        {
            var pageRowId = (int)(id % PageLength);
            var pageId = id / PageLength;
            pages[pageId] |= (1ul << pageRowId);

            var bookRowId = (int)(pageId % BookLength);
            var bookId = pageId / BookLength;
            books[bookId] |= (1ul << bookRowId);
        }

        public void Write(NetOutgoingMessage msg)
        {
            for(int i = 0; i < NBooks; i++)
                msg.Write(books[i]);

            for(int i = 0; i < NPages; i++)
            {
                var page = pages[i];
                if(page != 0)
                    msg.Write(page);
            }
        }
    }
}
