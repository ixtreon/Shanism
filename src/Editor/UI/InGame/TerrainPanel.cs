using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Common;
using Shanism.Editor.Controllers;
using Shanism.Editor.Models.Brushes;
using System;
using System.Numerics;

namespace Shanism.Editor.UI.InGame
{
    /// <summary>
    /// A panel for terrain modification and placement.
    /// </summary>
    class TerrainPanel : ListPanel
    {

        const float ButtonWidth = 0.22f;
        const float ButtonHeight = 0.08f;

        public TerrainPanel(TerrainController controller)
            : base(Direction.TopDown)
        {

            BackColor = UiColors.ControlBackground;
            Size = new Vector2(0.5f);
            Direction = Direction.TopDown;

            // shapes
            var shapeLabel = new Label("Brush Shape", true) { Font = Content.Fonts.NormalFont };

            var square = ShapeButton("plain-square");
            var circle = ShapeButton("plain-circle");

            square.IsSelected = true;

            var shapes = new ListPanel(sizeMode: ListSizeMode.ResizeBoth)
            {
                square, circle
            };

            // sizes
            var sizeLabel = new Label("Brush Size", true) { Font = Content.Fonts.NormalFont };
            var sizeSlider = new Slider
            {
                Text = "Brush Size:",

                Step = 1,
                MinValue = 1,
                MaxValue = 16,
                Value = 1,
            };

            // terrain types
            var types = new ListPanel(wrapMode: ContentWrap.Wrap)
            {
                Width = Size.X,
                ParentAnchor = AnchorMode.Top | AnchorMode.Horizontal,
            };

            foreach (var tty in Enum<TerrainType>.Values)
            {
                var btn = TypeButton(tty);

                btn.IsSelected = (tty == TerrainType.Dirt);
                btn.Selected += (o, e) => controller.TerrainType = tty;

                types.Add(btn);
            }


            // events
            square.MouseClick += (o, e) => controller.Shape = TerrainBrushShape.Square;
            circle.MouseClick += (o, e) => controller.Shape = TerrainBrushShape.Circle;

            sizeSlider.ValueChanged += (_) => controller.Size = sizeSlider.Value;


            AddRange(shapeLabel, shapes, sizeLabel, sizeSlider, types);
        }

        ToggleButton TypeButton(TerrainType tty) => new ToggleButton
        {
            Size = new Vector2(0.22f),
            IconName = null,

            TextFont = Content.Fonts.SmallFont,
            Text = tty.ToString(),
            ToolTip = tty.ToString(),

            StickyToggle = true,
        };

        ToggleButton ShapeButton(string text) => new ToggleButton
        {
            Text = null,
            IconName = text,
            Size = new Vector2(ButtonWidth, ButtonHeight),
            SpriteSizeMode = TextureSizeMode.FitZoom,
            Padding = DefaultPadding,
        };

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        static ListPanel CreateShapes(Action<TerrainBrushShape> handler)
        {

            var text = new Label("Shape:", true);

            var square = new ToggleButton
            {
                Text = "[ ]",
                IsSelected = true,
                Height = text.Height,
            };
            var circle = new ToggleButton
            {
                Text = "( )",
                Height = text.Height,
            };

            square.MouseClick += (o, e) => handler?.Invoke(TerrainBrushShape.Square);
            circle.MouseClick += (o, e) => handler?.Invoke(TerrainBrushShape.Circle);

            return new ListPanel(Direction.LeftToRight, ContentWrap.Wrap)
            {
                text, square, circle
            };
        }

    }
}
