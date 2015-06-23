using IO.Common;
using IO.Content;

namespace IO
{
    public interface IGameObject
    {
        int Guid { get; }
        Vector Location { get; }

        AnimationDef Model { get; }
        double Size { get; }

        string Name { get; }

    }
}