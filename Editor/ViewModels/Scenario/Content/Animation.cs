using Shanism.Common.Game;
using Shanism.Common.Content;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap;
using Shanism.Common;

namespace Shanism.Editor.ViewModels
{
    class AnimationViewModel
    {
        TextureViewModel _tex;

        readonly AnimationDef Animation;


        public string Name
        {
            get { return Animation.Name; }
            set
            {
                var oldName = Animation.Name;
                Animation.Name = value;
                NameChanged?.Invoke(this, oldName);
            }

        }

        public Rectangle Span => Animation.Span;

        public bool IsDynamic => Animation.IsDynamic;

        public bool IsLooping => Animation.IsLooping;

        public int Period => Animation.Period;

        public event Action<AnimationViewModel, string> NameChanged;

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
                    Animation.Texture = value.Data.Name;
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
                var szCells = Animation.IsDynamic ? Point.One : Animation.Span.Size;
                return Texture?.CellSize * szCells ?? Point.Zero;
            }
        }

        AnimationViewModel() { }

        public AnimationViewModel(AnimationViewModel @base)
        {
            _tex = @base._tex;
            Animation = new AnimationDef(@base.Animation);
        }

        public AnimationViewModel(TexturesViewModel model, AnimationDef anim)
        {
            if (anim == null) throw new ArgumentNullException(nameof(anim));

            var tex = model.Textures.Values
                .FirstOrDefault(t => t.Path == anim.Texture);

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

            var cellSize = (Vector)Texture.Size / Texture.Data.Splits;

            var oldSmoothingMode = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.DrawImage(Texture.Image,
                dest.ToNetRectangle(),
                (srcBounds * cellSize).ToNetRectangle(),
                System.Drawing.GraphicsUnit.Pixel);
            g.SmoothingMode = oldSmoothingMode;
        }

        public void SetSpan(Rectangle newSpan) => Animation.Span = newSpan;

        /// <summary>
        /// Adds this model to the content config. 
        /// Used when saving the models. 
        /// </summary>
        public void AddToContent(ContentConfig content) => content.Animations.Add(Animation);

        public void SetParams(bool? isDynamic = null, bool? isLooping = null, int? period = null)
        {
            Animation.IsDynamic = isDynamic ?? IsDynamic;
            Animation.IsLooping = isLooping ?? IsLooping;
            Animation.Period = period ?? Period;
        }
    }
}
