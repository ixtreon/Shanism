using Shanism.Common.Interfaces.Objects;
using Shanism.Network.Server;
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

        ulong[] pageBuffer = new ulong[MaxPages];

        public void WriteBitmask(IEnumerable<ObjectData> visibleObjects, BinaryWriter s)
        {
            Array.Clear(pageBuffer, 0, MaxPages);

            ulong pageMask = 0;
            foreach (var o in visibleObjects)
            {
                var id = o.Object.Id;

                var objPageId = id / PageSize;
                var objIndexInPage = id % PageSize;

                Set(ref pageMask, (int)objPageId);
                Set(ref pageBuffer[objPageId], (int)objIndexInPage);
            }

            //write pagemask
            s.Write(pageMask);

            //write pages
            for (int i = 0; i < MaxPages; i++)
                if (Get(pageMask, i))
                    s.Write(pageBuffer[i]);
        }

        public List<uint> VisibleIds { get; } = new List<uint>();

        public void ReadBitmask(BinaryReader s)
        {
            VisibleIds.Clear();

            //read included pages
            var pageMask = s.ReadUInt64();

            //read each page
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
