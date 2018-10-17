using Ix.Math;
using System.Numerics;

namespace Shanism.Engine.Systems.Map
{
    class EntityAABBTree : Ix.Collections.AABBTree<Entity>
    {

        protected override RectangleF GetBounds(Entity item)
        {
            return new RectangleF(item.Position - new Vector2(item.Scale / 2), new Vector2(item.Scale));
        }
    }
}
