using Ix.Math;
using Shanism.Common;
using System.Numerics;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// A map of a fixed size and terrain. 
    /// </summary>
    class FixedMap : ITerrainMap
    {
        readonly TerrainType[,] map;
        readonly int width, height;
        public Rectangle Bounds { get; private set; }

        Rectangle? ITerrainMap.Bounds => Bounds;

        public FixedMap(TerrainType[,] map)
        {
            width = map.GetLength(0);
            height = map.GetLength(1);

            Bounds = new Rectangle(0, 0, width, height);
            this.map = map;
        }

        public void Get(Rectangle rect, ref TerrainType[] outMap)
        {
            outMap = new TerrainType[rect.Width * rect.Height];
            
            for (int ix = rect.Width - 1; ix >= 0; ix--)
                for (int iy = rect.Height - 1; iy >= 0; iy--)
                {
                    var mx = rect.X + ix;
                    var my = rect.Y + iy;
                    if (mx >= 0 && mx < Bounds.Width && my >= 0 && my < Bounds.Height)
                        outMap[ix + rect.Width * iy] = map[mx, my];
                    else
                        outMap[ix + rect.Width * iy] = TerrainType.None;
                }
        }

        public TerrainType Get(Vector2 loc)
        {
            var pt = loc.Floor();
            if (!Bounds.Contains(pt))
                return TerrainType.None;
            return map[pt.X, pt.Y];
        }

        public void Set(Point loc, TerrainType tty)
        {
            if (!Bounds.Contains(loc))
                return;
            map[loc.X, loc.Y] = tty;
        }
    }
}
