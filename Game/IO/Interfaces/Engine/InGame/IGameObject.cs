using IO.Common;
using IO.Content;
using System.Collections.Generic;

namespace IO.Objects
{
    public interface IGameObject
    {
        uint Guid { get; }
        Vector Position { get; }

        string ModelName { get; }

        string AnimationName { get; }

        double Scale { get; }



        string Name { get; }

        ObjectType ObjectType { get; }

        RectangleF Bounds { get; }

        RectangleF TextureBounds { get; }
    }
}