using Shanism.Client;
using Shanism.Common;
using Shanism.Engine;
using Shanism.ScenarioLib;

namespace Shanism.Editor.Models.Brushes
{
    sealed class ObjectBrush : Brush
    {
        static readonly ObjectCreator objectCreator;

        public ObjectBrush(BrushArgs args) 
            : base(args, MouseButton.Left)
        {
        }

        public Color CursorColor { get; set; } = Color.RoyalBlue.SetAlpha(100);

        public ObjectConstructor Entity { get; set; }

        public override void Draw(Canvas c)
        {
        }
    }
}
