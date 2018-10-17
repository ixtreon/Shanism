using Ix.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shanism.Common.Content
{
    public static class Extensions
    {
        public static AnimationDef CreateStaticAnimation(this TextureDef texture, string name, int x, int y)
        {
            return new AnimationDef(name, texture.Name, new Rectangle(x, y, 1, 1));
        }
    }
}
