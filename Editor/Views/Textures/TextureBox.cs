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
using System.IO;
using Shanism.Common.Util;

using Color = System.Drawing.Color;

namespace Shanism.Editor.Views.Models
{
    partial class TextureBox : UserControl
    {
        static readonly Common.Rectangle DefaultSelection = new Common.Rectangle(0, 0, 0, 0);


        /// <summary>
        /// Gets the current model of this control. 
        /// </summary>
        TextureViewModel texture;

        bool _isSelecting;

        /// <summary>
        /// Gets or sets whether the user can select parts of the texture. 
        /// </summary>
        public bool CanSelectLogical { get; set; }

        /// <summary>
        /// Gets or sets whether the user can remove the selection. 
        /// </summary>
        public bool StickySelection { get; set; } = true;

        /// <summary>
        /// Gets the current in-texture selection.
        /// </summary>
        public Common.Rectangle Selection { get; private set; } = Common.Rectangle.Empty;

        /// <summary>
        /// Gets or sets whether the context menu for creating new animations 
        /// is shown when right-clicked. 
        /// </summary>
        public bool ContextMenuEnabled { get; set; }


        /// <summary>
        /// Raised whenever the user selection is changed. 
        /// </summary>
        public event Action SelectionChanged;

        /// <summary>
        /// Raised whenever a new animation is created via the context menu. 
        /// </summary>
        public event Action<AnimationDef[]> AnimationsCreated;


        //the current, uncommitted selection. 
        Common.Rectangle rawSelection;

        //the bounds of the image within this control
        Common.Rectangle ImageBounds;

        Common.Vector ImageCellSize;


        #region Property Shortcuts
        Image texImage => texture.Bitmap;

        int LogicalWidth => texture.Data.Cells.X;
        int LogicalHeight => texture.Data.Cells.Y;

        string newAnimName => ShanoPath.GetLastSegment(ShanoPath.RemoveExtension(texture.Name));
        #endregion


        public TextureBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        public void SetTexture(TextureViewModel modelView)
        {
            texture = modelView;
            updateImageBounds();

            setSelection(DefaultSelection, false);
        }


        public void SetAnimation(AnimationViewModel anim)
        {
            texture = anim?.Texture;
            updateImageBounds();

            setSelection(anim?.Span ?? DefaultSelection, false);
        }


        void setSelection(Common.Rectangle rect, bool raiseEvent = true)
        {
            Selection = rect;

            if (raiseEvent)
                SelectionChanged?.Invoke();

            Refresh();
        }

        bool tryGetCell(Point cursorPos, out Common.Point clampedPos)
        {
            var cursorGamePos = (cursorPos.ToPoint() - ImageBounds.Position) / ImageCellSize;
            var cellPos = cursorGamePos.Floor();
            clampedPos = cellPos.Clamp(Common.Point.Zero, texture.Data.Cells - 1);

            return (cellPos == clampedPos);
        }


        void startSelect(Point mousePos)
        {
            Common.Point cellPos;

            _isSelecting = tryGetCell(mousePos, out cellPos);

            if (_isSelecting)
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


            var newSelection = rawSelection.Inflate(1, 1);

            if (newSelection != Selection)
            {
                Selection = newSelection;
                Invalidate();
            }
        }

        #region Event Handlers

        protected override void OnResize(EventArgs e)
        {
            updateImageBounds();
            base.OnResize(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left || texture == null)
                return;

            if (ModifierKeys.HasFlag(Keys.Shift))
                extendSelect(e.Location);
            else
                startSelect(e.Location);

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!CanSelectLogical || e.Button != MouseButtons.Left || texture == null)
                return;

            if (_isSelecting)
                extendSelect(e.Location);

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (texture == null)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (CanSelectLogical && _isSelecting)
                    {
                        extendSelect(e.Location);

                        Selection = rawSelection.Inflate(1, 1);
                        SelectionChanged?.Invoke();
                    }
                    break;

                case MouseButtons.Right:
                    if (ContextMenuEnabled && Selection.Area > 0)
                        texStrip.Show(Cursor.Position);
                    break;
            }

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (texture == null)
                return;

            //draw image
            g.DrawImage(texImage, ImageBounds.X, ImageBounds.Y, ImageBounds.Width, ImageBounds.Height);

            //draw lines
            var penWidth = Math.Min(Width, Height) / 1000f;
            using (var p = new Pen(Color.Yellow, penWidth))
            {
                if (LogicalWidth > 0)
                    foreach (var ix in Enumerable.Range(0, LogicalWidth + 1))
                    {
                        var lx = ImageBounds.X + ImageBounds.Width * ix / LogicalWidth;
                        g.DrawLine(p, lx, ImageBounds.Bottom, lx, ImageBounds.Top);
                    }

                if (LogicalHeight > 0)
                    foreach (var iy in Enumerable.Range(0, LogicalHeight + 1))
                    {
                        var ly = ImageBounds.Y + ImageBounds.Height * iy / LogicalHeight;
                        g.DrawLine(p, ImageBounds.Left, ly, ImageBounds.Right, ly);
                    }
            }

            //draw selection
            if (CanSelectLogical)
            {
                var selection = Selection;
                using (var br = new SolidBrush(Color.FromArgb(100, 50, 250, 50)))
                {
                    var dest = selection * ImageCellSize + ImageBounds.Position;
                    g.FillRectangle(br, dest.X, dest.Y, dest.Width, dest.Height);
                }
            }

            base.OnPaint(e);
        }
        #endregion

        void updateImageBounds()
        {
            if (texture == null || texImage == null)
            {
                ImageBounds = Common.Rectangle.Empty;
                ImageCellSize = Common.Vector.Zero;
                return;
            }

            var imgRatio = (float)texImage.Width / texImage.Height;
            var w = (int)Math.Min(Width, Height * imgRatio);
            var h = (int)Math.Min(Width / imgRatio, Height);
            var x = (Width - w) / 2;
            var y = (Height - h) / 2;
            ImageBounds = new Common.Rectangle(x, y, w, h);
            ImageCellSize = (Common.Vector)ImageBounds.Size / texture.Data.Cells;
        }
        

        void aDynamicAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create a single, dynamic animation; each cell becomes a frame
            var anim = new AnimationDef(newAnimName, texture.Name, Selection, 100);
            AnimationsCreated?.Invoke(new[] { anim });
        }

        void aStaticAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create a single, static animation spanning the selected cells
            var anim = new AnimationDef(newAnimName, texture.Name, Selection);
            AnimationsCreated?.Invoke(new[] { anim });
        }

        void multipleStaticAnimationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create an animation out of each cell
            var anims = Selection.Iterate()
                .Select(pt => new AnimationDef($"{newAnimName}-{pt.X}-{pt.Y}", texture.Name, new Common.Rectangle(pt.X, pt.Y, 1, 1)))
                .ToArray();

            AnimationsCreated?.Invoke(anims);
        }

    }
}
