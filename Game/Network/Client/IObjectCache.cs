using IO.Common;
using IO.Interfaces.Engine;

namespace Network.Client
{
    public interface IObjectCache
    {
        IGameObject GetOrAdd(ObjectType objType, uint id);
    }
}