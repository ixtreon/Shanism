using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects;
using Shanism.Common;

namespace Shanism.Engine.Maps
{
    /// <summary>
    /// A map of a fixed size and terrain. 
    /// </summary>
    class FixedMap : ITerrainMap
    {
        readonly TerrainType[,] map;

        public Rectangle Bounds { get; private set; }

        public FixedMap(TerrainType[,] map)
        {
            var w = map.GetLength(0);
            var h = map.GetLength(1);

            Bounds = new Rectangle(0, 0, w, h);
            this.map = map;
        }

        public void Get(Rectangle rect, ref TerrainType[] outMap)
        {
            outMap = new TerrainType[rect.Width * rect.Height];

            foreach (var ix in Enumerable.Range(0, rect.Width))
                foreach (var iy in Enumerable.Range(0, rect.Height))
                {
                    var mx = rect.X + ix;
                    var my = rect.Y + iy;
                    if (mx >= 0 && mx < Bounds.Width && my >= 0 && my < Bounds.Height)
                        outMap[ix + rect.Width * iy] = map[mx, my];
                    else
                        outMap[ix + rect.Width * iy] = TerrainType.None;
                }
        }

        public IEnumerable<Entity> GetNativeEntities(Rectangle rect)
        {
            return Enumerable.Empty<Entity>();
        }

        public TerrainType Get(Vector loc)
        {
            var pt = loc.Floor();
            if (!Bounds.Contains(pt))
                return TerrainType.None;
            return map[pt.X, pt.Y];
        }

        public void SetTerrain(Point loc, TerrainType tty)
        {
            if (!Bounds.Contains(loc))
                return;
            map[loc.X, loc.Y] = tty;
        }
    }
}
