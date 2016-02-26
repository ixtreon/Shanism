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
    class ObjectMap : HashMap<Point, Entity>
    {
        public static readonly GameObjectMapper DefaultMapper = new GameObjectMapper(Constants.GameMap.ChunkSize);

        public ObjectMap() 
            : base(DefaultMapper)
        {

        }

        public ObjectMap(IObjectMapper<Entity, Point> mapper)
            : base(mapper)
        {

        }



        /// <summary>
        /// Executes a range query for the objects within the given rectangle. 
        /// </summary>
        /// <returns>An enumeration of all objects within the rectangle. </returns>
        public IEnumerable<Entity> RangeQuery(RectangleF rect)
        {
            return RawQuery(rect)
                .Where(obj => rect.Contains(obj.Position));
        }

        /// <summary>
        /// Executes a fast range query for all objects lying inside or around the given rectangle. 
        /// Returned units may not actually lie in the provided rectangle. 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public IEnumerable<Entity> RawQuery(RectangleF rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                return Enumerable.Empty<Entity>();

            var start = DefaultMapper.GetBinId(rect.Position);
            var end = DefaultMapper.GetBinId(rect.FarPosition);

            return BinQuery(start, end);
        }

        /// <summary>
        /// Returns all objects in the bins in a given range from a specified center bin. 
        /// </summary>
        /// <param name="centerBin">The map bin at the center of the area. </param>
        /// <param name="binRange">The range, in map bins, to select units. </param>
        /// <returns>All objects lying inside the center bin or a bin in the specified range from it. </returns>
        public IEnumerable<Entity> BinQuery(Point centerBin, int binRange)
        {
            var start = centerBin - new Point(binRange);
            var end = centerBin + new Point(binRange);

            return BinQuery(start, end);
        }

        public IEnumerable<Entity> BinQuery(Point lowerLeftBin, Point upperRightBin)
        {
            return lowerLeftBin.IterateToInclusive(upperRightBin)
                .SelectMany(p => BinQuery(p))
                .Distinct();
        }
    }

}
