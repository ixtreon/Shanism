using IO.Common;
using IO.Content;
using System.Collections.Generic;

namespace IO.Objects
{
    public interface IGameObject
    {
        int Guid { get; }
        Vector Position { get; }

        AnimationDefOld Animation { get; }
        double Size { get; }



        string Name { get; }

        ObjectType ObjectType { get; }
    }
}