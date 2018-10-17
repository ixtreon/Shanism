using Ix.Math;

namespace Shanism.Engine.Systems.Map
{

    public class EntityQuadTree : Ix.Collections.QuadTree<Entity>
    {
        public EntityQuadTree(RectangleF span, float minCellSize = 0, int maxItemsPerNode = 16, int initialNodeCapacity = 64)
            : base(span, minCellSize, maxItemsPerNode, initialNodeCapacity)
        {

        }

        protected override RectangleF GetBounds(Entity item) => item.Bounds;
        protected override bool GetIsAlive(Entity item) => !item.IsDestroyed;

    }
}
