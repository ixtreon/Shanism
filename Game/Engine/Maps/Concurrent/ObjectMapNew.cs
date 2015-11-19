using Engine.Objects;
using IO.Common;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps.Concurrent
{
    class ObjectMapNew : HashMapNew<Point, GameObject>
    {
        static readonly GameObjectMapper mapper = new GameObjectMapper(Constants.ObjectMap.CellSize);

        public ObjectMapNew(IObjectMapper<GameObject, Point> mapper) 
            : base(mapper)
        {

        }


        /// <summary>
        /// Executes a range query for the objects within the given rectangle. 
        /// </summary>
        /// <param name="pos">The bottom-left point of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>An enumeration of all objects within the rectangle. </returns>
        public IEnumerable<GameObject> RangeQuery(Vector pos, Vector size)
        {
            return RawQuery(pos, size)
                .Where(obj => obj.Position.Inside(pos, size));
        }


        public IEnumerable<GameObject> RawQuery(Vector pos, Vector size)
        {
            var start = mapper.GetBinId(pos);
            var end = mapper.GetBinId(pos + size);

            return start.IterateToInclusive(end).SelectMany(p => GetBinContents(p));
        }

    }

    class GameObjectMapper : IObjectMapper<GameObject, Point>
    {
        double cellSize { get; }

        public GameObjectMapper(double cellSize)
        {
            this.cellSize = cellSize;
        }

        public Point GetBinId(GameObject obj)
        {
            return GetBinId(obj.Position);
        }

        public Point GetBinId(Vector pos)
        {
            return (pos / cellSize).Floor();
        }

        public bool ShouldRemove(GameObject obj)
        {
            return obj.IsDestroyed;
        }

        public void Update(GameObject obj, int msElapsed)
        {
            obj.Update(msElapsed);
        }
    }
}
