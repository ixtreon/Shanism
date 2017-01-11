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
    public class PageWriter : PageBase
    {

        readonly HashSet<uint> oldObjects = new HashSet<uint>();

        readonly ulong[] pages = new ulong[PageCount];

        readonly ulong[] lines = new ulong[TotalLineCount];

        public void UpdatePages(IReadOnlyCollection<IGameObject> objects)
        {
            //reset pages/books
            Array.Clear(pages, 0, PageCount);
            Array.Clear(lines, 0, TotalLineCount);


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
            var pageRowId = (int)(id % LineLength);
            var pageId = id / LineLength;
            SetBit(ref lines[pageId], pageRowId);

            var bookRowId = (int)(pageId % PageLength);
            var bookId = pageId / PageLength;
            SetBit(ref pages[bookId], bookRowId);
        }

        public void Write(NetBuffer msg)
        {
            for(int i = 0; i < PageCount; i++)
                msg.Write(pages[i]);

            for(int i = 0; i < TotalLineCount; i++)
            {
                var page = lines[i];
                if(page != 0)
                    msg.Write(page);
            }
        }
    }
}
