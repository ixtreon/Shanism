using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Common.Content;
using Shanism.Editor.ViewModels;

namespace Shanism.Editor.Views.Models
{
    partial class TextureBox : UserControl
    {
        /// <summary>
        /// Gets the current model of this control. 
        /// </summary>
        public TextureViewModel Texture { get; private set; }

        /// <summary>
        /// Gets or sets whether the user can select parts of the texture. 
        /// </summary>
        public bool CanSelectLogical { get; set; }

        public bool StickySelection { get; set; } = true;

        /// <summary>
        /// Gets the current in-texture selection.
        /// </summary>
        public Common.Rectangle Selection { get; private set; } = Common.Rectangle.Empty;

        /// <summary>
        /// Raised whenever the user selection is changed. 
        /// </summary>
        public event Action SelectionChanged;

        //the current, uncommitted selection. 
        Common.Rectangle rawSelection;

        //the bounds of the image within this control
        Common.Point ImagePos, ImageSize;
        Common.Vector ImageCellSize;


        #region Property Shortcuts
        Image Image { get { return Texture.Image; } }

        int LogicalWidth { get { return Texture.Data.Splits.X; } }

        int LogicalHeight { get { return Texture.Data.Splits.Y; } }

        #endregion

        public TextureBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;
        }


        public void SetTexture(TextureViewModel modelView)
        {
            Texture = modelView;

            Invalidate();
        }

        public void SetAnimation(AnimationViewModel anim)
        {
            Texture = anim?.Texture;
            if (anim != null)
                SetSelection(anim.Span, false);
        }

        internal void Reset()
        {
            Texture = null;
            SetSelection(new Common.Rectangle(0, 0, 1, 1), false);
        }


        public void SetSelection(Common.Rectangle rect, bool raiseEvent = true)
        {
            Selection = rect;

            if(raiseEvent)
                SelectionChanged?.Invoke();

            Invalidate();
        }

        bool tryGetCell(Point cursorPos, out Common.Point clampedPos)
        {
            var cursorGamePos = (cursorPos.ToPoint() - ImagePos) / ImageCellSize;
            var cellPos = cursorGamePos.Floor();
            clampedPos = cellPos.Clamp(Common.Point.Zero, Texture.Data.Splits - 1);

            return (cellPos == clampedPos);
        }

        bool isSelecting;

        void startSelect(Point mousePos)
        {
            Common.Point cellPos;

            isSelecting = tryGetCell(mousePos, out cellPos);

            if (isSelecting)
            {
                rawSelection = new Common.Rectangle(cellPos, Common.Point.One);
                Invalidate();
            }
            else if (!StickySelection)
            {
                Selection = Common.Rectangle.Empty;
                SelectionChanged?.Invoke();
                Invalidate();
            }
        }

        void extendSelect(Point mousePos)
        {
            Common.Point cellPos;
            tryGetCell(mousePos, out cellPos);

            rawSelection = new Common.Rectangle(rawSelection.Position, cellPos - rawSelection.Position);

            Selection = rawSelection.Inflate(1, 1);

            Invalidate();
        }

        #region Event Handlers

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left || Texture == null)
                return;

            startSelect(e.Location);

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left || Texture == null)
                return;

            if(isSelecting)
                extendSelect(e.Location);

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left || Texture == null)
                return;

            // Extend and finalise the selection 
            if (isSelecting)
            {
                extendSelect(e.Location);

                Selection = rawSelection.Inflate(1, 1);
                SelectionChanged?.Invoke();
            }

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (Texture == null)
                return;


            //draw image
            var imgRatio = (float)Image.Width / Image.Height;

            ImageSize = new Common.Vector(
                Math.Min(Width, Height * imgRatio),
                Math.Min(Height, Width / imgRatio))
                .ToPoint();

            ImagePos = (Size.ToPoint() - ImageSize) / 2;

            ImageCellSize = (Common.Vector)ImageSize / Texture.Data.Splits; 

            g.DrawImage(Image, ImagePos.X, ImagePos.Y, ImageSize.X, ImageSize.Y);
            

            //draw lines
            using (var p = new Pen(Color.Yellow, 2))
            {
                if (LogicalWidth > 0)
                    foreach (var ix in Enumerable.Range(0, LogicalWidth + 1))
                    {
                        var lx = ImagePos.X + ImageSize.X * ix / LogicalWidth;
                        g.DrawLine(p, lx, ImagePos.Y, lx, (ImagePos + ImageSize).Y);
                    }

                if (LogicalHeight > 0)
                    foreach (var iy in Enumerable.Range(0, LogicalHeight + 1))
                    {
                        var ly = ImagePos.Y + ImageSize.Y * iy / LogicalHeight;
                        g.DrawLine(p, ImagePos.X, ly, (ImagePos + ImageSize).X, ly);
                    }
            }

            //draw selection
            if (CanSelectLogical)
            {
                var selection = Selection;
                using (var br = new SolidBrush(Color.FromArgb(100, 50, 250, 50)))
                {
                    var dest = selection * ImageCellSize + ImagePos;
                    g.FillRectangle(br, dest.X, dest.Y, dest.Width, dest.Height);
                }
            }

            base.OnPaint(e);
        }

        #endregion

    }
}
