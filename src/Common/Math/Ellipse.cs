using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// A 2D axis-aligned ellipse.
    /// </summary>
    public struct Ellipse
    {
        public Vector Center { get; }
        public Vector Radius { get; }

        public Ellipse(Vector center, float radius)
        {
            Center = center;
            Radius = new Vector(radius);
        }

        public Ellipse(Vector center, Vector radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool IsInside(Vector v)
        {
            var d = (v - Center) / Radius;
            return d.LengthSquared() < 1;
        }
    }
}
