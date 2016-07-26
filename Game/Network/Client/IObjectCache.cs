using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;

namespace Shanism.Network.Client
{
    public interface IObjectCache
    {
        bool TryGetValue(uint id, out ObjectStub obj);

        void Add(uint id, ObjectStub obj);
    }
}