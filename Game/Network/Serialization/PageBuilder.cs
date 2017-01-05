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
    public class PageBuilder : PageBase
    {

        readonly HashSet<uint> oldObjects = new HashSet<uint>();

        readonly ulong[] books = new ulong[NBooks];

        readonly ulong[] pages = new ulong[NPages];

        public void UpdatePages(IReadOnlyCollection<IGameObject> objects)
        {
            //reset pages/books
            Array.Clear(books, 0, NBooks);
            Array.Clear(pages, 0, NPages);


            //Add objects that were NOT in the old objects collection
            foreach(var obj in objects)
            {
                var objId = obj.Id;
                if(!oldObjects.Remove(objId))
                    Add(objId);
            }

            //Add objects that were in the old collection but NOT in the new 
            foreach(var objId in oldObjects)
                Add(objId);


            //reset the old objects collection
            oldObjects.Clear();
            foreach (var obj in objects)
                oldObjects.Add(obj.Id);
        }

        public void Add(uint id)
        {
            var pageRowId = (int)(id % PageBase.PageLength);
            var pageId = id / PageBase.PageLength;
            SetBit(ref pages[pageId], pageRowId);

            var bookRowId = (int)(pageId % PageBase.BookLength);
            var bookId = pageId / PageBase.BookLength;
            SetBit(ref books[bookId], bookRowId);
        }

        public void Write(NetBuffer msg)
        {
            for(int i = 0; i < PageBase.NBooks; i++)
                msg.Write(books[i]);

            for(int i = 0; i < PageBase.NPages; i++)
            {
                var page = pages[i];
                if(page != 0)
                    msg.Write(page);
            }
        }
    }
}
