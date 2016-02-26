﻿using Microsoft.Xna.Framework;
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

        /// <summary>
        /// Sets the alpha value of the color as a number from 0 to 255. 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="a"></param>
        /// <returns></returns>
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
            var ratio = 100 - perc;
            return new Color(c.R * ratio / 100, c.G * ratio / 100, c.B * ratio / 100, c.A);
        }

        /// <summary>
        /// Returns a brighter version of the provided color. 
        /// </summary>
        /// <param name="c">The base color. </param>
        /// <param name="perc">The amount of brightening to apply. Should be an int between 0 and 100. </param>
        /// <returns></returns>
        public static Color Brighten(this Color c, int perc = 5)
        {
            return new Color(
                c.R + (255 - c.R) * perc / 100, 
                c.G + (255 - c.G) * perc / 100, 
                c.B + (255 - c.B) * perc / 100, c.A);
        }
        #endregion


        #region maths primitives transformations
        public static Vector2 ToVector2(this IO.Common.Vector v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

        /// <summary>
        /// Converts this <see cref="IO.Common.Vector"/>
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this IO.Common.Vector v)
        {
            return new Vector3((float)v.X, (float)v.Y, 0);
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


        public static Color ToColor(this IO.Common.ShanoColor c)
        {
            return new Color(c.R, c.G, c.B, c.A);
        }

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
