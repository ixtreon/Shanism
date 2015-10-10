using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Objects
{
    struct ObjectGuid
    {
        static int guidCount = 0;

        public static int GetNew()
        {
            return Interlocked.Increment(ref guidCount);
        }

        //private readonly int value;


        //public static implicit operator int (ObjectGuid guid)
        //{
        //    return guid.value;
        //}
    }
}
