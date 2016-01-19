using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO.Content;
using ShanoEditor.ViewModels;

namespace ShanoEditor.Views.Models
{
    partial class TextureBox : UserControl
    {
        /// <summary>
        /// Gets the current model of this control. 
        /// </summary>
        public TextureViewModel Model { get; private set; }

        /// <summary>
        /// Gets or sets whether the user can select parts of the texture. 
        /// </summary>
        public bool CanSelectLogical { get; set; }

        /// <summary>
        /// Gets the current selection.
        /// </summary>
        public IO.Common.Rectangle Selection { get; private set; }
            = new IO.Common.Rectangle(0, 0, 1, 1);

        /// <summary>
        /// Raised whenever the user selection is changed. 
        /// </summary>
        public event Action SelectionChanged;

        //the current, uncommitted selection. 
        IO.Common.Rectangle rawSelection;

        //the bounds of the image within this control
        IO.Common.Point ImagePos, ImageSize;
        IO.Common.Vector ImageCellSize;


        #region Property Shortcuts
        Image Image { get { return Model.Image; } }

        int LogicalWidth { get { return Model.Data.Splits.X; } }

        int LogicalHeight { get { return Model.Data.Splits.Y; } }

        #endregion

        public TextureBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;
        }


        public void SetTexture(TextureViewModel modelView)
        {
            Model = modelView;

            Invalidate();
        }

        public void SetTextureSpan(AnimationViewModel anim)
        {
            Model = anim.Texture;
            SetSelection(anim.Animation.Span, false);

            Invalidate();
        }

        internal void Reset()
        {
            Model = null;
            SetSelection(new IO.Common.Rectangle(0, 0, 1, 1), false);
        }


        public void SetSelection(IO.Common.Rectangle rect, bool raiseEvent = true)
        {

            Selection = rect;

            if(raiseEvent)
                SelectionChanged?.Invoke();

            Invalidate();
        }

        IO.Common.Point getCell(Point cursorPos)
        {
            var pos = (cursorPos.ToPoint() - ImagePos) / ImageCellSize;
            pos = pos.Clamp(IO.Common.Point.Zero, Model.Data.Splits - 1);

            return pos.ToPoint();
        }

        void startSelect(Point mousePos)
        {
            var c = getCell(mousePos);
            rawSelection = new IO.Common.Rectangle(c.X, c.Y, 0,0);
            Invalidate();
        }

        void extendSelect(Point mousePos)
        {
            var c = getCell(mousePos);

            rawSelection = new IO.Common.Rectangle(rawSelection.Position, c - rawSelection.Position);

            Selection = rawSelection.Inflate(1, 1);

            Invalidate();
        }

        #region Event Handlers

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left)
                return;

            startSelect(e.Location);

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left)
                return;

            extendSelect(e.Location);

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left)
                return;

            // Extend and finalise the selection 
            extendSelect(e.Location);

            Selection = rawSelection.Inflate(1, 1);
            SelectionChanged?.Invoke();

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (Model == null)
                return;


            //draw image
            var imgRatio = (float)Image.Width / Image.Height;

            ImageSize = new IO.Common.Vector(
                Math.Min(Width, Height * imgRatio),
                Math.Min(Height, Width / imgRatio))
                .ToPoint();

            ImagePos = (Size.ToPoint() - ImageSize) / 2;

            ImageCellSize = (IO.Common.Vector)ImageSize / Model.Data.Splits; 

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
