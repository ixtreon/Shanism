using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO;
using Microsoft.Xna.Framework.Graphics;
using IO.Objects;
using Client.Textures;

namespace Client
{
    public static class GeneralExt
    {

        #region XNA.Color Extensions
        public static Color SetAlpha(this Color c, int a)
        {
            return c * ((float)a / 255);
        }

        /// <summary>
        /// Returns a darker version of the provided color. 
        /// </summary>
        /// <param name="c">The base color. </param>
        /// <param name="perc">The amount of darkening to apply. Should be an int between 0 and 100. </param>
        /// <returns></returns>
        public static Color Darken(this Color c, int perc = 5)
        {
            var ratio = 100 + perc;
            return new Color(c.R * 100 / ratio, c.G * 100 / ratio, c.B * 100 / ratio, c.A);
        }

        /// <summary>
        /// Returns a brighter version of the provided color. 
        /// </summary>
        /// <param name="c">The base color. </param>
        /// <param name="perc">The amount of brightening to apply. Should be an int between 0 and 100. </param>
        /// <returns></returns>
        public static Color Brighten(this Color c, int perc = 5)
        {
            var ratio = 100 - perc;
            return new Color(c.R * 100 / ratio, c.G * 100 / ratio, c.B * 100 / ratio, c.A);
        }
        #endregion


        #region maths primitives transformations
        public static Vector2 ToXnaVector(this IO.Common.Vector v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

        public static IO.Common.Point ToPoint(this Point p)
        {
            return new IO.Common.Point(p.X, p.Y);
        }


        public static Rectangle ToXnaRect(this IO.Common.Rectangle r)
        {
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public static IO.Common.Rectangle ToRect(this Rectangle r)
        {
            return new IO.Common.Rectangle(r.X, r.Y, r.Width, r.Height);
        }
        #endregion


        public static void SyncValues<TKey, TVal>(this IDictionary<TKey, TVal> dict, IEnumerable<TKey> other, 
            Func<TKey, TVal> addValueFactory,
            Action<TKey, TVal> removeValueFactory = null)
        {
            var toRemove = new HashSet<TKey>(dict.Keys);
            toRemove.ExceptWith(other);
            foreach (var k in toRemove)
            {
                removeValueFactory?.Invoke(k, dict[k]);
                dict.Remove(k);
            }

            foreach (var k in other)
                if (!dict.ContainsKey(k))
                {
                    var v = addValueFactory(k);
                    dict.Add(k, v);
                }
        }


    }
}
