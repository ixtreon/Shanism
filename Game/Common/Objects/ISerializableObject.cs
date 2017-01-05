using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    interface ISerializableObject
    {
        void WriteDiff(IWriter w, IGameObject newObject);

        void ReadDiff(IReader r);
    }
}
