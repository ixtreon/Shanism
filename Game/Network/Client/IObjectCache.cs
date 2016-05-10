using Shanism.Common.Game;
using Shanism.Common.Interfaces.Engine;

namespace Shanism.Network.Client
{
    public interface IObjectCache
    {
        IGameObject GetOrAdd(ObjectType objType, uint id);
    }
}