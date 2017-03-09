using Shanism.Common.Content;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap;
using Shanism.Common;
using System.ComponentModel;
using Shanism.Editor.Utils;
using Shanism.Editor.Util;

namespace Shanism.Editor.ViewModels
{
    class AnimationViewModel
    {
        TextureViewModel _texture;

        readonly AnimationDef Animation;

        [Description("The in-game name of this animation.")]
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


        [Description("The cells this animation spans within its texture. "
            + "\n" + "Of the format [X, Y, W, H]. ")]
        public Rectangle Span => Animation.Span;


        [Description("Determines whether this animation is dynamic or static."
            + "\n" + "Static animations treat their selection as a single object."
            + "\n" + "Dynamic animations treat their selection as a series of frames. ")]
        public bool IsDynamic
        {
            get { return Animation.IsDynamic; }
            set
            {
                Animation.IsDynamic = value;
            }
        }

        [Description("Describes the way this animation is displayed in-game."
            //+ "\n    * Fixed: The texture is never rotated or flipped"
            //+ "\n    * FullSizeLeft: The texture is never rotated or flipped"
            //+ "\n    * FullSizeRight: The texture is never rotated or flipped"
            //+ "\n    * Fixed: The texture is never rotated or flipped"
            )]
        public AnimationStyle Style
        {
            get { return Animation.RotationStyle; }
            set
            {
                Animation.RotationStyle = value;
            }
        }

        [Description("Controls the delay before the next frame of the animation is shown."
            + "\n" + "Only valid if the animation is dynamic. ")]
        [Range(10, 60000, 10)]
        public int Period
        {
            get { return Animation.Period; }
            set
            {
                Animation.Period = value;
            }
        }

        public event Action<AnimationViewModel, string> NameChanged;

        /// <summary>
        /// Gets or sets the texture of this animation, if there is one. 
        /// </summary>
        [TypeConverter(typeof(TextureTypeConverter))]
        [DisplayName("Texture")]
        [Description("The texture source of this animation.")]
        public TextureViewModel Texture
        {
            get { return _texture; }
            set
            {
                if (_texture != value)
                {
                    _texture = value;
                    Animation.Texture = value?.Data.Name;
                }
            }
        }

        public double? AspectRatio
        {
            get
            {
                if (Texture == null)
                    return null;
                var sz = Texture.CellSize * (Animation.IsDynamic ? Point.One : Animation.Span.Size);
                return (double)sz.X / sz.Y;
            }
        }


        AnimationViewModel() { }

        public AnimationViewModel(AnimationViewModel @base)
        {
            _texture = @base._texture;
            Animation = new AnimationDef(@base.Animation);
        }

        public AnimationViewModel(TexturesViewModel model, AnimationDef anim)
        {
            if (anim == null) throw new ArgumentNullException(nameof(anim));

            var tex = model.Textures
                .FirstOrDefault(t => t.Name == anim.Texture);

            _texture = tex;
            Animation = anim;
        }


        public void Paint(System.Drawing.Graphics g, Vector[] dest, int frame = 0)
        {
            if (Animation == null)
                return;

            var srcBounds = Animation.Span;
            if (Animation.IsDynamic && srcBounds.Area != 0)
            {
                var dx = frame % srcBounds.Width;
                var dy = frame / srcBounds.Width;
                srcBounds = new Rectangle(srcBounds.X + dx, srcBounds.Y + dy, 1, 1);
            }

            var cellSize = (Vector)Texture.ImageSize / Texture.Data.Cells;
            var vs = dest
                .Select(v => v.ToNetVector())
                .ToArray();

            var oldSmoothingMode = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.DrawImage(Texture.Bitmap,
                vs,
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

        public void SetParams(bool? isDynamic = null, AnimationStyle? rotateStyle = null, int? period = null)
        {
            Animation.IsDynamic = isDynamic ?? IsDynamic;
            Animation.RotationStyle = rotateStyle ?? Style;
            Animation.Period = period ?? Period;
        }
    }
}
