using Ix.Math;
using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Editor.Controllers;
using System;
using System.Numerics;

namespace Shanism.Editor.Models.Brushes
{
    sealed class SelectionBrush : Brush
    {
        readonly MapControllerBase controller;

        Vector2 selectionStart;
        RectangleF inGameSelection;


        public Color SelectionColor { get; set; } = Color.RoyalBlue.SetAlpha(100);

        public event Action<RectangleF> Select;


        public SelectionBrush(BrushArgs args, MapControllerBase controller)
            : base(args, MouseButton.Left)
        {
            this.controller = controller;
        }

        protected override void ApplyStart(MouseArgs e)
        {
            var inGamePos = Game.Screen.UiToGame(e.AbsolutePosition);
            selectionStart = clampToMap(inGamePos);
        }

        protected override void ApplyEnd(MouseArgs e)
        {
            Select?.Invoke(inGameSelection);
        }

        protected override void Hover(MouseArgs e)
        {
            if (IsApplying)
            {
                var inGamePos = Game.Screen.UiToGame(e.AbsolutePosition);
                var selectionEnd = clampToMap(inGamePos);
                var selectionSize = selectionEnd - selectionStart;

                inGameSelection = new RectangleF(selectionStart, selectionSize);
            }
        }

        Vector2 clampToMap(Vector2 v)
            => controller.MapBounds == null ? v : v.Clamp(controller.MapBounds.Value);

        public override void Draw(Canvas c)
        {
            if (IsApplying)
                c.FillRectangle(inGameSelection, SelectionColor);
        }
    }
}
