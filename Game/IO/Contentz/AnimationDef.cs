using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Content
{
    public class AnimationDef
    {
        public readonly FrameDef[] Frames;

        public AnimationDef(params FrameDef[] frames)
        {
            Frames = frames;
        }
    }
}
