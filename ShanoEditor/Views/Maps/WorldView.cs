using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoEditor.Views.Maps
{
    class WorldView
    {
        /// <summary>
        /// Gets the in-game point that is in the middle of the screen. 
        /// </summary>
        public Vector CenterPoint { get; private set; }

        /// <summary>
        /// Gets the size in pixels of one in-game unit. 
        /// </summary>
        public double UnitSize { get; private set; } = 5;

        public Point MapSize { get; private set; }

        public Point ScreenSize { get; private set; }

        public event Action ScaleChanged;

        /// <summary>
        /// Gets the lower-left point in in-game coordinates. 
        /// </summary>
        public Vector LowLeftPoint
        {
            get
            {
                return CenterPoint - (Vector)ScreenSize / UnitSize / 2;
            }
        }

        public void ResizeScreen(int w, int h)
        {
            ScreenSize = new Point(w, h);
        }

        public void ResizeMap(int w, int h)
        {
            MapSize = new Point(w, h);
            CenterPoint = (Vector)MapSize / 2;
        }

        public void Scale(double delta)
        {
            const double scale = 0.002;

            var factor = 1 + scale * Math.Abs(delta);
            if (delta > 0)
                UnitSize *= factor;
            else
                UnitSize /= factor;

            var maxSz = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            UnitSize = Math.Max(1, Math.Min(maxSz, UnitSize));

            ScaleChanged?.Invoke();
        }

        public void Pan(Point distPixels, Vector min, Vector max)
        {
            var distGame = (Vector)distPixels / UnitSize;
            CenterPoint = (CenterPoint - distGame).Clamp(min, max);
        }

        public Vector GameToControl(Vector gamePos)
        {
            return (Vector)ScreenSize / 2 + (gamePos - CenterPoint) * UnitSize;
        }

        public Vector ControlToGame(Vector controlPos)
        {
            return CenterPoint + (controlPos - (Vector)ScreenSize / 2) / UnitSize;
        }
    }
}
