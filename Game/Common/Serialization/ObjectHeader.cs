using Shanism.Common.Game;
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
    public struct ObjectHeader
    {
        public uint Id;
        public ObjectType Type;
    }
}
