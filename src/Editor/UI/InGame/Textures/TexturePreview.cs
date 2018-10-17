using Ix.Math;
using Microsoft.Xna.Framework.Input;
using Shanism.Client;
using Shanism.Client.Sprites;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Editor.Controllers.Content;
using System;
using System.Numerics;

namespace Shanism.Editor.UI.InGame.Textures
{
    class TexturePreview : SpriteBox
    {

        // view-model
        Point cellCount;
        Rectangle? selectedCells;
        RectangleF? selectedRect;

        public float LineWidth
        {
            get => Padding * 2;
            set => Padding = value / 2;
        }


        public TexturePreview(TexturesController controller)
        {
            SpriteSizeMode = TextureSizeMode.FitZoom;

            controller.TextureModified += OnTextureModified;

            // dragging left button selects texture
            var selectionStartCell = Point.Zero;

            this.AddMouseDragEvent(MouseButton.Left,
                (o, e) => StartSelect(PxToId(e.Position)),
                (o, e) => ContinueSelect(selectionStartCell, PxToId(e.Position)),
                null
            );

            KeyPress += (o, e) =>
            {
                if (e.Key == Keys.Escape)
                    selectedRect = null;
            };

            bool StartSelect(Point start)
            {
                if (TextureBounds.Size == Vector2.Zero || !((Vector2)start).IsInside(Vector2.Zero, cellCount))
                {
                    selectedCells = Rectangle.Empty;
                    return false;
                }

                selectionStartCell = start;
                ContinueSelect(start, start);
                return true;
            }

            void ContinueSelect(Point a, Point b)
            {
                selectedCells = Rectangle.Normalise(a, b - a)
                    .Inflate(0, 1, 1, 0)
                    .Intersect(new Rectangle(Point.Zero, cellCount));
            }
        }


        Point PxToId(Vector2 pos)
            => ((pos - TextureBounds.Position) * cellCount / TextureBounds.Size).ToPoint();

        Vector2 IdToPx(Point id)
            => id * TextureBounds.Size / cellCount + TextureBounds.Position;

        void OnTextureModified(ShanoTexture tex)
        {
            // only act if changing the currently shown texture
            if (tex.Name != Sprite?.Name)
                return;

            cellCount = Point.Max(tex.CellCount, Point.One);
        }

        public override void Update(int msElapsed)
        {
            if (selectedCells != null && selectedCells.Value.Area > 0)
                selectedRect = ((RectangleF)selectedCells.Value / cellCount) * TextureBounds.Size + TextureBounds.Position;

            base.Update(msElapsed);
        }

        // bind domain to view model
        public void SetTexture(ShanoTexture texture)
        {
            Sprite = new Sprite(texture.Texture, texture.Bounds, texture.Name);

            cellCount = texture.CellCount;
            selectedCells = null;
            selectedRect = null;
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            if (Sprite == null)
                return;

            var wVec = TextureBounds.Size / Vector2.Max(Vector2.One, cellCount) * 0.05f;
            var w = Math.Min(wVec.X, wVec.Y).Clamp(DefaultPadding / 5, DefaultPadding);

            // selection
            if (selectedRect != null)
                c.FillRectangle(selectedRect.Value, Color.DarkBlue.SetAlpha(150));

            // vertical lines
            var (pos, size) = TextureBounds;
            for (int ix = 0; ix <= cellCount.X; ix++)
            {
                var dest = new RectangleF(pos.X + size.X * ix / cellCount.X - w / 2, pos.Y, w, size.Y);
                c.FillRectangle(dest, Color.Goldenrod);
            }
            // horizontal lines
            for (int iy = 0; iy <= cellCount.Y; iy++)
            {
                var dest = new RectangleF(pos.X, pos.Y + size.Y * iy / cellCount.Y - w / 2, size.X, w);
                c.FillRectangle(dest, Color.Goldenrod);
            }
        }
    }
}
