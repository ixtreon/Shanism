using IO.Common;

namespace IO
{
    public interface IGameObject
    {
        int Guid { get; }
        Vector Location { get; }

        Model Model { get; }
        double Size { get; }

        string Name { get; }

    }
}