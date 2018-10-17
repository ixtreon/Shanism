using Ix.Math;
using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Editor.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanism.Editor.Models.Brushes
{
    sealed class TerrainBrush : Brush
    {

        readonly TerrainController terrain;
        readonly HashSet<Point> hoverTiles = new HashSet<Point>();

        public Color CursorColor { get; set; } = Color.RoyalBlue.SetAlpha(100);
        public float BrushSize { get; set; } = 1;
        public TerrainBrushShape Shape { get; set; } = TerrainBrushShape.Square;

        public TerrainBrush(BrushArgs args, TerrainController terrain)
            : base(args, MouseButton.Left)
        {
            this.terrain = terrain;

            args.Root.MouseLeave += (o, e) => hoverTiles.Clear();
        }


        protected override void Hover(MouseArgs e)
        {
            var inGamePos = Game.Screen.UiToGame(e.AbsolutePosition);
            var tiles = terrain.GetHoverTiles(inGamePos).ToList();

            if (!IsApplying)
                hoverTiles.Clear();

            foreach (var t in tiles)
                hoverTiles.Add(t);
        }

        protected override void ApplyEnd(MouseArgs e)
        {
            terrain.Apply(hoverTiles.ToList());
            hoverTiles.Clear();
        }


        public override void Draw(Canvas c)
        {
            var sz = 1f / Game.Screen.UI.Scale * Game.Screen.Game.Scale.X;
            foreach (var pt in hoverTiles)
            {
                var pos = Game.Screen.GameToUi(pt);
                c.FillRectangle(pos, new Vector2(sz), CursorColor);
            }
        }

    }

    enum TerrainBrushShape
    {
        Square, Circle,
    }
}