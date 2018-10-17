using Shanism.Common;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Ix.Math;
using Shanism.Common.Messages;

namespace Shanism.Editor.Actions
{
    sealed class TerrainBrushAction : ActionBase
    {
        readonly List<(Point, TerrainType)> oldValues = new List<(Point, TerrainType)>();
        readonly ITerrainMap map;

        public TerrainType BrushType { get; }
        public IReadOnlyList<Point> Points { get; }


        public TerrainBrushAction(ITerrainMap map, IReadOnlyList<Point> points, TerrainType brushType)
        {
            this.map = map;

            Points = points;
            BrushType = brushType;
            Description = $"Changed {Points.Count} tiles to {BrushType.ToString().ToLowerInvariant()}.";
        }

        public override void Apply()
        {
            if (!Points.Any())
                return;

            // edit the scenario
            oldValues.Clear();
            foreach (var p in Points)
            {
                oldValues.Add((p, map.Get(p)));
                map.Set(p, BrushType);
            }

            updateTheClient();
        }

        public override void Revert()
        {
            if (!Points.Any())
                return;

            // edit the scenario
            foreach (var (p, tty) in oldValues)
                map.Set(p, tty);

            updateTheClient();
        }

        void updateTheClient()
        {
            var span = getBoundingRect(Points);
            var tty = new TerrainType[span.Area];

            map.Get(span, ref tty);
            Game.SetChunkData(new MapData(span, tty));
        }

        static Rectangle getBoundingRect(IEnumerable<Point> ps)
        {
            var (ax, bx) = getMinMax(ps, p => p.X);
            var (ay, by) = getMinMax(ps, p => p.Y);
            return new Rectangle(ax, ay, bx - ax + 1, by - ay + 1);
        }

        static (int, int) getMinMax(IEnumerable<Point> ps, Func<Point, int> f)
            => (ps.Min(f), ps.Max(f));
    }
}
