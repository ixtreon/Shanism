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

        readonly ulong[] pages = new ulong[PageCount];

        readonly ulong[] lines = new ulong[TotalLineCount];

        public void UpdatePages(IReadOnlyDictionary<uint, ObjectStub> oldObjects,
            IReadOnlyCollection<IGameObject> objects)
        {
            //reset pages/books
            Array.Clear(pages, 0, PageCount);
            Array.Clear(lines, 0, TotalLineCount);


            var oldSet = new HashSet<uint>(oldObjects.Keys);

            //Add objects that were NOT in the old objects collection
            foreach(var obj in objects)
            {
                var objId = obj.Id;
                if(!oldSet.Remove(objId))
                    Add(objId);
            }

            //Add objects that were in the old collection but NOT in the new 
            foreach(var objId in oldSet)
                Add(objId);

        }

        public void Add(uint id)
        {
            var lineId = id / LineLength;
            var lineChar = (int)(id % LineLength);

            if (GetBit(lines[lineId], lineChar))
                throw new Exception();

            SetBit(ref lines[lineId], lineChar);

            var pageChar = (int)(lineId % PageLength);
            var pageId = lineId / PageLength;
            SetBit(ref pages[pageId], pageChar);
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
