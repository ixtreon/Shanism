using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Content
{
    public class AnimationDef
    {
        public TextureDef Texture { get; set; }

        public Rectangle Span { get; set; }

        public AnimationDef()
        {
            //Frames = frames;
        }
    }
}
