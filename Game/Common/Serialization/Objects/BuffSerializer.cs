using System.IO;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;

namespace Shanism.Common.Serialization
{
    class BuffInstanceSerializer : ObjectSerializer
    {
        public override ObjectStub Create(uint id) => new BuffInstanceStub(id);

        public override void Write(BinaryWriter w, IGameObject obj)
        {
            var b = (IBuffInstance)obj;

            w.Write(b.DurationLeft);

        }

        public override void Read(BinaryReader r, IGameObject obj)
        {
            var b = (BuffInstanceStub)obj;

            b.DurationLeft = r.ReadInt32();

        }
    }
}