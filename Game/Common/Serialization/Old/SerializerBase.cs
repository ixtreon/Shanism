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
    abstract class SerializerBase
    {
        public abstract ObjectStub Create(uint id);

        public abstract void Write(BinaryWriter w, IGameObject obj);

        public abstract void Read(BinaryReader r, IGameObject obj);
    }
}
