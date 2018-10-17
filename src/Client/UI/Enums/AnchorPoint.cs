using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    public enum AnchorPoint
    {
        TopLeft, TopCenter, TopRight,
        CenterLeft, Center, CenterRight,
        BottomLeft, BottomCenter, BottomRight,
    }

    public static class TextAnchorExt
    {
        /// <summary>
        /// Gets a point within the unit square
        /// representing the given <see cref="AnchorPoint"/>.
        /// </summary>
        public static Vector2 GetOffset(this AnchorPoint a)
            => new Vector2((int)a % 3, (int)a / 3) / 2;

        public static float GetXOffset(this AnchorPoint a)
            => ((int)a % 3) * 0.5f;

        public static float GetYOffset(this AnchorPoint a)
            => ((int)a / 3) * 0.5f;
    }
}
