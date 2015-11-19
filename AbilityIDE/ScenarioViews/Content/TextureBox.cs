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

namespace ShanoEditor.ScenarioViews.Models
{
    public partial class TextureBox : UserControl
    {
        static Bitmap tryLoad(TextureDef tex)
        {
            try
            {
                return (Bitmap)Image.FromFile(tex.Name);
            }
            catch
            {
                return null;
            }
        }


        public Image Image { get; private set; }

        public TextureDef Model { get; private set; }


        public void SetTexture(TextureViewModel modelView)
        {
            Model = modelView.Data;
            Image = modelView.Image ?? tryLoad(Model);

            Invalidate();
        }

        public void SetTextureSpan(TextureViewModel modelView, IO.Common.Rectangle selection)
        {
            Model = modelView.Data;
            Image = modelView.Image;

            SetSelection(selection, false);

            Invalidate();
        }

        internal void ResetTexture()
        {
            Model = null;
            Image = null;
        }

        //public void SetTexture(AnimationDef anim)
        //{
        //    Model = anim.Texture;
        //    Image = tryLoad(anim.Texture);
        //    SetSelection(anim.Span);

        //    Invalidate();
        //}

        public int LogicalWidth
        {
            get { return Model.Splits.X; }
        }

        public int LogicalHeight
        {
            get { return Model.Splits.Y; }
        }

        public bool CanSelectLogical { get; set; }


        IO.Common.Rectangle rawSelection;

        public IO.Common.Rectangle Selection { get; private set; } 
            = new IO.Common.Rectangle(0, 0, 1, 1);

        public event Action SelectionChanged;

        float ImageX, ImageY, ImageWidth, ImageHeight;

        float CellWidth
        {
            get { return ImageWidth / LogicalWidth; }
        }

        float CellHeight
        {
            get { return ImageHeight / LogicalHeight; }
        }


        public TextureBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;
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
            var x = (int)((cursorPos.X - ImageX) / CellWidth);
            var y = (int)((cursorPos.Y - ImageY) / CellHeight);

            x = Math.Max(0, Math.Min(LogicalWidth - 1, x));
            y = Math.Max(0, Math.Min(LogicalHeight - 1, y));
            return new IO.Common.Point(x, y);
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

            if (Image == null)
                return;


            //draw image
            var imgRatio = (float)Image.Width / Image.Height;

            ImageWidth = Math.Min(Width, Height * imgRatio);
            ImageHeight = Math.Min(Height, Width / imgRatio);
            ImageX = (Width - ImageWidth) / 2;
            ImageY = (Height - ImageHeight) / 2;

            g.DrawImage(Image, ImageX, ImageY, ImageWidth, ImageHeight);

            if (Model == null)
                return;

            //draw lines
            using (var p = new Pen(Color.Yellow, 2))
            {
                if (LogicalWidth > 0)
                    foreach (var ix in Enumerable.Range(0, LogicalWidth + 1))
                    {
                        var lx = ImageX + ImageWidth * ix / LogicalWidth;
                        g.DrawLine(p, lx, ImageY, lx, ImageY + ImageHeight);
                    }

                if (LogicalHeight > 0)
                    foreach (var iy in Enumerable.Range(0, LogicalHeight + 1))
                    {
                        var ly = ImageY + ImageHeight * iy / LogicalHeight;
                        g.DrawLine(p, ImageX, ly, ImageX + ImageWidth, ly);
                    }
            }

            //draw selection
            if (CanSelectLogical)
            {
                var selection = Selection;
                using (var br = new SolidBrush(Color.FromArgb(100, 50, 50, 250)))
                {
                    var x = ImageX + selection.X * CellWidth;
                    var y = ImageY + selection.Y * CellHeight;
                    var w = selection.Width * CellWidth;
                    var h = selection.Height * CellHeight;
                    g.FillRectangle(br, x, y, w, h);
                }
            }

            base.OnPaint(e);
        }
    }
}
