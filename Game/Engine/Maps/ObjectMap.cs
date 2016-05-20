using Shanism.Engine.Objects;
using Shanism.Common.Game;
using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Engine.Maps
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
            return RawRangeQuery(rect)
                .Where(obj => rect.Contains(obj.Position));
        }


        /// <summary>
        /// Executes a fast range query for all objects lying inside or around the given rectangle. 
        /// Returned units may not actually lie in the provided rectangle. 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public IEnumerable<Entity> RawRangeQuery(RectangleF rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                return Enumerable.Empty<Entity>();

            var start = DefaultMapper.GetBinId(rect.Position);
            var end = DefaultMapper.GetBinId(rect.FarPosition);

            return RawQuery(start, end);
        }

        public IEnumerable<Entity> RawQuery(Point lowerLeftBin, Point upperRightBin)
        {
            var entities = new HashSet<Entity>();

            for (int ix = lowerLeftBin.X; ix <= upperRightBin.X; ix++)
                for (int iy = lowerLeftBin.Y; iy <= upperRightBin.Y; iy++)
                    foreach (var e in BinQuery(new Point(ix, iy)))
                        entities.Add(e);

            return entities;
        }
    }

}
