using Engine.Objects;
using IO.Common;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    class ObjectMap : HashMap<Point, GameObject>
    {
        static readonly GameObjectMapper mapper = new GameObjectMapper(Constants.GameMap.ChunkSize);

        public ObjectMap() 
            : base(mapper)
        {

        }

        public ObjectMap(IObjectMapper<GameObject, Point> mapper)
            : base(mapper)
        {

        }


        /// <summary>
        /// Executes a range query for the objects within the given rectangle. 
        /// </summary>
        /// <param name="pos">The bottom-left point of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>An enumeration of all objects within the rectangle. </returns>
        public IEnumerable<GameObject> RangeQuery(RectangleF rect)
        {
            return RawQuery(rect)
                .Where(obj => rect.Contains(obj.Position));
        }


        public IEnumerable<GameObject> RawQuery(RectangleF rect)
        {
            var start = mapper.GetBinId(rect.Position);
            var end = mapper.GetBinId(rect.FarPosition);

            return BinQuery(start, end);
        }


        public IEnumerable<GameObject> BinQuery(Point centerBin, int binRange)
        {
            var start = centerBin - new Point(binRange);
            var end = centerBin + new Point(binRange);

            return BinQuery(start, end);
        }

        public IEnumerable<GameObject> BinQuery(Point lowerLeftBin, Point upperRightBin)
        {
            return lowerLeftBin.IterateToInclusive(upperRightBin)
                .SelectMany(p => GetBinObjects(p))
                .Distinct();
        }
    }

}
