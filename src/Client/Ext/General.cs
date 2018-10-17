using System;
using System.Collections.Generic;

using XnaColor = Microsoft.Xna.Framework.Color;
using XnaPoint = Microsoft.Xna.Framework.Point;
using XnaVector = Microsoft.Xna.Framework.Vector2;
using XnaVector3 = Microsoft.Xna.Framework.Vector3;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

using ShanoColor = Shanism.Common.Color;
using ShanoPoint = Ix.Math.Point;
using ShanoVector = System.Numerics.Vector2;
using ShanoRect = Ix.Math.Rectangle;
using Shanism.Client.IO;
using Microsoft.Xna.Framework.Input;

namespace Shanism.Client
{
    public static class ColorExt
    {
        public static XnaColor ToXnaColor(this ShanoColor c)
            => new XnaColor(c.R, c.G, c.B, c.A);
    }
    
    public static class MathsExt
    {
        // Vector Conversion
        public static XnaVector ToXnaVector(this ShanoVector v)
            => new XnaVector(v.X, v.Y);
        public static XnaVector3 ToXnaVector3(this ShanoVector v)
            => new XnaVector3(v.X, v.Y, 0);

        // Point Conversion
        public static ShanoPoint ToPoint(this XnaPoint p)
            => new ShanoPoint(p.X, p.Y);
        public static XnaPoint ToXnaPoint(this ShanoPoint p)
            => new XnaPoint(p.X, p.Y);

        // Rect Conversion
        public static ShanoRect ToRect(this XnaRect r)
            => new ShanoRect(r.X, r.Y, r.Width, r.Height);
        public static XnaRect ToXnaRect(this ShanoRect r)
            => new XnaRect(r.X, r.Y, r.Width, r.Height);

    }

    public static class GeneralExt
    {

        public static bool IsModifier(this Keys k) => k >= Keys.LeftShift && k <= Keys.RightAlt;


        public static void SyncValues<TKey, TVal>(this IDictionary<TKey, TVal> dict, 
            IEnumerable<TKey> allNewVals,
            Func<TKey, TVal> addValueFactory,
            Action<TKey, TVal> removeValueFactory)
        {
            // invalidate current
            var toRemove = new HashSet<TKey>(dict.Keys);

            // add or re-validate new nodes
            foreach (var k in allNewVals)
                if (dict.ContainsKey(k))
                    toRemove.Remove(k);
                else
                    dict.Add(k, addValueFactory(k));

            //remove invalidated ones
            foreach (var k in toRemove)
            {
                removeValueFactory(k, dict[k]);
                dict.Remove(k);
            }
        }
    }
}
