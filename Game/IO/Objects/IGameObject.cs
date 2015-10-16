using IO.Common;
using IO.Content;
using System.Collections.Generic;

namespace IO
{
    public interface IGameObject
    {
        int Guid { get; }
        Vector Position { get; }

        AnimationDefOld Model { get; }
        double Size { get; }

        string Name { get; }

        ObjectType ObjectType { get; }

        //IEnumerable<IUnit> SeenBy { get; }
    }
}