using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    class DiffReader
    {
        public BinaryReader Reader { get; }


        public int Read(int oldVal)
        {
            if (Reader.ReadBoolean() == false)
                return oldVal;

            return oldVal + Reader.ReadInt32();
        }

        public float Read(float oldVal)
        {
            if (Reader.ReadBoolean() == false)
                return oldVal;

            return oldVal + Reader.ReadSingle();
        }

        public string Read(string oldVal)
        {
            if (Reader.ReadBoolean() == false)
                return oldVal;

            return Reader.ReadString();
        }
        

    }
}
