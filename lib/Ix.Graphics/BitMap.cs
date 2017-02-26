using System;
using System.Numerics;
using System.Text;

namespace Ix.Graphics
{
    class BitMap
    {
        public static bool IsHardwareAccelerated => Vector.IsHardwareAccelerated;

        public static int VectorWidth { get; } = Vector<byte>.Count;

        static readonly Vector<byte> vFull;
        static readonly Vector<byte>[] vStarts, vEnds;

        static BitMap()
        {
            vStarts = new Vector<byte>[VectorWidth];
            vEnds = new Vector<byte>[VectorWidth];

            vFull = genBytes(VectorWidth, VectorWidth);

            for (int i = 0; i < vEnds.Length; i++)
            {
                vEnds[i] = genBytes(i, VectorWidth);
                vStarts[i] = ~vEnds[i];
            }
        }

        static Vector<byte> genBytes(int nFull, int nTotal)
        {
            var b = new byte[nTotal];
            for (int i = 0; i < nFull; i++)
                b[i] = byte.MaxValue;
            return new Vector<byte>(b);
        }



        public int Width { get; }

        public int Height { get; }


        readonly int VectorCount;

        Vector<byte>[,] canvas;

        public BitMap(int w, int h)
        {
            Width = w; Height = h;
            VectorCount = (int)Math.Ceiling((float)w / VectorWidth);

            canvas = new Vector<byte>[VectorCount, Height];
        }

        public void Clear()
        {
            for (int x = 0; x < VectorCount; x++)
                for (int y = 0; y < Height; y++)
                    canvas[x, y] = Vector<byte>.Zero;
        }


        public void DrawRectangle(int x, int y, int w, int h)
        {
            var vxStart = x / VectorWidth;
            var vxEnd = (x + w) / VectorWidth;

            var startOffset = (x % VectorWidth);
            var endOffset = ((x + w) % VectorWidth);

            var startMask = vStarts[startOffset];
            var endMask = vEnds[endOffset];

            if (vxStart == vxEnd)
            {
                var combinedMask = startMask & endMask;
                for (int iy = 0; iy < h; iy++)
                    canvas[vxStart, y + iy] |= combinedMask;
            }
            else
            {
                for (int iy = 0; iy < h; iy++)
                    fillMultiLine(vxStart, vxEnd, startMask, endMask, y + iy);
            }
        }

        void fillLine(int x, int y, int w)
        {
            var vxStart = x / VectorWidth;
            var vxEnd = (x + w) / VectorWidth;

            var startOffset = (x % VectorWidth);
            var endOffset = ((x + w) % VectorWidth);

            var startMask = vStarts[startOffset];
            var endMask = vEnds[endOffset];

            if (vxStart == vxEnd)
            {
                var combinedMask = startMask & endMask;
                canvas[vxStart, y] |= combinedMask;
            }
            else
            {
                fillMultiLine(vxStart, vxEnd, startMask, endMask, y);
            }
        }


        public void DrawCircle(int cx, int cy, int r)
        {
            fillLine(cx - r, cy, 2 * r);

            int dx;
            for (int dy = 1; dy < r; dy++)
            {
                dx = (int)(Math.Sqrt(r * r - dy * dy) + 0.5);

                fillLine(cx - dx, cy - dy, 2 * dx);
                fillLine(cx - dx, cy + dy, 2 * dx);
            }
        }

        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            //order by y
            order(ref x1, ref y1, ref x2, ref y2);
            order(ref x2, ref y2, ref x3, ref y3);
            order(ref x1, ref y1, ref x2, ref y2);

            //mutual y-dist
            var d13 = y3 - y1;
            var d12 = y2 - y1;
            var d23 = y3 - y2;

            var mx = lerp(x1, x3, (float)(y2 - y1) / d13);
            var smaller = x2 < mx;

            //lower triangle: p1 to p2
            for (int iy = y1; iy < y2; iy++)
            {
                var xa = lerp(x1, x3, (float)(iy - y1) / d13);
                var xb = lerp(x1, x2, (float)(iy - y1) / d12);
                if (smaller)
                    fillLine(xb, iy, xa - xb);
                else
                    fillLine(xa, iy, xb - xa);
            }

            //upper triangle: p2 to p3
            for (int iy = y2; iy < y3; iy++)
            {
                var xa = lerp(x1, x3, (float)(iy - y1) / d13);
                var xb = lerp(x2, x3, (float)(iy - y2) / d23);
                if (smaller)
                    fillLine(xb, iy, xa - xb);
                else
                    fillLine(xa, iy, xb - xa);
            }
        }

        void order(ref int x1, ref int y1, ref int x2, ref int y2)
        {
            if (y1 < y2)
                return;

            var tx = x1;
            var ty = y1;

            x1 = x2;
            y1 = y2;

            x2 = tx;
            y2 = ty;
        }

        void fillMultiLine(int vxStart, int vxEnd, Vector<byte> startMask, Vector<byte> endMask, int y)
        {
            canvas[vxStart, y] |= startMask;
            for (int ix = vxStart + 1; ix < vxEnd; ix++)
                canvas[ix, y] |= vFull;
            canvas[vxEnd, y] |= endMask;
        }

        public byte GetPixel(int x, int y)
        {
            var vx = x / VectorWidth;
            var vo = x % VectorWidth;
            return canvas[vx, y][vo];
        }



        static int lerp(int a, int b, float r)
            => (int)(a + (b - a) * r + 0.5f);


        public override string ToString()
        {
            var sb = new StringBuilder(Width * Height);
            for (int iy = 0; iy < Height; iy++)
            {
                for (int ix = 0; ix < Width; ix++)
                {
                    if (GetPixel(ix, iy) != 0)
                        sb.Append('W');
                    else if (ix % VectorWidth == 0)
                        sb.Append(':');
                    else

                        sb.Append('.');
                }

                sb.AppendLine();
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

    }
}
