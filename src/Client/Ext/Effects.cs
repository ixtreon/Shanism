using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client
{ 
    static class EffectExt
    {
        readonly static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, -3), new Vector3(0, 0, 0), new Vector3(0, -1, 0));

        public static void SetStaticViewMatrix(this BasicEffect effect)
        {
            effect.View = view;
        }
    }
}
