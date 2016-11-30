using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Common
{
    class ObjectBitmaskBuilder
    {
        const int PageSize = 64;
        const int MaxPages = 64;

        const int HeaderLength = MaxPages / 8;
        const int PageLength = PageSize / 8;


        public int Length { get; private set; }

        Dictionary<uint, ulong> pageBuffer = new Dictionary<uint, ulong>();

        public void WriteBitmask(IEnumerable<IGameObject> visibleObjects, BinaryWriter s)
        {
            pageBuffer.Clear();

            ulong pageMask = 0;
            ulong curPage;
            foreach (var o in visibleObjects)
            {
                var id = o.Id;
                var objPage = o.Id / PageSize;
                var objIndex = o.Id % PageSize;

                if (!pageBuffer.TryGetValue(objIndex, out curPage))
                    curPage = 0;

                Set(ref curPage, (int)objIndex);
                Set(ref pageMask, (int)objPage);

                pageBuffer[objPage] = curPage;
            }

            //write pagemask
            s.Write(pageMask);

            //write pages
            foreach (var p in pageBuffer.OrderBy(kvp => kvp.Key))
                s.Write(p.Value);
        }

        public List<uint> VisibleIds { get; } = new List<uint>();

        public void ReadBitmask(BinaryReader s)
        {
            VisibleIds.Clear();

            //read included pages
            var pageMask = s.ReadUInt64();
            //read 
            for (int i = 0; i < MaxPages; i++)
            {
                if (!Get(pageMask, i))
                    continue;

                var startId = i * PageSize;
                var curPage = s.ReadUInt64();
                for (int j = 0; j < PageSize; j++)
                {
                    if (Get(curPage, j))
                        VisibleIds.Add((uint)(startId + j));
                }
            }
        }

        bool Get(ulong mask, int id)
            => (mask & (1ul << id)) != 0;


        void Set(ref ulong mask, int id)
            => mask |= (1ul << id);

    }
}
