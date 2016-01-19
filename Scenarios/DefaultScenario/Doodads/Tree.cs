using Engine.Objects.Game;
using IO.Common;

class Tree : Doodad
{
    public Tree(Vector location)
        : base(location)
    {
        ModelName = "pruchka";

        this.Scale = 0.5;
        this.Name = "Tree";
    }
}