using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    public class FieldReader
    {
        BinaryReader Reader { get; }

        public int ReadInt(int oldVal)
        {
            var areEqual = Reader.ReadBoolean();
            if(areEqual)
                return oldVal;

            return oldVal + Reader.ReadInt32();
        }
    }
}
