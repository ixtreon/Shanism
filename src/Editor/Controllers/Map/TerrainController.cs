using Ix.Math;
using Shanism.Common;
using Shanism.Editor.Actions;
using Shanism.Editor.Models.Brushes;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanism.Editor.Controllers
{
    /// <summary>
    /// Changes map type, size and terrain.
    /// </summary>
    class TerrainController : MapControllerBase
    {
        readonly ActionList history;

        public TerrainType TerrainType { get; set; } = TerrainType.Dirt;

        public TerrainBrushShape Shape { get; set; }

        public float Size { get; set; } = 1;

        public TerrainController(ActionList history, ITerrainMap map)
            : base(map)
        {
            this.history = history;
        }



        public void Apply(IReadOnlyList<Point> points)
        {
            if(points.All(p => Terrain.Get(p) == TerrainType))
                return;

            history.Do(new TerrainBrushAction(Terrain, points, TerrainType));
        }

        /// <summary>
        /// Gets the tiles covered by a terrain brush at the given position.
        /// </summary>
        public IEnumerable<Point> GetHoverTiles(Vector2 pos)
        {
            var r = new Vector2(Size / 2);
            var start = (pos - r).Floor();
            var end = (pos + r).Ceiling();

            if (MapBounds != null)
            {
                start = start.Clamp(MapBounds.Value);
                end = end.Clamp(MapBounds.Value);
            }

            var half = new Vector2(0.5f);
            for (int x = start.X; x < end.X; x++)
                for (int y = start.Y; y < end.Y; y++)
                    if (isInside(Shape, pos, new Vector2(x, y) + half, r))
                        yield return new Point(x, y);
        }

        static bool isInside(TerrainBrushShape shape, Vector2 a, Vector2 b, Vector2 r)
        {
            var d = Vector2.Abs(a - b) / r;
            switch (shape)
            {
                case TerrainBrushShape.Square:
                    return d.X < 1 && d.Y < 1;

                case TerrainBrushShape.Circle:
                    return d.LengthSquared() < 1;
            }
            throw new Exception("Unknown brush shape");
        }
    }
}
