using Shanism.Common.StubObjects;

namespace Shanism.Common.Serialization
{
    class HeroSerializer : UnitSerializer
    {
        public override ObjectStub Create(uint id) => new HeroStub(id);

        //TODO
    }
}