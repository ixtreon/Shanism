using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    public class FieldWriter
    {
        BinaryWriter Writer { get; }

        public FieldWriter(BinaryWriter writer)
        {
            Writer = writer;
        }

        public void WriteInt(int oldVal, int newVal)
        {
            var areEqual = (oldVal == newVal);
            Writer.Write(areEqual);

            if(areEqual)
                return;

            Writer.Write(newVal - oldVal);
        }
    }
}
