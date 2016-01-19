using IO.Common;
using IO.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap;

namespace ShanoEditor.ViewModels
{
    class AnimationViewModel
    {
        TextureViewModel _tex;

        public AnimationDef Animation { get; }

        //public Bitmap Thumbnail { get; private set; }

        /// <summary>
        /// Gets or sets the texture of this animation, if there is one. 
        /// </summary>
        public TextureViewModel Texture
        {
            get { return _tex; }
            set
            {
                if (_tex != value)
                {
                    _tex = value;
                    Animation.Texture = value.Data;
                }
            }
        }

        /// <summary>
        /// Gets the size, in pixels, of this animation. 
        /// </summary>
        public Point Size
        {
            get
            {
                var szCells = Animation.IsDynamic ? new Point(1) : Animation.Span.Size;
                return Texture.CellSize * szCells;
            }
        }

        AnimationViewModel() { }

        public AnimationViewModel(ContentViewModel content, AnimationDef anim)
        {
            if (anim == null) throw new ArgumentNullException(nameof(anim));

            var tex = content.Textures.Values
                .FirstOrDefault(t => t.Path == anim.Texture?.Name);

            _tex = tex;
            Animation = anim;
        }



        public void Paint(System.Drawing.Graphics g, RectangleF dest, int frame = 0)
        {
            var srcBounds = Animation.Span;
            if (Animation.IsDynamic && srcBounds.Width > 0)
            {
                var dx = frame % srcBounds.Width;
                var dy = frame / srcBounds.Width;
                srcBounds = new Rectangle(srcBounds.X + dx, srcBounds.Y + dy, 1, 1);
            }

            var cellSize = (Vector)Texture.FullSize / Texture.Data.Splits;

            var oldSmoothingMode = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.DrawImage(Texture.Image,
                dest.ToNetRectangle(),
                (srcBounds * cellSize).ToNetRectangle(),
                System.Drawing.GraphicsUnit.Pixel);
            g.SmoothingMode = oldSmoothingMode;
        }
    }
}
