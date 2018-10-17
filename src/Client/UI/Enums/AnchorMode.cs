using System;
using System.Numerics;

namespace Shanism.Client
{
    [Flags]
    public enum AnchorMode
    {
        None       = 0,
        Left       = 1 << 0,    // 1 -> 0,x
        Right      = 1 << 1,    // 2 -> 1,x
        Top        = 1 << 2,    // 4 -> x,0
        Bottom     = 1 << 3,    // 8 -> 0,x
        Horizontal = Left | Right,
        Vertical   = Top | Bottom,
        All        = Left | Right | Bottom | Top,
    }

    public static class AnchorModeExt
    {
        const float lo = 0f;
        const float mid = 0.5f;
        const float hi = 1f;

        static readonly Vector2[] offsets =
        {
            new Vector2(mid, mid),  // 0 = None
            new Vector2(lo, mid),   // 1 = Left
            new Vector2(hi, mid),   // 2 = Right
            new Vector2(mid, mid),  // 3 = Left | Right
            new Vector2(mid, lo),   // 4 = Top
            new Vector2(lo, lo),    // 5 = Top | Left
            new Vector2(hi, lo),    // 6 = Top | Right
            new Vector2(mid, lo),   // 7 = Top | Left | Right
            new Vector2(mid, hi),   // 8 = Bottom
            new Vector2(lo, hi),    // 9 = Bottom | Left
            new Vector2(hi, hi),    // A = Bottom | Right
            new Vector2(mid, hi),   // B = Bottom | Left | Right
            new Vector2(mid, mid),  // C = Bottom | Top
            new Vector2(lo, mid),   // D = Bottom | Top | Left
            new Vector2(hi, mid),   // E = Bottom | Top | Right
            new Vector2(mid, mid),  // F = Bottom | Top | Left | Right
        };

        public static float GetXOffset(this AnchorMode a)
            => offsets[(int)a].X;

        public static float GetYOffset(this AnchorMode a)
            => offsets[(int)a].Y;

        public static Vector2 GetOffset(this AnchorMode a)
            => offsets[(int)a];

    }
}
