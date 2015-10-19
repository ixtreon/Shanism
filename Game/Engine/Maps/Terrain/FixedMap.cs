using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects.Game;
using IO.Common;

namespace Engine.Maps
{
    class FixedMap : ITerrainMap
    {
        readonly Rectangle bounds;

        readonly TerrainType[,] map;

        public int Bottom { get { return bounds.Bottom; } }

        public int Left { get { return bounds.Left; } }
        public int Right { get { return bounds.Right; } }

        public int Top { get { return bounds.Top; } }


        public FixedMap(TerrainType[,] map)
        {
            var w = map.GetLength(0);
            var h = map.GetLength(1);

            this.bounds = new Rectangle(0, 0, w, h);
            this.map = map;
        }

        public void GetMap(Rectangle rect, ref TerrainType[,] outMap)
        {
            outMap = new TerrainType[rect.Width, rect.Height];

            foreach (var ix in Enumerable.Range(0, rect.Width))
                foreach (var iy in Enumerable.Range(0, rect.Height))
                {
                    var mx = rect.X + ix;
                    var my = rect.Y + iy;
                    if (mx >= 0 && mx < bounds.Width && my >= 0 && my < bounds.Height)
                        outMap[ix, iy] = map[mx, my];
                    else
                        outMap[ix, iy] = TerrainType.None;

                }
        }

        public IEnumerable<Doodad> GetNativeDoodads(Rectangle rect)
        {
            return Enumerable.Empty<Doodad>();
        }
    }
}
