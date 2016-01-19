using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using System.Windows.Forms;

namespace ShanoEditor
{
    static class Ext
    {

        public static System.Drawing.Rectangle ToNetRectangle(this Rectangle r)
        {
            return new System.Drawing.Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public static System.Drawing.RectangleF ToNetRectangle(this RectangleF r)
        {
            return new System.Drawing.RectangleF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
        }

        public static System.Drawing.Point ToPoint(this Point p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static Point ToPoint(this System.Drawing.Point p)
        {
            return new Point(p.X, p.Y);
        }
        public static Point ToPoint(this System.Drawing.Size p)
        {
            return new Point(p.Width, p.Height);
        }

        public static Vector ToVector(this System.Drawing.SizeF p)
        {
            return new Vector(p.Width, p.Height);
        }


        public static System.Drawing.Color Darken(this System.Drawing.Color a, float ratio = 0.05f)
        {
            return a.MixWith(System.Drawing.Color.Black, ratio);
        }

        public static System.Drawing.Color Lighten(this System.Drawing.Color c, float ratio = 0.05f)
        {
            return c.MixWith(System.Drawing.Color.White, ratio);
        }
        public static System.Drawing.Color SetAlpha(this System.Drawing.Color c, float alpha = 0.05f)
        {
            return System.Drawing.Color.FromArgb((byte)(255 * alpha), c.R, c.G, c.B);
        }

        public static System.Drawing.Color MixWith(this System.Drawing.Color a, System.Drawing.Color b, float perc)
        {
            return System.Drawing.Color.FromArgb(
                lerp(a.A, b.A, perc),
                lerp(a.R, b.R, perc),
                lerp(a.G, b.G, perc),
                lerp(a.B, b.B, perc));
        }

        static byte lerp(byte a, byte b, float ratio)
        {
            return (byte)(a * (1 - ratio) + b * ratio);
        }
    }

    static class ControlExt
    {
        public static bool IsMouseInside(this Control c)
        {
            return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition));
        }
    }
}

