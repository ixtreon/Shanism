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
    static class EffectExt
    {
        readonly static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, -3), new Vector3(0, 0, 0), new Vector3(0, -1, 0));
        static readonly Matrix projection = Matrix.CreateOrthographic(Constants.Client.WindowWidth, Constants.Client.WindowHeight, -5, 5);

        public static void Set2DMatrices(this BasicEffect effect)
        {
            effect.View = view;
            //effect.Projection = projection;
        }
    }
}
