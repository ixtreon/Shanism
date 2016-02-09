using IO.Common;
using IO.Content;
using ProtoBuf;
using System.Collections.Generic;

namespace IO.Objects
{
    public interface IGameObject
    {
        ObjectType Type { get; }

        uint Guid { get; }

        Vector Position { get; }

        string ModelName { get; }

        string AnimationName { get; }

        double Scale { get; }

        string Name { get; }
    }
}