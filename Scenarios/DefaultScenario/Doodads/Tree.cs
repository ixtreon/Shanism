using Engine;
using Engine.Entities.Objects;
using IO.Common;

class Tree : Doodad
{
    public Tree()
    {
        var id = Rnd.Next(1, 3);
        ModelName = "tree-" + id;

        Scale = Rnd.NextDouble(2, 5);
        Name = "Tree";
    }
}