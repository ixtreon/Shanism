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
    public static class Ext
    {
        public static Color SetAlpha(this Color c, int a)
        {
            return new Color(c.R, c.G, c.B, a);
        }
        public static Color Darken(this Color c, int ratio = 5)
        {
            return new Color(c.R * 100 / (100 + ratio), c.G * 100 / (100 + ratio), c.B * 100 / (100 + ratio), c.A);
        }

        //xna <-> common vector transforms 

        public static Vector2 ToVector2(this IO.Common.Vector v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

        public static IO.Common.Vector ToVector(this Vector2 v)
        {
            return new IO.Common.Vector(v.X, v.Y);
        }

        public static Point ToXnaPoint(this Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        //xna <-> common rectangle transforms

        public static IO.Common.Rectangle ToCommonRect(this Rectangle r)
        {
            return new IO.Common.Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public static Rectangle ToXnaRect(this IO.Common.Rectangle r)
        {
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }



        public static Point DivideBy(this Point p, int divisor)
        {
            return new Point(p.X / divisor, p.Y / divisor);
        }

        public static Point Add(this Point p, int dx, int dy)
        {
            return new Point(p.X + dx, p.Y + dy);
        }

        /// <summary>
        /// Constrains the first point between the other two. 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static Point ConstrainWithin(this Point p, Point low, Point high)
        {
            var x = Math.Min(high.X, Math.Max(low.X, p.X));
            var y = Math.Min(high.Y, Math.Max(low.Y, p.Y));
            return new Point(x, y);
        }

        public static Texture2D GetIconTexture(this IBuffInstance buff)
        {
            return TextureCache.Get(TextureType.Icon, buff.Icon);
        }

        public static void SyncValues<TKey, TVal>(this IDictionary<TKey, TVal> dict, IEnumerable<TKey> other, Func<TKey, TVal> addValueFactory)
        {
            var toRemove = new HashSet<TKey>(dict.Keys);
            TVal val;
            foreach (var k in other)
            {
                if (dict.TryGetValue(k, out val))
                    toRemove.Remove(k);
                else
                    dict.Add(k, addValueFactory(k));
            }

            foreach(var k in toRemove)
            {
                dict.Remove(k);
            }
        }


    }


    static class EffectExt
    {
        readonly static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, -3), new Vector3(0, 0, 0), new Vector3(0, -1, 0));
        static Matrix projection = Matrix.CreateOrthographic(Constants.Client.WindowWidth, Constants.Client.WindowHeight, -5, 5);

        public static void Set2DMatrices(this BasicEffect effect)
        {
            effect.View = view;
            effect.Projection = projection;
        }
    }
}
