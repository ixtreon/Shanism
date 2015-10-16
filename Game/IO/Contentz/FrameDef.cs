using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IxSerializer.Attributes;
using IO.Common;

namespace IO.Content
{

    /// <summary>
    /// Represents an animation definition as used by model definitions (see <see cref="ModelDef"/>) to display object animations. 
    /// </summary>
    [SerialKiller]
    public class FrameDef
    {

        /// <summary>
        /// Gets the texture of this frame. 
        /// </summary>
        [SerialMember]
        public readonly TextureDef Texture;

        /// <summary>
        /// Gets the span of this frame relative to its texture. 
        /// </summary>
        [SerialMember]
        public readonly Rectangle Span;


        FrameDef() { }

        /// <summary>
        /// Creates a new frame spanning the whole texture. 
        /// </summary>
        /// <param name="tex">The underlying texture of the frame. </param>
        public FrameDef(TextureDef tex)
        {
            Texture= tex;
            Span = new Rectangle(0, 0, tex.Splits.X, tex.Splits.Y);
        }


        /// <summary>
        /// Creates a new frame from the given texture, spanning a single logical cell in it. 
        /// </summary>
        /// <param name="tex">The underlying texture of the frame. </param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public FrameDef(TextureDef tex, int x, int y)
        {
            if (x < 0 || x >= tex.Splits.X) throw new ArgumentException(nameof(x), "Texture isn't as wide as you thought!");
            if (y < 0 || y >= tex.Splits.Y) throw new ArgumentException(nameof(x), "Texture isn't as high as you thought!");

            Texture = tex;
            Span = new Rectangle(x, y, 1, 1);
        }


        /// <summary>
        /// Creates a new frame from the given texture, spanning the specified logical rectangle in it. 
        /// </summary>
        /// <param name="tex">The underlying texture of the frame. </param>
        /// <param name="span"></param>
        public FrameDef(TextureDef tex, Rectangle span)
        {
            if (span.Left < 0 || span.Bottom < 0 || span.Right > tex.Splits.X || span.Top > tex.Splits.Y)
                throw new ArgumentException(nameof(span), "Span extends outside the underlying texture!");

            Texture = tex;
            Span = span;
        }
    }
}
