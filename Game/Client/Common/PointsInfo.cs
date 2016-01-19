using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Common
{
    struct PointsInfo
    {
        const double delta = 1e-5;

        readonly int x;
        readonly int y;
        readonly int w;
        readonly int h;

        public PointsInfo(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }


        public Vector TopLeft
        {
            get
            {
                return new Vector(
                    (x + 0 + delta) / w,
                    (y + 0 + delta) / h);
            }
        }
        public Vector TopRight
        {
            get
            {
                return new Vector(
                    (x + 1 - delta) / w,
                    (y + 0 + delta) / h);
            }
        }
        public Vector BottomLeft
        {
            get
            {
                return new Vector(
                    (x + 0 + delta) / w,
                    (y + 1 - delta) / h);
            }
        }
        public Vector BottomRight
        {
            get
            {
                return new Vector(
                    (x + 1 - delta) / w,
                    (y + 1 - delta) / h);
            }
        }
    }
}
