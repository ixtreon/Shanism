using IO.Common;
using IO.Content;
using ProtoBuf;
using System.Collections.Generic;

namespace IO.Objects
{
    public interface IGameObject
    {
        uint Id { get; }

        ObjectType Type { get; }

        Vector Position { get; }

        string ModelName { get; }

        string AnimationName { get; }

        double Scale { get; }

        string Name { get; }
    }
}