using IO.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sprites
{
    /// <summary>
    /// A static sprite. 
    /// </summary>
    class StaticSprite : Sprite
    {
        public StaticSprite(AnimationDefOld m)
            : base(m)
        {
            this.SourceRectangle = m.SizeAndLocation * new IO.Common.Point(Texture.Width, Texture.Height) / m.File.Splits;
        }
    }
}
