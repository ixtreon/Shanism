using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    public class PageBuilder
    {
        const int NBooks = 2;

        long[] books = new long[NBooks];

        Dictionary<int, long> Pages { get; } = new Dictionary<int, long>();

        public void MakePages(Dictionary<uint, ObjectStub> oldObjects, List<IGameObject> objects)
        {
            foreach(var obj in oldObjects.Values)
                obj._ReaderFlag = false;

            foreach(var obj in objects)
            {
                ObjectStub existingObj;
                if(oldObjects.TryGetValue(obj.Id, out existingObj))
                    existingObj._ReaderFlag = true;
                else
                    Add(obj.Id);
            }
            foreach(var obj in oldObjects.Values)
                if(!obj._ReaderFlag)
                    Add(obj.Id);
        }

        public void Add(uint id)
        {
            var pageId = id % 64;
            var bookId = id / 64;

            throw new NotImplementedException();
        }

        public void Write(BinaryWriter wr)
        {
            throw new NotImplementedException();
        }
    }
}
